using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Entities;

namespace DAL.Converts
{
    public static class UserConvert
    {
            public static DAL.User ConvertUserToEF(Entities.User u)
            {
                return new DAL.User
                {
                    Code = u.Code,
                    Username = u.Username,
                    UserPassword = u.UserPassword,
                    UserEmail = u.UserEmail,
                    UserPhoneNumber = u.UserPhoneNumber,
                    PaymentDetails1 = u.PaymentDetails1,
                    PaymentDetails2 = u.PaymentDetails2
                };
            }
            public static Entities.User ConvertUserToEntity(DAL.User u)
            {
                return new Entities.User
                {
                    Code = u.Code,
                    Username = u.Username,
                    UserPassword = u.UserPassword,
                    UserEmail = u.UserEmail,
                    UserPhoneNumber = u.UserPhoneNumber,
                    PaymentDetails1 = u.PaymentDetails1,
                    PaymentDetails2 = u.PaymentDetails2
                };
            }



            public static List<Entities.User> ConvertUsersListToEntity(IEnumerable<DAL.User> users)
            {
                return users.Select(u => ConvertUserToEntity(u)).OrderBy(n => n.Code).ToList();
            }
    }
}
