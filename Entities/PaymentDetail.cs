using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
     public class PaymentDetail
    {
        public int Code { get; set; }
        public string CreditCardNumber { get; set; }
        public Nullable<double> PaymentAmount { get; set; }
        public string ExpiryDateMonth { get; set; }
        public string ExpiryDateYear { get; set; }
        public string SecurityCode { get; set; }
        public string PostalCode { get; set; }
    }
}
