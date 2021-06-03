using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Entities;
using City = DAL.City;
using Feedback = DAL.Feedback;
using ParkingSpot = DAL.ParkingSpot;
using ParkingSpotSearch = DAL.ParkingSpotSearch;
using PaymentDetail = DAL.PaymentDetail;
using User = DAL.User;
using WeekDay = DAL.WeekDay;

namespace BL
{
    public class UserBL : DbHandler
    {
        DBConnection dBConnection;
        FeedbackBL fbl;
        ParkingSpotsBL parkingSpotsBL;
        PaymentDetailsBL paymentDetailsBL;
        SearchRequestsBL searchRequestsBL;
        public UserBL()
        {
            dBConnection = new DBConnection();
            fbl = new FeedbackBL();
            parkingSpotsBL = new ParkingSpotsBL();
            paymentDetailsBL = new PaymentDetailsBL();
            searchRequestsBL = new SearchRequestsBL();
        }

        //sign up function - returns user code
        public int SignUp(Entities.User user)
        {
            //if this username already exists
            if (!DbHandler.GetAll<User>().Any(d => d.Username.Trim() == user.Username.Trim()))
            {
                AddSet<User>(DAL.Converts.UserConvert.ConvertUserToEF(user));
                return DbHandler.GetAll<User>().Where(u => u.Username == user.Username && u.UserPassword == user.UserPassword).Select(c => c.Code).ToList()[0];
            }

            return 0;
        }
       
        // login function - returns user code
        public int Login(string username, string password)
        {
            return dBConnection.GetUserCode(username, password);
        }
        // update user function. returns code if succeeds.
        public int UpdateUser(Entities.User user)
        {

            if (dBConnection.GetUserCode(user.Username, user.UserPassword) != 0)
            {
                UpdateSet<User>(DAL.Converts.UserConvert.ConvertUserToEF(user));
                return user.Code;
            }
            return 0;
        }
        //deletes a user returns 1 if succeeds
        public int DeleteUser(Entities.User u)
        {
            //success
            int result = 0;
            if (paymentDetailsBL.DeletePaymentDetailsByUser(u) == 1)
            {
                if (parkingSpotsBL.DeleteParkingSpotByUser(u) == 1)
                {
                    if (searchRequestsBL.DeletePSSearchesByUser(u) == 1)
                    {
                        if (fbl.DeleteFeedbacksByUser(u.Code) == 1)
                            result = 1;
                    }
                }
            }
            DeleteSet<User>(GetAll<User>().First(y => y.Code == u.Code));
            return result;
        }
        //checking if user's rating is fine, if not, deletes
        public int checkUserAvRating(int usercode)
        {
            var feedbacks = DAL.Converts.FeedbackConvert.ConvertFeedbacksListToEntity(GetAll<Feedback>().Where(g => g.DescriptedUserCode == usercode).ToList());
            int sum_raiting = 0;
            if ((feedbacks != null) || (feedbacks.Count() != 0))
            {
                foreach (var f in feedbacks)
                {
                    sum_raiting += (int)(f.Rating);
                }
                if ((sum_raiting / feedbacks.Count()) < 2)
                {
                    Entities.User user = DAL.Converts.UserConvert.ConvertUserToEntity(GetAll<User>().First(u => u.Code == usercode));
                    Email.MailToUserDeletingUser(user);
                    if (DeleteUser(user) == 1)
                        return 1;
                }
                return 0;
            }

            else
                return 1;
            
        }

    }
}
