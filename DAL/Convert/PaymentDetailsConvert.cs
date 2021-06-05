using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Convert
{
    public static class PaymentDetailsConvert
    {
        public static DAL.PaymentDetail ConvertPaymentDetailToEF(Entities.PaymentDetail pd)
        {
            return new DAL.PaymentDetail
            {
                Code = pd.Code,
                CreditCardNumber = pd.CreditCardNumber,
                PaymentAmount = pd.PaymentAmount,
                ExpiryDateMonth = pd.ExpiryDateMonth,
                ExpiryDateYear = pd.ExpiryDateYear,
                SecurityCode = pd.SecurityCode,
                PostalCode = pd.PostalCode
            };
        }
        public static Entities.PaymentDetail ConvertPaymentDetailToEntity(DAL.PaymentDetail pd)
        {
            return new Entities.PaymentDetail
            {
                Code = pd.Code,
                CreditCardNumber = pd.CreditCardNumber,
                PaymentAmount = pd.PaymentAmount,
                ExpiryDateMonth = pd.ExpiryDateMonth,
                ExpiryDateYear = pd.ExpiryDateYear,
                SecurityCode = pd.SecurityCode,
                PostalCode = pd.PostalCode
            };
        }
        public static List<Entities.PaymentDetail> ConvertPaymentDetailListToEntity(IEnumerable<DAL.PaymentDetail> pdetails)
        {
            return pdetails.Select(p => ConvertPaymentDetailToEntity(p)).OrderBy(n => n.Code).ToList();
        }


    }
}
