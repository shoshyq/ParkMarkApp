using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BL;
using Entities;

namespace API.Controllers
{
    //post-כל מה שעושה שינויים בדטה בייס
    //get-מה שלא עושה שינויים בדטה בייס
    [RoutePrefix("api/searches")]
    public class SearchesController : ApiController
    {

        SearchRequestsBL sbl = new SearchRequestsBL();
        CityBL ctbl = new CityBL();
        WeekDayBL wdbl = new WeekDayBL();
        [AcceptVerbs("GET", "POST")]
        [Route("addRegSearch")]
        [HttpPost]
        //adding a reg search request 
        public int AddParkingSpotSearch(ParkingSpotSearch pss)
        {
            return sbl.AddParkingSpotSearch(pss);
        }
        [Route("addImmidSearch")]
        [HttpPost]
        //adding an immidiate search request - return results
        public List<ResDict> AddImmidiateSearch(ParkingSpotSearch pss)
        {
            return sbl.AddImmidiateParkingSpotSearch(pss);
        }
        [AcceptVerbs("GET", "POST")]
        [Route("updateSearch")]
        [HttpPost]
        public int UpdateSearch(ParkingSpotSearch pss)
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
        [AcceptVerbs("GET", "POST")]
        [Route("addSchedule")]
        [HttpPost]
        //adding a schedule table 
        public int AddWeekDays(Schedule_Week sw)
        {
            return wdbl.AddWeekDays(sw);
        }
        //public int AddWeekDays(Schedule_Week sw)
        //{
        //    return wdbl.AddWeekDays(sw);
        //}
        [AcceptVerbs("GET", "POST")]
        [Route("getSchedule")]
        [HttpGet]
        // getting schedule by weekday table code
        public Schedule_Week GetSchedule(int wcode)
        {
            return wdbl.GetSchedule(wcode);
        }
        [AcceptVerbs("GET", "POST")]
        [Route("getCities")]
        [HttpGet]
        // getting list of cities
        public List<CityDTO> GetCities()
        {
            return ctbl.GetAllCities();
        }
        [AcceptVerbs("GET", "POST")]
        [Route("addCity")]
        [HttpPost]
        // adding a city
        public int AddCity(CityDTO city)
        {
            return ctbl.AddCity(city);
        }
        [AcceptVerbs("GET", "POST")]
        [Route("updateWeekDays")]
        [HttpPost]
        public Schedule_Week UpdateWeekDays(Schedule_Week sw)
        {
            return (wdbl.UpdateWeekDay(sw));
        }
        //[AcceptVerbs("GET", "POST")]
        //[Route("getQuickSearchResults")]
        //[HttpPost]
        ////adding a reg search request 
        //public List< GetQuickSearchResults(ParkingSpotSearch pss)
        //{
        //    return sbl.AddParkingSpotSearch(pss);
        //}
        //[Route("api/electionResult/getResult/{electionId}")]
        //public List<ResultOfOption> GetResult(long electionId)
        //{
        //    return GeneralBL.GetResult(electionId);
        //}
    }

}
