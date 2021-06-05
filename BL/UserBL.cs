using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using DAL;
using User = DAL.User;

namespace BL
{
    public class UserBL : DbHandler
    {
        FeedbackBL fbl;
        ParkingSpotsBL parkingSpotsBL;
        PaymentDetailsBL paymentDetailsBL;
        SearchRequestsBL searchRequestsBL;
        public UserBL()
        {
            //fbl = new FeedbackBL();
            parkingSpotsBL = new ParkingSpotsBL();
            paymentDetailsBL = new PaymentDetailsBL();
            searchRequestsBL = new SearchRequestsBL();
        }

        //sign up function - returns user code
        public int SignUp(Entities.User user)
        {
            //if this username already exists
            if (!DAL.Convert.UserConvert.ConvertUsersListToEntity(GetAll<User>()).Any(d => d.Username.Trim() == user.Username.Trim()))
            {
                AddSet<User>(DAL.Convert.UserConvert.ConvertUserToEF(user));
                return (DAL.Convert.UserConvert.ConvertUsersListToEntity(GetAll<User>()).First(u => u.Username == user.Username && u.UserPassword == user.UserPassword)).Code;
            }

            return 0;
        }
       
        // login function - returns user code
        public int Login(string username, string password)
        {
            return GetUserCode(username, password);
        }
        // update user function. returns code if succeeds.
        public int UpdateUser(Entities.User user)
        {

            if (GetUserCode(user.Username, user.UserPassword) != 0)
            {
                UpdateSet<User>(DAL.Convert.UserConvert.ConvertUserToEF(user));
                return user.Code;
            }
            return 0;
        }
        //deletes a user returns 1 if succeeds
        public int DeleteUser(Entities.User u)
        {
            //success var
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
            var feedbacks = DAL.Convert.FeedbackConvert.ConvertFeedbacksListToEntity(GetAll<DAL.Feedback>().Where(g => g.DescriptedUserCode == usercode).ToList());
            int sum_raiting = 0;
            if ((feedbacks != null) || (feedbacks.Count() != 0))
            {
                foreach (var f in feedbacks)
                {
                    sum_raiting += (int)(f.Rating);
                }
                if ((sum_raiting / feedbacks.Count()) < 2)
                {
                    Entities.User user = DAL.Convert.UserConvert.ConvertUsersListToEntity(GetAll<User>()).First(u => u.Code == usercode);
                    Email.MailToUserDeletingUser(user);
                    if (DeleteUser(DAL.Convert.UserConvert.ConvertUserToEF(user)) == 1)
                        return 1;
                }
                return 0;
            }

            else
                // because he doesn't have any feedbacks yet
                return 1;
            
        }

        private int DeleteUser(User user)
        {
            throw new NotImplementedException();
        }

        //gets user code by username and password
        public int GetUserCode(string userName, string password)
        {
            if (GetAll<User>().Any(u => u.Username == userName && u.UserPassword == password))
                return (GetAll<User>().First(u => u.Username == userName && u.UserPassword == password)).Code;
            return 0;//DAL.Converts.UserConvert.ConvertUserToEntity
        }
        // function that sends email to user
        public int SendEmail()
        {
            SendMail sendMail = new SendMail("blala", "shoshy.ustinov43770@gmail.com");
            string body = "";
            string subject = string.Format(" אימות סיסמא למשתמש {0}", "Shoshy");
            body += "\nלתשומת לבך, מצורפת סיסמתך החדשה לכניסה למערכת";
            body += string.Format(" :סיסמתך החדשה היא {0}", "56785");
            //מבצע את השליחה
            bool mailSend = sendMail.SendEMail(new MessageGmail()
            {
                sendTo = "tamid.lehajeh@gmail.com",
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            });

            return 0;
        }


    }
}
