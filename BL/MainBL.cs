using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;


namespace BL
{
    public class MainBL
    {
        HungarianFunctions hf;
        DistanceFunc df;
        ConvertFuncBL cvrt;
        WeekDayBL wbl;
        ParkingSpotsBL pspbl;
        SearchRequestsBL srbl;

        public MainBL()
        {
            df = new DistanceFunc();
            hf = new HungarianFunctions();
            cvrt = new ConvertFuncBL();
        }

        //once a day main-hungarian function
        public void RegFunc()
        {

            //Time when method needs to be called
            var DailyTime = "11:10:00";
            var timeParts = DailyTime.Split(new char[1] { ':' });

            var dateNow = DateTime.Now;
            var date = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day,
                       int.Parse(timeParts[0]), int.Parse(timeParts[1]), int.Parse(timeParts[2]));
            TimeSpan ts;
            if (date > dateNow)
                ts = date - dateNow;
            else
            {
                date = date.AddDays(1);
                ts = date - dateNow;
            }

            //waits certan time and run the code
            Task.Delay(ts).ContinueWith((x) => ParkingSpotPerUser());
        }
        //Main algorithm function  - returns dictionary key:city, value: dictionary - schedule of pspots and searches
        public void ParkingSpotPerUser()
        {
            var yesterday = DateTime.Today.AddDays(-1);
            Dictionary<int, Dictionary<int?, int>> rdic = hf.PSpotsAllSearchesByCities(yesterday);
            //updating parking spot schedule
            foreach (var item in rdic)
            {
                //  dictionary < spot_code, search_code >
                foreach (var dic in item.Value)
                {
                    Entities.ParkingSpot pspot = DAL.Convert.ParkSpotConvert.ConvertParkingSpotsListToEntity(DbHandler.GetAll<DAL.ParkingSpot>()).First(y => y.Code == dic.Key);
                    Entities.ParkingSpotSearch psearch = DAL.Convert.SearchConvert.ConvertSearchesListToEntity(DbHandler.GetAll<DAL.ParkingSpotSearch>()).First(y => y.Code == dic.Value);
                    Entities.User spOwner = DAL.Convert.UserConvert.ConvertUsersListToEntity((IEnumerable<DAL.User>)DbHandler.GetAll<DAL.User>()).First(u => u.Code == pspot.UserCode);
                    Entities.User spRenter = DAL.Convert.UserConvert.ConvertUsersListToEntity((IEnumerable<DAL.User>)DbHandler.GetAll<DAL.User>()).First(u => u.Code == psearch.UserId);

                    //adding search result table
                    Entities.SearchResult searchResult = new Entities.SearchResult()
                    {
                        ResultPSCode = pspot.Code,
                        SearchCode = psearch.Code,
                        Usercode = psearch.UserId
                    };
                    DbHandler.AddSet(DAL.Convert.SearchResultConvert.ConvertSResultToEF(searchResult));

                    //  WeekDay pSweekdaytbl = (DAL.Convert.WeekDayConvert.ConvertWeekDaysListToEntity(DbHandler.GetAll<DAL.WeekDay>()).First(w => w.Code == pspot.DaysSchedule));
                    //Schedule_Week spotSch = WeekDayTblToSchedWeek(weekdaytbl);
                }
            }
        }
        public List<SearchResult> GetUserSearchResults(int usercode)
        {
            return DAL.Convert.SearchResultConvert.ConvertSearchResultListToEntity(DbHandler.GetAll<DAL.SearchResult>()).Where(r => r.Usercode == usercode).ToList();
        }

        // confirmation of the result from user about the parking spot. updating the schedule-times table 
        public int ConfirmResult(Entities.ParkingSpotSearch psr, int usercode, Entities.ParkingSpot ps)
        {
            return 1;
        }
//        string subject = "You have a new  regularly parking spot renting";
//        string body = "";
//        body += string.Format("User {0} wants to rent Your parking spot on {1} <br />", spRenter.Username, pspot.FullAddress);
//                    foreach (var day in searchHours)
//                    {
//                        bool keyExists = spotHours.ContainsKey(day.Key);
//                        if (keyExists)
//                        {
//                            string dayName = "";
//                            switch (day.Key)
//                            {
//                                case 0:
//                                    dayName = "Sunday";
//                                    break;
//                                case 1:
//                                    dayName = "Monday";
//                                    break;
//                                case 2:
//                                    dayName = "Tuesday";
//                                    break;
//                                case 3:
//                                    dayName = "Wednesday";
//                                    break;
//                                case 4:
//                                    dayName = "Thursday";
//                                    break;
//                                case 5:
//                                    dayName = "Friday";
//                                    break;
//                                default:
//                                    break;
//                            }
//    body += string.Format("On {0} :  <br />", dayName);
//                            foreach (var hours in day.Value)
//                            {
//                                body += string.Format("from {0} to {1} <br />", hours.StartHour, hours.EndHour);
//}
//                        }
//                    }
//                    body += string.Format(" You can contact {0}:<br />", spRenter.Username);
//body += string.Format("Email: {0}<br />", spRenter.UserEmail);
//if ((spOwner.UserPhoneNumber != null) || (spRenter.UserPhoneNumber != ""))
//    body += string.Format("Phone: {0}<br />", spRenter.UserPhoneNumber);
//body += " To submit  =)<br />";
////if sending mail succeeds, update schedules and return 1
//int sendmail = UserBL.SendEmail(spOwner.UserEmail, spOwner.Username, subject, body);
   //updating parking spot schedule
//            foreach (var item in rdic)
//            {
//                //  dictionary < spot_code, search_code >
//                foreach (var dic in item.Value)
//                {
//                    Entities.ParkingSpot pspot = DAL.Convert.ParkSpotConvert.ConvertParkingSpotsListToEntity(DbHandler.GetAll<DAL.ParkingSpot>()).First(y => y.Code == dic.Key);
//        Entities.ParkingSpotSearch psearch = DAL.Convert.SearchConvert.ConvertSearchesListToEntity(DbHandler.GetAll<DAL.ParkingSpotSearch>()).First(y => y.Code == dic.Value);
//        Entities.User spOwner = DAL.Convert.UserConvert.ConvertUsersListToEntity((IEnumerable<DAL.User>)DbHandler.GetAll<DAL.User>()).First(u => u.Code == pspot.UserCode);
//        Entities.User spRenter = DAL.Convert.UserConvert.ConvertUsersListToEntity((IEnumerable<DAL.User>)DbHandler.GetAll<DAL.User>()).First(u => u.Code == psearch.UserId);

//        var spotHours = cvrt.GetHoursListFromWeekDay((int)pspot.DaysSchedule);
//        var searchHours = cvrt.GetHoursListFromWeekDay((int)psearch.DaysSchedule);

//        WeekDay pspotWeekDay = (DAL.Convert.WeekDayConvert.ConvertWeekDaysListToEntity(DbHandler.GetAll<DAL.WeekDay>()).First(w => w.Code == pspot.DaysSchedule));
//        Schedule_Week pspotSW = cvrt.WeekDayTblToSchedWeek(pspotWeekDay);
//        WeekDay psearchWeekDay = (DAL.Convert.WeekDayConvert.ConvertWeekDaysListToEntity(DbHandler.GetAll<DAL.WeekDay>()).First(w => w.Code == psearch.DaysSchedule));
//        Schedule_Week psearchSW = cvrt.WeekDayTblToSchedWeek(psearchWeekDay);
//                    foreach (var dayi in searchHours)
//                    {

//                        bool keyExists = spotHours.ContainsKey(dayi.Key);
//                        if (keyExists)
//                        {
//                            foreach (var hours in dayi.Value)
//                            {
//                                if (spotHours[dayi.Key].Any(h => (Double.Parse(h.StartHour) <= Double.Parse(hours.StartHour)) && (Double.Parse(h.EndHour) >= Double.Parse(hours.EndHour))))
//                                {
//                                    Hours hrs = spotHours[dayi.Key].First(h => (Double.Parse(h.StartHour) <= Double.Parse(hours.StartHour)) && (Double.Parse(h.EndHour) >= Double.Parse(hours.EndHour)));
//        Hours newh1 = new Hours();
//        Hours newh2 = new Hours();
//                                    if ((Double.Parse(hrs.StartHour) < Double.Parse(hours.StartHour)) && (Double.Parse(hrs.EndHour) > Double.Parse(hours.EndHour)))
//                                    {
//                                        newh1.StartHour = hrs.StartHour;
//                                        newh1.EndHour = hours.StartHour;
//                                        newh2.StartHour = hours.EndHour;
//                                        newh2.EndHour = hrs.EndHour;
//                                    }
//                                    else
//                                    {
//                                        if ((Double.Parse(hrs.StartHour) == Double.Parse(hours.StartHour)) && (Double.Parse(hrs.EndHour) > Double.Parse(hours.EndHour)))
//                                        {
//                                            newh1.StartHour = hours.EndHour;
//                                            newh1.EndHour = hrs.EndHour;
//                                        }
//                                        else
//{
//    if ((Double.Parse(hrs.StartHour) < Double.Parse(hours.StartHour)) && (Double.Parse(hrs.EndHour) == Double.Parse(hours.EndHour)))
//    {
//        newh1.StartHour = hrs.StartHour;
//        newh1.EndHour = hours.StartHour;
//    }
//}
//                                    }
//                                    switch (dayi.Key)
//{
//    case 0:
//        Hours r1 = pspotSW.SundayHours.Find(h => ((h.StartHour == hrs.StartHour) && (h.EndHour == hrs.EndHour)));
//        pspotSW.SundayHours.Remove(r1);
//        if (newh2 != null)
//            pspotSW.SundayHours.Add(newh2);
//        if (newh1 != null)
//            pspotSW.SundayHours.Add(newh1);
//        break;
//    case 1:
//        Hours r2 = pspotSW.MondayHours.Find(h => ((h.StartHour == hrs.StartHour) && (h.EndHour == hrs.EndHour)));
//        pspotSW.MondayHours.Remove(r2);
//        if (newh2 != null)
//            pspotSW.MondayHours.Add(newh2);
//        if (newh1 != null)
//            pspotSW.MondayHours.Add(newh1); break;
//    case 2:
//        Hours r3 = pspotSW.TuesdayHours.Find(h => ((h.StartHour == hrs.StartHour) && (h.EndHour == hrs.EndHour)));
//        pspotSW.TuesdayHours.Remove(r3);
//        if (newh2 != null)
//            pspotSW.TuesdayHours.Add(newh2);
//        if (newh1 != null)
//            pspotSW.TuesdayHours.Add(newh1); break;
//    case 3:
//        Hours r4 = pspotSW.WednesdayHours.Find(h => ((h.StartHour == hrs.StartHour) && (h.EndHour == hrs.EndHour)));
//        pspotSW.WednesdayHours.Remove(r4);
//        if (newh2 != null)
//            pspotSW.WednesdayHours.Add(newh2);
//        if (newh1 != null)
//            pspotSW.WednesdayHours.Add(newh1);
//        break;
//    case 4:
//        Hours r5 = pspotSW.ThursdayHours.Find(h => ((h.StartHour == hrs.StartHour) && (h.EndHour == hrs.EndHour)));
//        pspotSW.ThursdayHours.Remove(r5);
//        if (newh2 != null)
//            pspotSW.ThursdayHours.Add(newh2);
//        if (newh1 != null)
//            pspotSW.ThursdayHours.Add(newh1);
//        break;
//    case 5:
//        Hours r6 = pspotSW.FridayHours.Find(h => ((h.StartHour == hrs.StartHour) && (h.EndHour == hrs.EndHour)));
//        pspotSW.FridayHours.Remove(r6);
//        if (newh2 != null)
//            pspotSW.FridayHours.Add(newh2);
//        if (newh1 != null)
//            pspotSW.FridayHours.Add(newh1);
//        break;
//    default:
//        break;
//}

//                                }
//                            }
//                        }
//                    }
//                    wbl = new WeekDayBL();
//var sw_updated = wbl.UpdateWeekDay(pspotSW);
//pspot.DaysSchedule = sw_updated.Code;
//pspbl = new ParkingSpotsBL();
//pspbl.UpdateUsersParkSpot(pspot);


////adding search result table
//Entities.SearchResult searchResult = new Entities.SearchResult()
//{
//    ResultPSCode = pspot.Code,
//    SearchCode = psearch.Code,
//    Usercode = psearch.UserId
//};
//DbHandler.AddSet(DAL.Convert.SearchResultConvert.ConvertSResultToEF(searchResult));


//                    //  WeekDay pSweekdaytbl = (DAL.Convert.WeekDayConvert.ConvertWeekDaysListToEntity(DbHandler.GetAll<DAL.WeekDay>()).First(w => w.Code == pspot.DaysSchedule));
//                    //Schedule_Week spotSch = WeekDayTblToSchedWeek(weekdaytbl);
//                }
//            }


    }

}
