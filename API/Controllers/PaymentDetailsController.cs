using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BL;

namespace API.Controllers
{
    //post-כל מה שעושה שינויים בדטה בייס
    //get-מה שלא עושה שינויים בדטה בייס
    [RoutePrefix("api/paymentDetails")]
    public class PaymentDetailsController : ApiController
    {
        PaymentDetailsBL pbl = new PaymentDetailsBL();

        [AcceptVerbs("GET", "POST")]
        [Route("addPaymentAccount/{usercode}")]
        
        [HttpPost]
        //adding a payment details
        public int AddPaymentA(int usercode, Entities.PaymentDetail p)
        {
            return pbl.CheckAndAddPaymentA(usercode, p);
        }
        [AcceptVerbs("GET", "POST")]
        [Route("deletePaymentAccount")]
        [HttpPost]
        //deleting the payment details
        public int deletePaymentA(int usercode, Entities.PaymentDetail p)
        {
            return pbl.DeletePaymentDetails(usercode, p);
        }
    }
}
