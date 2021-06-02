using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BL;
using DAL;

namespace API.Controllers
{
    //post-כל מה שעושה שינויים בדטה בייס
    //get-מה שלא עושה שינויים בדטה בייס
    [RoutePrefix("api/paymentDetails")]
    public class PaymentDetailsController : ApiController
    {
        MainBL mbl = new MainBL();
        [AcceptVerbs("GET", "POST")]
        [Route("addPaymentAccount")]
        [HttpPost]
        //adding a payment details
        public int AddPaymentA(int usercode, Entities.PaymentDetail p)
        {
            return mbl.CheckAndAddPaymentA(usercode, p);
        }
        [AcceptVerbs("GET", "POST")]
        [Route("deletePaymentAccount")]
        [HttpPost]
        //deleting the payment details
        public int deletePaymentA(int usercode, DAL.PaymentDetail p)
        {
            return mbl.DeletePaymentDetails(usercode, p);
        }
    }
}
