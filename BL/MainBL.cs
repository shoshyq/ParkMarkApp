using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Entities;
using City = DAL.City;
using Feedback = DAL.Feedback;
using ParkingSpot = DAL.ParkingSpot;
using ParkingSpotSearch = DAL.ParkingSpotSearch;
using PaymentDetail = DAL.PaymentDetail;
using User = DAL.User;
using WeekDay = DAL.WeekDay;

namespace BL
{
    public class MainBL
    {
        HungarianFunctions hf;
        DBConnection DBCon;
        DistanceFunc df;

        //public static List<string> weekdays = new List<string>() { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
        public MainBL()
        {
            DBCon = new DBConnection();
            df = new DistanceFunc();
            hf = new HungarianFunctions();
        }

        #region add-functions

        //sign up function - returns user code
        public int SignUp(Entities.User user)
        {
            //if this username already exists
            if (!DbHandler.GetAll<User>().Any(d => d.Username.Trim() == user.Username.Trim()))
            {
                DbHandler.AddSet(user);
                return DbHandler.GetAll<User>().Where(u => u.Username == user.Username && u.UserPassword == user.UserPassword).Select(c => c.Code).ToList()[0];
            }

            return 0;
        }
        // login function - returns user code
        public int Login(string username, string password)
        {
            return DBCon.GetUserCode(username, password);
        }

        // registering new parking spot . returns 1 if succeeds
        public int RegisterUsersParkSpot(Entities.ParkingSpot mp)
        {
            if (!DbHandler.GetAll<ParkingSpot>().Any(d => d.Place_id.Trim() == mp.Place_id.Trim()))
            {
                //if (!DbHandler.GetAll<ParkingLocation>().Any(d => d.Place_Id.Trim() == mp.Place_id.Trim()))
                //{
                mp.Place_id = df.GetPlaceId(mp.FullAddress);
                DbHandler.AddSet(mp);

                //DbHandler.AddSet<ParkingLocation>(new ParkingLocation
                //{
                //    ParkingCode = mp.Code,
                //    Place_Id = mp.Place_id,
                //    FullAddress = mp.FullAddress
                //    //});
                //}
                return DbHandler.GetAll<ParkingSpot>().First(w => w.Code == mp.Code).Code;
            }
            return 0;

        }
        // adding a new city to cities table. returns 1 if succeeds
        public int AddCity(string cityname)
        {

            DbHandler.AddSet(new City { CityName = cityname });
            return 1;
        }

        // adding a new week to week table. gets a Schedule_Week/// and returns the code if succeeds, if not - 0
        public int AddWeekDays(Schedule_Week sw)
        {
            DAL.WeekDay wd = SchedWeekToWeekDayTbl(sw);
            DbHandler.AddSet(wd);
            return DbHandler.GetAll<WeekDay>().Any(w => w.Code == wd.Code) ? DbHandler.GetAll<WeekDay>().First(w => w.Code == wd.Code).Code : 0;

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
        // adding a new payment details table. returns code if succeeds
        public int AddPaymentDetails(Entities.PaymentDetail pd)
        {

            DbHandler.AddSet(pd);
            if (DbHandler.GetAll<PaymentDetail>().Any(p => p.Code == pd.Code))
                return DbHandler.GetAll<PaymentDetail>().First(p => p.Code == pd.Code).Code;
            return 0;
        }

        // adding new feedback. returns code if succeeds
        public int AddFeedback(Entities.Feedback f)
        {
            DbHandler.AddSet(f);
            checkUserAvRating((int)f.DescriptedUserCode);
            if (DbHandler.GetAll<Feedback>().Any(g => g.Code == f.Code))
                return DbHandler.GetAll<Feedback>().First(g => g.Code == f.Code).Code;
            return 0;

        }
        //returns 2 if can't add more payment accounts, returns the new payment code if succeeds
        public int CheckAndAddPaymentA(int usercode, Entities.PaymentDetail p)
        {
            if ((DbHandler.GetAll<User>().First(u => u.Code == usercode).PaymentDetails1 != null) && (DbHandler.GetAll<User>().First(u => u.Code == usercode).PaymentDetails2 != null))
                return 2;
            else
                return AddPaymentDetails(p);
        }
        #endregion
        #region update-functions
        // updating a parking spot search. returns code if succeeds
        public int UpdateParkingSpotSearch(Entities.ParkingSpotSearch pss)
        {
            pss.Place_id = df.GetPlaceId(pss.MyLocationAddress);

            DBCon.Execute(pss, DBConnection.ExecuteActions.Update);
            return DbHandler.GetAll<ParkingSpotSearch>().First(s => s.Code == pss.Code).Code;
        }
        // updating a feedback. returns code if succeeds
        public int UpdateFeedback(Entities.Feedback f)
        {
            DBCon.Execute(f, DBConnection.ExecuteActions.Update);
            return DbHandler.GetAll<Feedback>().First(g => g.Code == f.Code).Code;

        }


        // update user function. returns code if succeeds.
        public int UpdateUser(Entities.User user)
        {

            if (DBCon.GetUserCode(user.Username, user.UserPassword) != 0)
            {
                DBCon.Execute(user, DBConnection.ExecuteActions.Update);
                return user.Code;
            }
            return 0;
        }
        // update weekday function. returns code if succeeds.
        public int UpdateWeekDay(Schedule_Week sw)
        {
            DAL.WeekDay wd = SchedWeekToWeekDayTbl(sw);
            DbHandler.UpdateSet(wd);
            return DbHandler.GetAll<WeekDay>().Any(w => w.Code == wd.Code) ? DbHandler.GetAll<WeekDay>().First(w => w.Code == wd.Code).Code : 0;
        }

        // updating a parking spot . returns code if succeeds
        public int UpdateUsersParkSpot(Entities.ParkingSpot mp)
        {
            mp.Place_id = df.GetPlaceId(mp.FullAddress);

            DBCon.Execute(mp, DBConnection.ExecuteActions.Update);
            return mp.Code;
        }
        #endregion
        #region delete-functions
        public int DeleteUser(DAL.User u)
        {
            //success
            int result = 0;
            if (DeletePaymentDetailsByUser(u) == 1)
            {
                if (DeleteParkingSpotByUser(u) == 1)
                {
                    if (DeletePSSearchesByUser(u) == 1)
                    {
                        if (DeleteFeedbacksByUser(u.Code) == 1)
                            result = 1;
                    }
                }
            }
            DbHandler.DeleteSet(DbHandler.GetAll<User>().First(y => y.Code == u.Code));
            return result;
        }

        public int DeleteFeedbacksByUser(int usercode)
        {
            var flist = DbHandler.GetAll<Feedback>();
            if (flist != null)
            {
                foreach (var item in flist)
                {
                    if (item.DescriptedUserCode == usercode)
                    {
                        DbHandler.DeleteSet(item);
                    }
                }

            }
            return 1;
        }

        public int DeletePSSearchesByUser(DAL.User u)
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

        public int DeleteParkingSpotByUser(DAL.User u)
        {
            var pslist = DbHandler.GetAll<ParkingSpot>();
            if (pslist != null)
            {
                foreach (var item in pslist)
                {
                    if (item.UserCode == u.Code)
                    {

                        DbHandler.DeleteSet(item);

                    }
                    else
                        return 0;
                }
            }
            return 1;
        }

        public int DeleteFeedback(Feedback f)
        {

            var l = DbHandler.GetAll<Feedback>().First(i => i.Code == f.Code);
            DbHandler.DeleteSet(l);
            return 1;
        }
        public int DeleteParkingSpot(ParkingSpot p)
        {
            var l = DbHandler.GetAll<ParkingSpot>().First(i => i.Code == p.Code);
            DbHandler.DeleteSet(l);

            return 1;
        }
        public int DeleteParkingSpotSearch(ParkingSpotSearch pss)
        {
            var l = DbHandler.GetAll<ParkingSpotSearch>().First(i => i.Code == pss.Code);
            DbHandler.DeleteSet(l);

            return 1;
        }
        public int DeletePaymentDetailsByUser(DAL.User user)
        {
            var pdlist = DbHandler.GetAll<PaymentDetail>();
            if (pdlist != null)
            {
                foreach (var item in pdlist)
                {
                    if (item.Code == user.PaymentDetails1 || item.Code == user.PaymentDetails2)
                    {
                        DbHandler.DeleteSet(item);
                    }
                }
            }
            return 1;
        }
        public int DeletePaymentDetails(int usercode, PaymentDetail p)
        {
            if ((DbHandler.GetAll<User>().First(u => u.Code == usercode).PaymentDetails1 != null) && (DbHandler.GetAll<User>().First(u => u.Code == usercode).PaymentDetails2 != null))
            {
                DbHandler.DeleteSet(p);
                return 1;
            }
            else
                return 2;
        }
        #endregion
        #region convertion functions
        //converts List<ParkingSpot> to List<PSpotHandler>
        public List<PSpotHandler> ConvertToPSpotHandlerList(List<DAL.ParkingSpot> pslist)
        {
            List<PSpotHandler> parkSpotsMatrixList = new List<PSpotHandler>();
            for (int spot = 0; spot < pslist.Count; spot++)
            {
                var hourdic = GetHoursListFromWeekDay(pslist[spot].WeekDay.Code);
                PSpotHandler psh = new PSpotHandler()
                {
                    Code = pslist[spot].Code,
                    UserCode = pslist[spot].UserCode,
                    SpotWidth = pslist[spot].SpotWidth,
                    SpotLength = pslist[spot].SpotLength,
                    PricePerHour = pslist[spot].PricePerHour,
                    Place_Id = pslist[spot].Place_id,
                    Address = pslist[spot].FullAddress,
                    HasRoof = pslist[spot].HasRoof,
                    Hours = hourdic
                };
                parkSpotsMatrixList.Add(psh);
            }
            return parkSpotsMatrixList;
        }
        //converts List<ParkingSpotSearch> to List<PSpotSearchHandler>
        public List<PSpotSearchHandler> ConvertToSpotSearchHandlerList(List<DAL.ParkingSpotSearch> psearches)
        {
            List<PSpotSearchHandler> parkSearchesMatrixList = new List<PSpotSearchHandler>();
            for (int search = 0; search < psearches.Count; search++)
            {
                var hourdic2 = GetHoursListFromWeekDay(psearches[search].WeekDay.Code);
                PSpotSearchHandler psh = new PSpotSearchHandler()
                {
                    Code = psearches[search].Code,
                    SizeOpt = psearches[search].SizeOpt,
                    PreferableWidth = psearches[search].PreferableWidth,
                    PreferableLength = psearches[search].PreferableLength,
                    RoofOpt = psearches[search].RoofOpt,
                    MinPrice = psearches[search].MinPrice,
                    MaxPrice = psearches[search].MaxPrice,
                    Place_Id = psearches[search].Place_id,
                    Address = psearches[search].MyLocationAddress,
                    Hours = hourdic2
                };
                parkSearchesMatrixList.Add(psh);
            }
            return parkSearchesMatrixList;
        }

        //converts WeekDay table by Code from db/ returns dictionary - key: indexes a day (of weekdays list static prop above), value: list of Hour object per day
        public Dictionary<int, List<Hours>> GetHoursListFromWeekDay(int dscheduleCode)
        {
            DAL.WeekDay weekdaylist = DbHandler.GetAll<WeekDay>().First(w => w.Code == dscheduleCode);

            List<int> weekdayhours = new List<int>()
            {
                Convert.ToInt32(weekdaylist.SundayHourQuaters),
                Convert.ToInt32(weekdaylist.MondayHourQuaters),
                Convert.ToInt32(weekdaylist.TuedayHourQuaters),
                Convert.ToInt32(weekdaylist.WednesdayHourQuaters),
                Convert.ToInt32(weekdaylist.ThursdayHourQuaters),
                Convert.ToInt32(weekdaylist.FridayHourQuaters)
            };
            Dictionary<int, List<Hours>> wkdhdic = new Dictionary<int, List<Hours>>();
            int index = 0;
            foreach (var item in weekdayhours)
            {
                List<Hours> hourList = new List<Hours>();

                string byteshours = Convert.ToByte(item).ToString();
                int count = 0;
                bool flag = byteshours[0] == '1' ? true : false;
                for (int j = 0; j < 96; j++)
                {
                    if ((byteshours[j].Equals('1')) && (j != 95))
                    {
                        count++;
                        continue;
                    }

                    if (flag)
                    {
                        Hours h = new Hours(00.00, Convert.ToDouble(((j / 4).ToString() + "." + ((j % 4) * 15).ToString())));
                        hourList.Add(h);
                        count = 0;
                        flag = false;
                    }
                    else
                    {
                        Hours h = new Hours(Convert.ToDouble(((j - (count - 1)) / 4).ToString() + "." + (((j - (count - 1)) % 4) * 15).ToString()), Convert.ToDouble(((j + 1) / 4).ToString() + "." + (((j + 1) % 4) * 15).ToString()));
                        hourList.Add(h);
                        count = 0;
                        continue;
                    }
                }
                wkdhdic.Add(key: index, value: hourList);
                index++;
            }
            return wkdhdic;
        }
        // (for imidiate search) convert hours schedule for pspotsearch from weekDay to dictionary = key: index in weekdays list static prop above, value: Hours
        public Dictionary<int, Hours> GetHoursForPSImidiateSearch(int wdtCode)
        {
            DAL.WeekDay weekdaylist = DbHandler.GetAll<WeekDay>().First(w => w.Code == wdtCode);

            List<int> weekdayhours = new List<int>()
            {
                Convert.ToInt32(weekdaylist.SundayHourQuaters),
                Convert.ToInt32(weekdaylist.MondayHourQuaters),
                Convert.ToInt32(weekdaylist.TuedayHourQuaters),
                Convert.ToInt32(weekdaylist.WednesdayHourQuaters),
                Convert.ToInt32(weekdaylist.ThursdayHourQuaters),
                Convert.ToInt32(weekdaylist.FridayHourQuaters)
            };
            Dictionary<int, Hours> hours = new Dictionary<int, Hours>();
            for (int i = 0; i < 6; i++)
            {
                string byteshours = Convert.ToByte(weekdayhours[i]).ToString();
                int count = 0;
                bool flag = byteshours[0] == '1' ? true : false;
                for (int j = 0; j < 96; j++)
                {
                    if ((byteshours[j].Equals('1')) && (j != 95))
                        count++;
                    else
                    {
                        if (flag)
                            hours.Add(key: i, value: new Hours(00.00, Convert.ToDouble(((j / 4).ToString() + "." + ((j % 4) * 15).ToString()))));
                        else

                            hours.Add(key: i, value: new Hours(Convert.ToDouble(((j - (count - 1)) / 4).ToString() + "." + (((j - (count - 1)) % 4) * 15).ToString()), Convert.ToDouble(((j + 1) / 4).ToString() + "." + (((j + 1) % 4) * 15).ToString())));
                        break;
                    }
                }
            }
            return hours;
        }
        //converts double_format time to a number of bits
        public int ConvertToNumOfBits(double num)
        {
            int s_h = Convert.ToInt32(num) * 4;
            switch (num % (Convert.ToInt32(num)))
            {
                case (0):
                    s_h += 0;
                    break;
                case (0.15):
                    s_h += 1;
                    break;
                case (0.30):
                    s_h += 2;
                    break;
                case (0.45):
                    s_h += 3;
                    break;
            }
            return s_h;
        }
        // converts time schedule data from Schedule_Week to WeekDay table. uses the function GetWeekDayHours below. returns a WeekDay table
        public DAL.WeekDay SchedWeekToWeekDayTbl(Schedule_Week sw)
        {
            Dictionary<int, List<Hours>> lhdic = new Dictionary<int, List<Hours>>();
            lhdic[0] = sw.SundayHours;
            lhdic[1] = sw.MondayHours;
            lhdic[2] = sw.TuedayHours;
            lhdic[3] = sw.WednesdayHours;
            lhdic[4] = sw.ThursdayHours;
            lhdic[5] = sw.FridayHours;
            DAL.WeekDay wd = GetWeekDayHours(lhdic);
            wd.Code = sw.Code;

            return wd;
        }
        // converts time_schedule from dic to WeekDay table
        public DAL.WeekDay GetWeekDayHours(Dictionary<int, List<Hours>> hdic)
        {
            var weekdaylist = new DAL.WeekDay();

            foreach (var item in hdic)
            {
                int[] byteshours = new int[96];
                foreach (var hours in item.Value)
                {
                    if (hours != null)
                    {
                        int s_h = ConvertToNumOfBits(hours.StartHour);
                        int e_h = ConvertToNumOfBits(hours.EndHour);
                        for (int i = s_h; i < e_h - 1; i++)
                        {
                            byteshours[i] = 1;
                        }
                    }
                }
                switch (item.Key)
                {
                    case (0):
                        weekdaylist.SundayHourQuaters = Convert.ToByte(byteshours.ToString());
                        break;
                    case (1):
                        weekdaylist.MondayHourQuaters = Convert.ToByte(byteshours.ToString());
                        break;
                    case (2):
                        weekdaylist.TuedayHourQuaters = Convert.ToByte(byteshours.ToString());
                        break;
                    case (3):
                        weekdaylist.WednesdayHourQuaters = Convert.ToByte(byteshours.ToString());
                        break;
                    case (4):
                        weekdaylist.ThursdayHourQuaters = Convert.ToByte(byteshours.ToString());
                        break;
                    case (5):
                        weekdaylist.FridayHourQuaters = Convert.ToByte(byteshours.ToString());
                        break;
                }
            }
            return weekdaylist;
        }

        #endregion
        #region getter functions
        public List<DAL.Feedback> GetAllFeedbacksByUser(int usercode)
        {
            return DbHandler.GetAll<Feedback>().Where(f => f.DescriptedUserCode == usercode).ToList();
        }
        #endregion

        #region main functions
        //searching for the spot, imidiate
        public Dictionary<ParkingSpot, int> GetFiveClosestParkSpots(Entities.ParkingSpotSearch pss)
        {
            //gets all parking spots in this city
            var listOfSpots = DbHandler.GetAll<ParkingSpot>().Where(y => y.CityCode == pss.CityCode).ToList();
            //filtering by hours
            var shl = GetHoursForPSImidiateSearch(pss.WeekDay.Code);
            foreach (var spot in listOfSpots)
            {
                var hl = GetHoursListFromWeekDay(spot.WeekDay.Code);
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

        //once a day main-hungarian function - suppose to be here 
        public Dictionary<int, Dictionary<int, int>> ParkingSpotPerUser()
        {
            return hf.PSpotsAllSearchesByCities();
        }
        // confirmation of the result from user about the parking spot. updating the schedule-times table 
        public int ConfirmResult()
        {
            return 1;
        }
        #endregion

        //checking if user's rating is fine, if not, delete
        public int checkUserAvRating(int usercode)
        {
            var feedbacks = DbHandler.GetAll<Feedback>().Where(g => g.DescriptedUserCode == usercode).ToList();
            int sum_raiting = 0;
            foreach (var f in feedbacks)
            {
                sum_raiting += (int)(f.Rating);
            }
            if ((sum_raiting / feedbacks.Count()) < 2)
            {
                User user = DbHandler.GetAll<User>().First(u => u.Code == usercode);
                Email.MailToUserDeletingUser(user);
                if (DeleteUser(user) == 1)
                    return 1;
            }

            return 0;
        }


    }

}
