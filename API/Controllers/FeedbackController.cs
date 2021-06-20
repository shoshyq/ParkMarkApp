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
    [RoutePrefix("api/feedbacks")]
    public class FeedbackController : ApiController
    {
        FeedbackBL fbl = new FeedbackBL();
        [AcceptVerbs("GET", "POST")]
        [Route("addFeedback")]
        [HttpGet]
        //adding a feedback 
        public int AddFeedback(Entities.Feedback f)
        {
            return fbl.AddFeedback(f);
        }
        [AcceptVerbs("GET", "POST")]
        [Route("updateFeedback")]
        [HttpPost]
        public int UpdateFeedback(int usercode,Entities.Feedback f)
        {
            return (fbl.UpdateFeedback(usercode,f));
        }
        [AcceptVerbs("GET", "POST")]
        [Route("deleteFeedback")]
        [HttpPost]
        public int DeleteFeedback(Entities.Feedback f)
        {
            return (fbl.DeleteFeedback(f));
        }

    }
}
