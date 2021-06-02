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
    [RoutePrefix("api/parkingSpots")]
    public class ParkingSpotController : ApiController
    {
        MainBL mbl = new MainBL();
        [AcceptVerbs("GET", "POST")]
        [Route("addSchedule")]
        [HttpPost]
        //adding a schedule table 
        public int AddWeekDays(Schedule_Week sw)
        {
            return mbl.AddWeekDays(sw);
        }

        [AcceptVerbs("GET", "POST")]
        [Route("addParkSpot")]
        [HttpPost]
        //adding a parking spot
        public int AddParkSpot(Entities.ParkingSpot ps)
        {
            return (mbl.RegisterUsersParkSpot(ps));
        }
        [AcceptVerbs("GET", "POST")]
        [Route("addCity")]
        [HttpPost]
        //adding a new city
        public int AddCity(string cityname)
        {
            return mbl.AddCity(cityname);
        }
        [AcceptVerbs("GET", "POST")]
        [Route("updateParkSpot")]
        [HttpPost]
        public int UpdateParkSpot(Entities.ParkingSpot ps)
        {
            return (mbl.UpdateUsersParkSpot(ps));
        }
        [AcceptVerbs("GET", "POST")]
        [Route("deleteParkSpot")]
        [HttpPost]
        public int DeleteParkSpot(DAL.ParkingSpot ps)
        {
            return (mbl.DeleteParkingSpot(ps));
        }
        [AcceptVerbs("GET", "POST")]
        [Route("updateWeekDays")]
        [HttpPost]
        public int UpdateWeekDays(Schedule_Week sw)
        {
            return (mbl.UpdateWeekDay(sw));
        }

    }
}
