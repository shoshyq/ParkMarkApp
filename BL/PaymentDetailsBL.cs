using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Entities;
using PaymentDetail = DAL.PaymentDetail;
using User = DAL.User;


namespace BL
{
    public class PaymentDetailsBL : DbHandler
    {
        public PaymentDetailsBL()
        {
        }
        // adding a new payment details table. returns code if succeeds
        public int AddPaymentDetails(Entities.PaymentDetail pd)
        {

            AddSet<PaymentDetail>(DAL.Converts.PaymentDetailsConvert.ConvertPaymentDetailToEF(pd));
            var list = DAL.Converts.PaymentDetailsConvert.ConvertPaymentDetailListToEntity(GetAll<PaymentDetail>());
            if (list.Any(p => p.Code == pd.Code))
                return list.First(p => p.Code == pd.Code).Code;
            return 0;
        }


        //returns 2 if can't add more payment accounts, returns the new payment code if succeeds
        public int CheckAndAddPaymentA(int usercode, Entities.PaymentDetail p)
        {
            if ((DAL.Converts.UserConvert.ConvertUsersListToEntity(GetAll<User>()).First(u => u.Code == usercode).PaymentDetails1 != null) &&(DAL.Converts.UserConvert.ConvertUsersListToEntity(GetAll<User>()).First(u => u.Code == usercode).PaymentDetails2 != null))
                return 2;
            else
                return AddPaymentDetails(p);
        }
        public int DeletePaymentDetailsByUser(Entities.User user)
        {
            var pdlist = DAL.Converts.PaymentDetailsConvert.ConvertPaymentDetailListToEntity(GetAll<PaymentDetail>());
            if (pdlist != null)
            {
                foreach (var item in pdlist)
                {
                    if (item.Code == user.PaymentDetails1 || item.Code == user.PaymentDetails2)
                    {
                        DeleteSet(DAL.Converts.PaymentDetailsConvert.ConvertPaymentDetailToEF(item));
                    }
                }
            }
            return 1;
        }
        public int DeletePaymentDetails(int usercode, Entities.PaymentDetail p)
        {
            if ((DAL.Converts.UserConvert.ConvertUsersListToEntity(GetAll<User>()).First(u => u.Code == usercode).PaymentDetails1 != null) && (DAL.Converts.UserConvert.ConvertUsersListToEntity(GetAll<User>()).First(u => u.Code == usercode).PaymentDetails2 != null))
            {
                DeleteSet(DAL.Converts.PaymentDetailsConvert.ConvertPaymentDetailToEF(p));
                return 1;
            }
            else
                return 0;
        }
    }
}
