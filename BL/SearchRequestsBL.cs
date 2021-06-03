using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Entities;
using ParkingSpot = DAL.ParkingSpot;
using ParkingSpotSearch = DAL.ParkingSpotSearch;


namespace BL
{
    public class SearchRequestsBL
    {
        DBConnection DBCon;
        DistanceFunc df;
        ConvertFuncBL convertFunc;
        
        public SearchRequestsBL()
        {
            DBCon = new DBConnection();
            df = new DistanceFunc();
            convertFunc = new ConvertFuncBL();
        }
        // adding a new parking spot search. returns code if succeeds
        public int AddParkingSpotSearch(Entities.ParkingSpotSearch pss)

        {
            pss.Place_id = df.GetPlaceId(pss.MyLocationAddress);
            DbHandler.AddSet(pss);
            return DbHandler.GetAll<ParkingSpotSearch>().Any(s => s.Code == pss.Code) ? DbHandler.GetAll<ParkingSpotSearch>().First(s => s.Code == pss.Code).Code : 0;

        }
        public Dictionary<ParkingSpot, int> AddImmidiateParkingSpotSearch(Entities.ParkingSpotSearch pss)
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

            DBCon.Execute(pss, DBConnection.ExecuteActions.Update);
            return DbHandler.GetAll<ParkingSpotSearch>().First(s => s.Code == pss.Code).Code;
        }
        // deletes search by user
        public int DeletePSSearchesByUser(Entities.User u)
        {
            var psslist = DbHandler.GetAll<ParkingSpotSearch>();
            if (psslist != null)
            {
                foreach (var item in psslist)
                {
                    if (item.UserId == u.Code)
                    {
                        DbHandler.DeleteSet(item);
                    }
                }
            }
            return 1;
        }
        
        //deletes search
        public int DeleteParkingSpotSearch(ParkingSpotSearch pss)
        {
            var l = DbHandler.GetAll<ParkingSpotSearch>().First(i => i.Code == pss.Code);
            DbHandler.DeleteSet(l);

            return 1;
        }
        public Dictionary<ParkingSpot, int> GetFiveClosestParkSpots(Entities.ParkingSpotSearch pss)
        {
            //gets all parking spots in this city
            var listOfSpots = DbHandler.GetAll<ParkingSpot>().Where(y => y.CityCode == pss.CityCode).ToList();
            //filtering by hours
            var shl = convertFunc.GetHoursForPSImidiateSearch(pss.WeekDay.Code);
            foreach (var spot in listOfSpots)
            {
                var hl = convertFunc.GetHoursListFromWeekDay(spot.WeekDay.Code);
                //checks if there is any hours in listOfSpots that matches hours in that search at that day
                if (!(hl[shl.First().Key].Any(h => (h.StartHour <= shl.First().Value.StartHour) && (h.EndHour >= shl.First().Value.EndHour))))
                    listOfSpots.Remove(spot);
            }

            //caclulating distances from the place_id to listOfSpotsInCity
            Dictionary<int, int> results_dic = df.GetDistanceToManyPoints(pss.Place_id, listOfSpots);

            //result Dict
            Dictionary<ParkingSpot, int> distdic = new Dictionary<ParkingSpot, int>();
            //  select the 5 closest
            foreach (var item in results_dic)
            {
                distdic.Add(key: DbHandler.GetAll<ParkingSpot>().First(y => y.Code == item.Key), value: item.Value);
            }
            return distdic;
        }


    }
}
