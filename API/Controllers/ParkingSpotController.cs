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
        WeekDayBL wdbl = new WeekDayBL();
        ParkingSpotsBL psbl = new ParkingSpotsBL();
        CityBL cbl = new CityBL();

        [AcceptVerbs("GET", "POST")]
        [Route("addSchedule")]
        [HttpPost]
        //adding a schedule table 
        public int AddWeekDays(Schedule_Week sw)
        {
            return wdbl.AddWeekDays(sw);
        }

        [AcceptVerbs("GET", "POST")]
        [Route("addParkSpot")]
        [HttpPost]
        //adding a parking spot
        public int AddParkSpot(Entities.ParkingSpot ps)
        {
            return (psbl.RegisterUsersParkSpot(ps));
        }
        [AcceptVerbs("GET", "POST")]
        [Route("addCity")]
        [HttpPost]
        //adding a new city
        public int AddCity(string cityname)
        {
            return cbl.AddCity(cityname);
        }
        [AcceptVerbs("GET", "POST")]
        [Route("updateParkSpot")]
        [HttpPost]
        public int UpdateParkSpot(Entities.ParkingSpot ps)
        {
            return (psbl.UpdateUsersParkSpot(ps));
        }
        [AcceptVerbs("GET", "POST")]
        [Route("deleteParkSpot")]
        [HttpPost]
        //returns 0 if succeeds
        public int DeleteParkSpot(Entities.ParkingSpot ps)
        {
            return (psbl.DeleteParkingSpot(ps));
        }
        [AcceptVerbs("GET", "POST")]
        [Route("updateWeekDays")]
        [HttpPost]
        public Schedule_Week UpdateWeekDays(Schedule_Week sw)
        {
            return (wdbl.UpdateWeekDay(sw));
        }

    }
}
