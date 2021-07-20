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
        MainBL mbl;
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
        public int DeleteSearch(Entities.ParkingSpotSearch pss)
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
        [AcceptVerbs("GET", "POST")]
        [Route("confirmImidSearchResult/{pspotCode}/{psearchCode}")]
        [HttpGet]
        //confirming imidiate search result 
        public int ConfirmResult(int pspotCode, int psearchCode)
        {
            return sbl.ConfirmResult(pspotCode, psearchCode);
        }
        [AcceptVerbs("GET", "POST")]
        [Route("confirmRegSearchResult/{srCode}")]
        [HttpGet]
        //confirming reg search result 
        public int ConfirmRegSResult(int srCode)
        {
            mbl = new MainBL();
            return mbl.ConfirmRegSResult(srCode);
        }
        //public int AddWeekDays(Schedule_Week sw)
        //{
        //    return wdbl.AddWeekDays(sw);
        //}
        [AcceptVerbs("GET", "POST")]
        [Route("getSchedule/{wcode}")]
        [HttpGet]
        // getting schedule by weekday table code
        public Schedule_Week GetSchedule(int wcode)
        {
            return wdbl.GetSchedule(wcode);
        }
        [AcceptVerbs("GET", "POST")]
        [Route("getSearch/{scode}")]
        [HttpGet]
        // getting schedule by weekday table code
        public Entities.ParkingSpotSearch GetSearch(int scode)
        {
            return sbl.GetSearch(scode);
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
        [Route("getRegSearchesResults/{ucode}")]
        [HttpGet]
        // getting list of cities
        public List<SearchResult> GetUserSearchResults(int ucode)
        {
            mbl = new MainBL();
            return mbl.GetUserSearchResults(ucode);
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
