﻿using System;
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

        public int ConfirmValCode(string username, string valcode)
        {
            Entities.User user = DAL.Convert.UserConvert.ConvertUsersListToEntity(GetAll<User>()).First(u => u.Username == username.Trim());
            if (user.UserPassword == valcode)
                return user.Code;
            return 0;

        }

        public int NewPassword(int usercode, string newPassword)
        {
            Entities.User user = DAL.Convert.UserConvert.ConvertUsersListToEntity(GetAll<User>()).First(u => u.Code == usercode);
            user.UserPassword = newPassword;
            UpdateSet<User>(DAL.Convert.UserConvert.ConvertUserToEF(user));
            return GetUserCode(user.Username, user.UserPassword);

        }

        // login function - returns user code
        public int Login(string username, string password)
        {
            int uc = GetUserCode(username, password);
            if (uc == 0)
            {
                if (!Password(password, username))
                    // incorrect password
                    return 2;
            }
            return uc;


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
            if (DAL.Convert.UserConvert.ConvertUsersListToEntity(GetAll<User>()).Any(u => u.Username == userName && u.UserPassword == password))
                return (DAL.Convert.UserConvert.ConvertUsersListToEntity(GetAll<User>()).First(u => u.Username == userName && u.UserPassword == password)).Code;
            return 0;
        }
        // function that sends email to user
        public int SendEmail(string email, string username, string subject, string body)
        {
            SendMail sendMail = new SendMail(username, "parkmarkapp2021@gmail.com");

            //subject = string.Format(" אימות סיסמא למשתמש {0}", "Shoshy");
            //body += "\nלתשומת לבך, מצורפת סיסמתך החדשה לכניסה למערכת";
            // body += string.Format(" :סיסמתך החדשה היא {0}", "56785");
            //מבצע את השליחה
            bool mailSend = sendMail.SendEMail(new MessageGmail()
            {
                sendTo = email,
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            });

            return 0;
        }
        // password verification
        public bool Password(string password, string username)
        {
            return (DAL.Convert.UserConvert.ConvertUsersListToEntity(GetAll<User>()).First(u => u.Username == username).UserPassword == password);
        }
        //reset the password
        public int ResetPassword(string username)
        {
            Entities.User user = DAL.Convert.UserConvert.ConvertUsersListToEntity(GetAll<User>()).First(u => u.Username == username.Trim());
            Random rand = new Random();
            int valcode = rand.Next(11111, 99999);
            user.UserPassword = valcode.ToString();
            UpdateSet<User>(DAL.Convert.UserConvert.ConvertUserToEF(user));
            string subject = string.Format(" החלפת סיסמא למשתמש {0}", username);
            string body = "";
            body += "\nהכנס את הקוד הבא לצורך החלפת סיסמא לחדשה: ";
            body += string.Format(" :הקוד לאימות: {0}", valcode);
            SendEmail(user.UserEmail, username, subject, body);
            return 1;
        }

    }
}
