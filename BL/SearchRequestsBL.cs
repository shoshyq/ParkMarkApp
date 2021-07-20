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
        ParkingSpotsBL pspbl;
        MainBL mainBL;

        List<Entities.ParkingSpotSearch> psslist = DAL.Convert.SearchConvert.ConvertSearchesListToEntity(GetAllPSS());
        WeekDayBL sw;

        public SearchRequestsBL()
        {
            df = new DistanceFunc();
            dbConnection = new DBConnection();
            convertFunc = new ConvertFuncBL();
            sw = new WeekDayBL();
            pspbl = new ParkingSpotsBL();
            mainBL = new MainBL();
        }

        // adding a new parking spot search. returns code if succeeds
        public int AddParkingSpotSearch(Entities.ParkingSpotSearch pss)

        {
            if ((pss.Place_id == null) || (pss.Place_id == ""))
            {
                pss.Place_id = df.GetPlaceId(pss.MyLocationAddress);

            }
            AddSet(DAL.Convert.SearchConvert.ConvertSearchToEF(pss));
            mainBL.RegFunc();
           // mainBL.ParkingSpotPerUser();
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
                var rdic = GetFiveClosestParkSpots(pss);
                List<ResDict> rdlist = new List<ResDict>();
                if (rdic != null)
                {
                    foreach (var item in rdic)
                        rdlist.Add(new ResDict { PSpot = item.Key, Distance = item.Value });
                    rdlist.Add(new ResDict { PSpot = new Entities.ParkingSpot { Code = result }, Distance = "" });

                }
                return rdlist;
            }
            //gets dic = key: pspot , value: distance (in km.000 but without a dot)

            else
                return null;
        }


        public int ConfirmResult(int pspCode, int psCode)
        {
            //sending notifying email to pspot user 

            Entities.ParkingSpot pspot = DAL.Convert.ParkSpotConvert.ConvertParkingSpotsListToEntity(GetAll<DAL.ParkingSpot>()).First(y => y.Code == pspCode);
            Entities.ParkingSpotSearch psearch = DAL.Convert.SearchConvert.ConvertSearchToEntity((ParkingSpotSearch)GetAll<DAL.ParkingSpotSearch>().First(y => y.Code == psCode));
            Entities.User spOwner = DAL.Convert.UserConvert.ConvertUsersListToEntity((IEnumerable<DAL.User>)GetAll<DAL.User>()).First(u => u.Code == pspot.UserCode);
            Entities.User spRenter = DAL.Convert.UserConvert.ConvertUsersListToEntity((IEnumerable<DAL.User>)GetAll<DAL.User>()).First(u => u.Code == psearch.UserId);
            Schedule_Week searchHours = sw.GetSchedule((int)psearch.DaysSchedule);
            string today = DateTime.Today.DayOfWeek.ToString();
            string sh = "";
            string eh = "";
            int dayi = 6;
            switch (today)
            {
                case ("Sunday"):
                    sh = searchHours.SundayHours[0].StartHour;
                    eh = searchHours.SundayHours[0].EndHour;
                    dayi = (int)DayOfWeek.Sunday;
                    break;
                case ("Monday"):
                    sh = searchHours.MondayHours[0].StartHour;
                    eh = searchHours.MondayHours[0].EndHour;
                    dayi = (int)DayOfWeek.Monday;

                    break;
                case ("Tuesday"):
                    sh = searchHours.TuesdayHours[0].StartHour;
                    eh = searchHours.TuesdayHours[0].EndHour;
                    dayi = (int)DayOfWeek.Tuesday;

                    break;
                case ("Wednesday"):
                    sh = searchHours.WednesdayHours[0].StartHour;
                    eh = searchHours.WednesdayHours[0].EndHour;
                    dayi = (int)DayOfWeek.Wednesday;

                    break;
                case ("Thursday"):
                    sh = searchHours.ThursdayHours[0].StartHour;
                    eh = searchHours.ThursdayHours[0].EndHour;
                    dayi = (int)DayOfWeek.Thursday;

                    break;
                case ("Friday"):
                    sh = searchHours.FridayHours[0].StartHour;
                    eh = searchHours.FridayHours[0].EndHour;
                    dayi = (int)DayOfWeek.Friday;

                    break;
                default:
                    break;
            }

            string subject = "You have a new parking renter!";
            string body = "";
            body += string.Format("User {0} is renting Your parking spot on {1} <br />", spRenter.Username, pspot.FullAddress);
            body += string.Format("from {0} to {1} <br />", sh, eh);
            body += string.Format(" You can contact {0}:<br />", spRenter.Username);
            body += string.Format("Email: {0}<br />", spRenter.UserEmail);
            if ((spOwner.UserPhoneNumber != null) || (spRenter.UserPhoneNumber != ""))
                body += string.Format("Phone: {0}<br />", spRenter.UserPhoneNumber);
            body += " Don't forget to leave a review! =)<br />";
            //if sending mail succeeds, update schedules and return 1
            int sendmail = UserBL.SendEmail(spOwner.UserEmail, spOwner.Username, subject, body);
            if (sendmail == 1)
            {
                //updating pspot schedule
                Entities.ParkingSpot updates_pspot = sw.UpdatePSpotScheduleBySearch(dayi, sh, eh, pspot);
                pspbl.UpdateUsersParkSpot(updates_pspot);
                return 1;
            }
            else
            {
                //error while sending email
                return 2;
            }


        }

        public Entities.ParkingSpotSearch GetSearch(int scode)
        {
            return psslist.First(s => s.Code == scode);
        }

        // updating a parking spot search. returns code if succeeds
        public int UpdateParkingSpotSearch(Entities.ParkingSpotSearch pss)
        {

            if ((psslist.First(s => s.Code == pss.Code).MyLocationAddress != pss.MyLocationAddress))
            {

                if ((pss.Place_id == null) || (pss.Place_id == ""))
                {
                    pss.Place_id = df.GetPlaceId(pss.MyLocationAddress);

                }
            }
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
        public int DeleteParkingSpotSearch(Entities.ParkingSpotSearch pss)
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
                        if ((pss.MinPrice <= spot.PricePerHour) && (pss.MaxPrice >= spot.PricePerHour))
                            pslist.Add(spot);
            }
            if (pslist.Count == 0)
                return null;
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
