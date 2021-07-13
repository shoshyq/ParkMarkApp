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
            if ((pss.Place_id == null) || (pss.Place_id == ""))
            {
                pss.Place_id = df.GetPlaceId(pss.MyLocationAddress);

            }
            AddSet(DAL.Convert.SearchConvert.ConvertSearchToEF(pss));
            return psslist.LastOrDefault().Code;

        }
        public List<ResDict> AddImmidiateParkingSpotSearch(Entities.ParkingSpotSearch pss)
        {

            if ((pss.Place_id == null) || (pss.Place_id == ""))
            {
                pss.Place_id = df.GetPlaceId(pss.MyLocationAddress);

            }
            int result = AddParkingSpotSearch(pss);
            if (result != 0)
            { 
                var rdic =  GetFiveClosestParkSpots(pss);
                List<ResDict> rdlist = new List<ResDict>();
                foreach (var item in rdic)
                    rdlist.Add(new ResDict { PSpot = item.Key, Distance = item.Value });
                return rdlist;
            }
                //gets dic = key: pspot , value: distance (in km.000 but without a dot)
               
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
            var shl = convertFunc.GetHoursListFromWeekDay((int)pss.DaysSchedule);
            List<Entities.ParkingSpot> pslist = new List<Entities.ParkingSpot>();
            foreach (var spot in listOfSpots)
            {
                var hl = convertFunc.GetHoursListFromWeekDay((int)spot.DaysSchedule);
                bool keyExists = hl.ContainsKey(shl.First().Key);
                if (keyExists)
                    //checks if there is any hours in listOfSpots that matches hours in that search at that day
                    if ((hl[shl.First().Key].Any(h => (Double.Parse(h.StartHour) <= Double.Parse(shl.First().Value[0].StartHour)) &&
                (Double.Parse(h.EndHour) >= Double.Parse(shl.First().Value[0].EndHour)))))
                    pslist.Add(spot);
            }

            //caclulating distances from the place_id to listOfSpotsInCity
            Dictionary<int, string> results_dic = df.GetDistanceToManyPoints(pss.Place_id, pslist);

            //result Dict
            Dictionary<Entities.ParkingSpot, string> distdic = new Dictionary<Entities.ParkingSpot, string>();
            //  select the 5 closest
            foreach (var item in results_dic)
            {
                distdic.Add(key: DAL.Convert.ParkSpotConvert.ConvertParkingSpotsListToEntity(GetAll<DAL.ParkingSpot>())
                    .First(y => y.Code == item.Key), value: item.Value.ToString());
            }
            return distdic;
        }


    }
}
