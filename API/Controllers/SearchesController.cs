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
    [RoutePrefix("api/searches")]
    public class SearchesController : ApiController
    {

        SearchRequestsBL sbl = new SearchRequestsBL();

        [AcceptVerbs("GET", "POST")]
        [Route("addRegSearch")]
        [HttpPost]
        //adding a reg search request 
        public int AddParkingSpotSearch(Entities.ParkingSpotSearch pss)
        {
            return sbl.AddParkingSpotSearch(pss);
        }
        [Route("addImmidSearch")]
        [HttpPost]
        //adding an immidiate search request 
        public Dictionary<DAL.ParkingSpot, int> AddImmidiateSearch(Entities.ParkingSpotSearch pss)
        {
            return sbl.AddImmidiateParkingSpotSearch(pss);
        }
        [AcceptVerbs("GET", "POST")]
        [Route("updateSearch")]
        [HttpPost]
        public int UpdateSearch(Entities.ParkingSpotSearch pss)
        {
            return (sbl.UpdateParkingSpotSearch(pss));
        }
        [AcceptVerbs("GET", "POST")]
        [Route("deleteSearch")]
        [HttpPost]
        public int DeleteSearch(DAL.ParkingSpotSearch pss)
        {
            return (sbl.DeleteParkingSpotSearch(pss));
        }
    }
}
