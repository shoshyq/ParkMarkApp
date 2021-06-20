using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using DAL;
using ParkingSpotSearch = DAL.ParkingSpotSearch;

namespace BL
{
    public class SearchRequestsBL : DbHandler
    {
        DistanceFunc df;
        DBConnection dbConnection;
        ConvertFuncBL convertFunc;
        List<Entities.ParkingSpotSearch> psslist = DAL.Convert.SearchConvert.ConvertSearchesListToEntity(GetAllPSS());


        public SearchRequestsBL()
        {
            df = new DistanceFunc();
            dbConnection = new DBConnection();
            convertFunc = new ConvertFuncBL();
        }
     
        // adding a new parking spot search. returns code if succeeds
        public int AddParkingSpotSearch(Entities.ParkingSpotSearch pss)

        {
            pss.Place_id = df.GetPlaceId(pss.MyLocationAddress);
            AddSet(DAL.Convert.SearchConvert.ConvertSearchToEF(pss));
            return psslist.Any(s => s.Code == pss.Code) ? psslist.First(s => s.Code == pss.Code).Code : 0;

        }
        public Dictionary<Entities.ParkingSpot, string> AddImmidiateParkingSpotSearch(Entities.ParkingSpotSearch pss)
        {
            int result = AddParkingSpotSearch(pss);
            if (result != 0)
                //gets dic = key: pspot , value: distance (in km.000 but without a dot)
                return GetFiveClosestParkSpots(pss);
            else
                return null;
        }
        // updating a parking spot search. returns code if succeeds
        public int UpdateParkingSpotSearch(Entities.ParkingSpotSearch pss)
        {
            pss.Place_id = df.GetPlaceId(pss.MyLocationAddress);

            UpdateSet(DAL.Convert.SearchConvert.ConvertSearchToEF(pss));
            return psslist.First(s => s.Code == pss.Code).Code;
        }
        // deletes search by user
        public int DeletePSSearchesByUser(Entities.User u)
        {   
            if (psslist != null)
            {
                foreach (var item in psslist)
                {
                    if (item.UserId == u.Code)
                    {
                       DeleteSet(DAL.Convert.SearchConvert.ConvertSearchToEF(item));
                    }
                }
            }
            return 1;
        }
        
        //deletes search
        public int DeleteParkingSpotSearch(ParkingSpotSearch pss)
        {
            var l = psslist.First(i => i.Code == pss.Code);
            DeleteSet(DAL.Convert.SearchConvert.ConvertSearchToEF(l));

            return 1;
        }
        public Dictionary<Entities.ParkingSpot, string> GetFiveClosestParkSpots(Entities.ParkingSpotSearch pss)
        {
            //gets all parking spots in this city
            var listOfSpots = DAL.Convert.ParkSpotConvert.
                ConvertParkingSpotsListToEntity(GetAll<DAL.ParkingSpot>()).Where(y => y.CityCode == pss.CityCode)
                .ToList();
            //filtering by hours
            var shl = convertFunc.GetHoursForPSImidiateSearch((int)pss.DaysSchedule);
            foreach (var spot in listOfSpots)
            {
                var hl = convertFunc.GetHoursListFromWeekDay((int)spot.DaysSchedule);
                //checks if there is any hours in listOfSpots that matches hours in that search at that day
                if (!(hl[shl.First().Key].Any(h => (h.StartHour <= shl.First().Value.StartHour) && 
                (h.EndHour >= shl.First().Value.EndHour))))
                    listOfSpots.Remove(spot);
            }

            //caclulating distances from the place_id to listOfSpotsInCity
            Dictionary<int, int> results_dic = df.GetDistanceToManyPoints(pss.Place_id, listOfSpots);

            //result Dict
            Dictionary<Entities.ParkingSpot, string> distdic = new Dictionary<Entities.ParkingSpot, string>();
            //  select the 5 closest
            foreach (var item in results_dic)
            {
                distdic.Add(key: DAL.Convert.ParkSpotConvert.ConvertParkingSpotsListToEntity(GetAll<DAL.ParkingSpot>())
                    .First(y => y.Code == item.Value), value: item.Key.ToString().Insert(item.Key.ToString().Length - 3,
                    "."));
            }
            return distdic;
        }


    }
}
