using Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BL
{
    public class ConvertFuncBL
    {
        #region convertion functions
        //converts List<ParkingSpot> to List<PSpotHandler>
        public List<PSpotHandler> ConvertToPSpotHandlerList(List<ParkingSpot> pslist)
        {
            List<PSpotHandler> parkSpotsMatrixList = new List<PSpotHandler>();
            for (int spot = 0; spot < pslist.Count; spot++)
            {
                var hourdic = GetHoursListFromWeekDay((int)pslist[spot].DaysSchedule);
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
        public List<PSpotSearchHandler> ConvertToSpotSearchHandlerList(List<ParkingSpotSearch> psearches)
        {
            List<PSpotSearchHandler> parkSearchesMatrixList = new List<PSpotSearchHandler>();
            for (int search = 0; search < psearches.Count; search++)
            {
                var hourdic2 = GetHoursListFromWeekDay((int)psearches[search].DaysSchedule);
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

        //updated - converts WeekDay table by Code from db/ returns dictionary - key: indexes a day ], value: list of Hour object per day
        public Dictionary<int, List<Hours>> GetHoursListFromWeekDay(int dscheduleCode)

        {
            WeekDay weekdaylist = (DbHandler.GetAll<WeekDay>()).First(w => w.Code == dscheduleCode);
            //DAL.Converts.WeekDayConvert.ConvertWeekDaysListToEntity(
            Dictionary<int, long> weekdayhours = new Dictionary<int, long>()
            {
            {  1,  Convert.ToInt64(weekdaylist.Sunday1HourQuaters)},
            {   2,  Convert.ToInt64(weekdaylist.Sunday2HourQuaters)},
            {  3,  Convert.ToInt64(weekdaylist.Monday1HourQuaters) },
            {    4,Convert.ToInt64(weekdaylist.Monday2HourQuaters) },
            {   5, Convert.ToInt64(weekdaylist.Tueday1HourQuaters) },
            {   6,  Convert.ToInt64(weekdaylist.Tueday2HourQuaters) },
            {  7,  Convert.ToInt64(weekdaylist.Wednesday1HourQuaters) },
            {   8,  Convert.ToInt64(weekdaylist.Wednesday2HourQuaters) },
            {  9,  Convert.ToInt64(weekdaylist.Thursday1HourQuaters) },
            {   10,  Convert.ToInt64(weekdaylist.Thursday2HourQuaters) },
            {  11,  Convert.ToInt64(weekdaylist.Friday1HourQuaters) },
            {  12,  Convert.ToInt64(weekdaylist.Friday2HourQuaters) }

            };
            Dictionary<int, List<Hours>> wkdhdic = new Dictionary<int, List<Hours>>();
            int index = 0;
            foreach (var item in weekdayhours)
            {
                List<Hours> hourList = new List<Hours>();
                char[] bitshours = new char[48];
                int k = 0;
                long num = item.Value;
                while (num != 0)
                {
                    bitshours[k++] = (num & 1) == 1 ? '1' : '0';
                    num >>= 1;
                }
                Array.Reverse(bitshours, 0, 48);
                //string strbithours = new string(bitshours);
                int count = 0;
                // bool flag = bitshours[0] == '1' ? true : false;
                //true if it's 1, if the previous was 1
                bool flag = bitshours[0] == '1' ? true : false;
                bool startflag = bitshours[0] == '1' ? true : false;
                double add = item.Key % 2 == 0 ? 12.00 : 00.00;
                for (int j = 1; j < 48; j++)
                {
                    if ((bitshours[j].Equals('1')) && (j == 47))
                    {
                        double end = item.Key % 2 == 0 ? 00.00 : 12.00;
                        Hours h = new Hours(Convert.ToDouble(((j - count) / 4).ToString() + "." + (((j - count) % 4) * 15).ToString()) + add, end);
                        hourList.Add(h);
                        break;
                    }
                    if ((bitshours[j].Equals('1')) && (j != 47))
                    {
                        count++;
                        flag = true;
                        continue;
                    }
                    else
                    {
                        if (startflag)
                        {
                            Hours h = new Hours(00.00 + add, Convert.ToDouble(((j / 4).ToString() + "." + ((j % 4) * 15).ToString())) + add);
                            hourList.Add(h);
                            count = 0;
                            flag = false;
                            startflag = false;
                            continue;
                        }
                        if (flag)
                        {
                            Hours h = new Hours(Convert.ToDouble(((j - count) / 4).ToString() + "." + (((j - count) % 4) * 15).ToString()) + add, Convert.ToDouble((j / 4).ToString() + "." + ((j % 4) * 15).ToString()) + add);
                            hourList.Add(h);
                            count = 0;
                            flag = false;
                            continue;
                        }

                    }

                }
                wkdhdic.Add(key: index, value: hourList);
                index++;
            }
            return wkdhdic;
        }

        // (for imidiate search) convert hours schedule for pspotsearch from weekDay to dictionary = key: index in weekdays list static prop above, value: Hours
        //public Dictionary<int, Hours> GetHoursForPSImidiateSearch(int wdtCode)
        //{
        //    WeekDay weekdaylist = (DAL.Convert.WeekDayConvert.ConvertWeekDaysListToEntity(DbHandler.GetAll<DAL.WeekDay>())).First(w => w.Code == wdtCode);
        //    //DAL.Converts.WeekDayConvert.ConvertWeekDaysListToEntity
        //    Dictionary<int, long> weekdayhours = new Dictionary<int, long>()
        //    {
        //    {  1,  Convert.ToInt64(weekdaylist.Sunday1HourQuaters)},
        //    {   2,  Convert.ToInt64(weekdaylist.Sunday2HourQuaters)},
        //    {  3,  Convert.ToInt64(weekdaylist.Monday1HourQuaters) },
        //    {    4,Convert.ToInt64(weekdaylist.Monday2HourQuaters) },
        //    {   5, Convert.ToInt64(weekdaylist.Tueday1HourQuaters) },
        //    {   6,  Convert.ToInt64(weekdaylist.Tueday2HourQuaters) },
        //    {  7,  Convert.ToInt64(weekdaylist.Wednesday1HourQuaters) },
        //    {   8,  Convert.ToInt64(weekdaylist.Wednesday2HourQuaters) },
        //    {  9,  Convert.ToInt64(weekdaylist.Thursday1HourQuaters) },
        //    {   10,  Convert.ToInt64(weekdaylist.Thursday2HourQuaters) },
        //    {  11,  Convert.ToInt64(weekdaylist.Friday1HourQuaters) },
        //    {  12,  Convert.ToInt64(weekdaylist.Friday2HourQuaters) }

        //    };
        //    Dictionary<int, Hours> hours = new Dictionary<int, Hours>();
        //    int index = 0;
        //    foreach (var item in weekdayhours)
        //    {
        //        List<Hours> hourList = new List<Hours>();
        //        char[] bitshours = new char[48];
        //        int k = 0;
        //        long num = item.Value;
        //        while (num != 0)
        //        {
        //            bitshours[k++] = (num & 1) == 1 ? '1' : '0';
        //            num >>= 1;
        //        }
        //        Array.Reverse(bitshours, 0, 48);
        //        //string strbithours = new string(bitshours);
        //        int count = 0;
        //        // bool flag = bitshours[0] == '1' ? true : false;
        //        //true if it's 1, if the previous was 1
        //        bool flag = bitshours[0] == '1' ? true : false;
        //        bool startflag = bitshours[0] == '1' ? true : false;
        //        double add = item.Key % 2 == 0 ? 12.00 : 00.00;
        //        for (int j = 1; j < 48; j++)
        //        {
        //            if ((bitshours[j].Equals('1')) && (j == 47))
        //            {
        //                double end = item.Key % 2 == 0 ? 00.00 : 12.00;
        //                Hours h = new Hours(Convert.ToDouble(((j - count) / 4).ToString() + "." + (((j - count) % 4) * 15).ToString()) + add, end);
        //                hourList.Add(h);
        //                break;
        //            }
        //            if ((bitshours[j].Equals('1')) && (j != 47))
        //            {
        //                count++;
        //                flag = true;
        //                continue;
        //            }
        //            else
        //            {
        //                if (startflag)
        //                {
        //                    Hours h = new Hours(00.00 + add, Convert.ToDouble(((j / 4).ToString() + "." + ((j % 4) * 15).ToString())) + add);
        //                    hourList.Add(h);
        //                    count = 0;
        //                    flag = false;
        //                    startflag = false;
        //                    continue;
        //                }
        //                if (flag)
        //                {
        //                    Hours h = new Hours(Convert.ToDouble(((j - count) / 4).ToString() + "." + (((j - count) % 4) * 15).ToString()) + add, Convert.ToDouble((j / 4).ToString() + "." + ((j % 4) * 15).ToString()) + add);
        //                    hourList.Add(h);
        //                    count = 0;
        //                    flag = false;
        //                    continue;
        //                }

        //            }

        //        }
        //        hours.Add(key: index, value: hourList[0]);
        //        index++;
        //    }
        //    return hours;
        //}
        //updated - converts double_format time to a number of bits
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
        public Schedule_Week WeekDayTblToSchedWeek(WeekDay w)
        {
            Dictionary<int, List<Hours>> lhdic =  GetHoursListFromWeekDay(w.Code);
            Schedule_Week sw = new Schedule_Week();          
            sw.SundayHours = lhdic[0];
            sw.MondayHours = lhdic[1];
            sw.TuedayHours = lhdic[2];
            sw.WednesdayHours = lhdic[3];
            sw.ThursdayHours = lhdic[4];
            sw.FridayHours = lhdic[5];
            sw.Code = w.Code;

            return sw;
        }
        //// converts time_schedule from dic to WeekDay table
        //public WeekDay GetWeekDayHours(Dictionary<int, List<Hours>> hdic)
        //{
        //    var weekday = new WeekDay();

        //    foreach (var item in hdic)
        //    {
        //        int[] byteshours = new int[96];
        //        foreach (var hours in item.Value)
        //        {
        //            if (hours != null)
        //            {
        //                int s_h = ConvertToNumOfBits(hours.StartHour);
        //                int e_h = ConvertToNumOfBits(hours.EndHour);
        //                for (int i = s_h; i < e_h - 1; i++)
        //                {
        //                    byteshours[i] = 1;
        //                }
        //            }
        //        }
        //        switch (item.Key)
        //        {
        //            case (0):
        //                weekdaylist.SundayHourQuaters = Convert.ToByte(byteshours.ToString());
        //                break;
        //            case (1):
        //                weekdaylist.MondayHourQuaters = Convert.ToByte(byteshours.ToString());
        //                break;
        //            case (2):
        //                weekdaylist.TuedayHourQuaters = Convert.ToByte(byteshours.ToString());
        //                break;
        //            case (3):
        //                weekdaylist.WednesdayHourQuaters = Convert.ToByte(byteshours.ToString());
        //                break;
        //            case (4):
        //                weekdaylist.ThursdayHourQuaters = Convert.ToByte(byteshours.ToString());
        //                break;
        //            case (5):
        //                weekdaylist.FridayHourQuaters = Convert.ToByte(byteshours.ToString());
        //                break;
        //        }
        //    }
        //    return weekday;
        //}
        // converts time schedule data from Schedule_Week to WeekDay table. uses the function GetWeekDayHours below. returns a WeekDay table

        public WeekDay GetWeekDayHours(Schedule_Week sw)
        {
            Dictionary<int, List<Hours>> hoursdic = GetHoursListFromScheduleWeek(sw);
            var weekday = new WeekDay();

            foreach (var item in hoursdic)
            {
                int[] hinbits = new int[48];
                foreach (var hours in item.Value)
                {
                    if (hours != null)
                    {
                        int s_h = ConvertToNumOfBits(hours.StartHour);
                        int e_h = ConvertToNumOfBits(hours.EndHour);
                        for (int j = s_h; j < e_h; j++)
                        {
                            hinbits[j] = 1;
                        }
                    }

                }
                string trystr = string.Join("", hinbits);
                long encryptedhours = Convert.ToInt64(trystr, 2);

                switch (item.Key)
                {

                    case (0):
                        weekday.Sunday1HourQuaters = encryptedhours;
                        break;
                    case (1):
                        weekday.Sunday2HourQuaters = encryptedhours;
                        break;
                    case (2):
                        weekday.Monday1HourQuaters = encryptedhours;
                        break;
                    case (3):
                        weekday.Monday2HourQuaters = encryptedhours;
                        break;
                    case (4):
                        weekday.Tueday1HourQuaters = encryptedhours;
                        break;
                    case (5):
                        weekday.Tueday2HourQuaters = encryptedhours;
                        break;
                    case (6):
                        weekday.Wednesday1HourQuaters = encryptedhours;
                        break;
                    case (7):
                        weekday.Wednesday2HourQuaters = encryptedhours;
                        break;
                    case (8):
                        weekday.Thursday1HourQuaters = encryptedhours;
                        break;
                    case (9):
                        weekday.Thursday2HourQuaters = encryptedhours;
                        break;
                    case (10):
                        weekday.Friday1HourQuaters = encryptedhours;
                        break;
                    case (11):
                        weekday.Friday2HourQuaters = encryptedhours;
                        break;
                }
            }

            return weekday;
        }
        public Schedule_Week GetSchedule_WeekFromHoursList(int code, Dictionary<int, List<Hours>> hdic)
        {
            Schedule_Week sw = new Schedule_Week();
            Hours midlleh = new Hours();
            foreach (var item in hdic[0])
            {
                if ((item.StartHour >= 00.00) && (item.EndHour < 12.00))
                {
                    sw.SundayHours.Add(item);
                }
                if ((item.StartHour >= 00.00) && (item.EndHour == 12.00))
                {
                    midlleh.StartHour = item.StartHour;
                }
            }
            foreach (var item in hdic[1])
            {
                if ((item.StartHour > 12.00) && (item.EndHour <= 00.00))
                {
                    sw.SundayHours.Add(item);
                }
                if ((item.StartHour == 12.00) && (item.EndHour <= 00.00))
                {
                    midlleh.EndHour = item.EndHour;
                }
            }
            sw.SundayHours.Add(midlleh);
            foreach (var item in hdic[2])
            {
                if ((item.StartHour >= 00.00) && (item.EndHour < 12.00))
                {
                    sw.MondayHours.Add(item);
                }
                if ((item.StartHour >= 00.00) && (item.EndHour == 12.00))
                {
                    midlleh.StartHour = item.StartHour;
                }
            }
            foreach (var item in hdic[3])
            {
                if ((item.StartHour > 12.00) && (item.EndHour <= 00.00))
                {
                    sw.MondayHours.Add(item);
                }
                if ((item.StartHour == 12.00) && (item.EndHour <= 00.00))
                {
                    midlleh.EndHour = item.EndHour;
                }
            }
            sw.MondayHours.Add(midlleh);
            foreach (var item in hdic[4])
            {
                if ((item.StartHour >= 00.00) && (item.EndHour < 12.00))
                {
                    sw.TuedayHours.Add(item);
                }
                if ((item.StartHour >= 00.00) && (item.EndHour == 12.00))
                {
                    midlleh.StartHour = item.StartHour;
                }
            }
            foreach (var item in hdic[5])
            {
                if ((item.StartHour > 12.00) && (item.EndHour <= 00.00))
                {
                    sw.TuedayHours.Add(item);
                }
                if ((item.StartHour == 12.00) && (item.EndHour <= 00.00))
                {
                    midlleh.EndHour = item.EndHour;
                }
            }
            sw.TuedayHours.Add(midlleh);
            foreach (var item in hdic[6])
            {
                if ((item.StartHour >= 00.00) && (item.EndHour < 12.00))
                {
                    sw.WednesdayHours.Add(item);
                }
                if ((item.StartHour >= 00.00) && (item.EndHour == 12.00))
                {
                    midlleh.StartHour = item.StartHour;
                }
            }
            foreach (var item in hdic[7])
            {
                if ((item.StartHour > 12.00) && (item.EndHour <= 00.00))
                {
                    sw.WednesdayHours.Add(item);
                }
                if ((item.StartHour == 12.00) && (item.EndHour <= 00.00))
                {
                    midlleh.EndHour = item.EndHour;
                }
            }
            sw.WednesdayHours.Add(midlleh);
            foreach (var item in hdic[8])
            {
                if ((item.StartHour >= 00.00) && (item.EndHour < 12.00))
                {
                    sw.ThursdayHours.Add(item);
                }
                if ((item.StartHour >= 00.00) && (item.EndHour == 12.00))
                {
                    midlleh.StartHour = item.StartHour;
                }
            }
            foreach (var item in hdic[9])
            {
                if ((item.StartHour > 12.00) && (item.EndHour <= 00.00))
                {
                    sw.ThursdayHours.Add(item);
                }
                if ((item.StartHour == 12.00) && (item.EndHour <= 00.00))
                {
                    midlleh.EndHour = item.EndHour;
                }
            }
            sw.ThursdayHours.Add(midlleh);
            foreach (var item in hdic[10])
            {
                if ((item.StartHour >= 00.00) && (item.EndHour < 12.00))
                {
                    sw.FridayHours.Add(item);
                }
                if ((item.StartHour >= 00.00) && (item.EndHour == 12.00))
                {
                    midlleh.StartHour = item.StartHour;
                }
            }
            foreach (var item in hdic[11])
            {
                if ((item.StartHour > 12.00) && (item.EndHour <= 00.00))
                {
                    sw.FridayHours.Add(item);
                }
                if ((item.StartHour == 12.00) && (item.EndHour <= 00.00))
                {
                    midlleh.EndHour = item.EndHour;
                }
            }
            sw.FridayHours.Add(midlleh);
            sw.Code = code;
            return sw;
        }
        public Dictionary<int, List<Hours>> GetHoursListFromScheduleWeek(Schedule_Week sw)
        {
            Dictionary<int, List<Hours>> hdic = new Dictionary<int, List<Hours>>();
            List<Hours> fhours = new List<Hours>();
            List<Hours> shours = new List<Hours>();
            foreach (var item in sw.SundayHours)
            {
                if (((item.StartHour >= 00.00) && (item.EndHour <= 12.00)) || ((item.StartHour >= 12.00) && (item.EndHour <= 00.00)))
                {
                    shours.Add(item);
                }
                if ((item.StartHour >= 00.00) && (item.EndHour > 12.00))
                {

                    fhours.Add(new Hours(item.StartHour, 12.00));
                    shours.Add(new Hours(00.00, item.EndHour - 12));
                }
            }
            hdic.Add(key: 0, fhours);
            hdic.Add(key: 1, shours);
            fhours.Clear();
            shours.Clear();
            foreach (var item in sw.MondayHours)
            {
                if (((item.StartHour >= 00.00) && (item.EndHour <= 12.00)) || ((item.StartHour >= 12.00) && (item.EndHour <= 00.00)))
                {
                    shours.Add(item);
                }
                if ((item.StartHour >= 00.00) && (item.EndHour > 12.00))
                {

                    fhours.Add(new Hours(item.StartHour, 12.00));
                    shours.Add(new Hours(00.00, item.EndHour - 12));
                }
            }
            hdic.Add(key: 2, fhours);
            hdic.Add(key: 3, shours);
            fhours.Clear();
            shours.Clear();
            foreach (var item in sw.TuedayHours)
            {
                if (((item.StartHour >= 00.00) && (item.EndHour <= 12.00)) || ((item.StartHour >= 12.00) && (item.EndHour <= 00.00)))
                {
                    shours.Add(item);
                }
                if ((item.StartHour >= 00.00) && (item.EndHour > 12.00))
                {

                    fhours.Add(new Hours(item.StartHour, 12.00));
                    shours.Add(new Hours(00.00, item.EndHour - 12));
                }
            }
            hdic.Add(key: 4, fhours);
            hdic.Add(key: 5, shours);
            fhours.Clear();
            shours.Clear();
            foreach (var item in sw.WednesdayHours)
            {
                if (((item.StartHour >= 00.00) && (item.EndHour <= 12.00)) || ((item.StartHour >= 12.00) && (item.EndHour <= 00.00)))
                {
                    shours.Add(item);
                }
                if ((item.StartHour >= 00.00) && (item.EndHour > 12.00))
                {

                    fhours.Add(new Hours(item.StartHour, 12.00));
                    shours.Add(new Hours(00.00, item.EndHour - 12));
                }
            }
            hdic.Add(key: 6, fhours);
            hdic.Add(key: 7, shours);
            fhours.Clear();
            shours.Clear();
            foreach (var item in sw.ThursdayHours)
            {
                if (((item.StartHour >= 00.00) && (item.EndHour <= 12.00)) || ((item.StartHour >= 12.00) && (item.EndHour <= 00.00)))
                {
                    shours.Add(item);
                }
                if ((item.StartHour >= 00.00) && (item.EndHour > 12.00))
                {

                    fhours.Add(new Hours(item.StartHour, 12.00));
                    shours.Add(new Hours(00.00, item.EndHour - 12));
                }
            }
            hdic.Add(key: 8, fhours);
            hdic.Add(key: 9, shours);
            fhours.Clear();
            shours.Clear();
            foreach (var item in sw.FridayHours)
            {
                if (((item.StartHour >= 00.00) && (item.EndHour <= 12.00)) || ((item.StartHour >= 12.00) && (item.EndHour <= 00.00)))
                {
                    shours.Add(item);
                }
                if ((item.StartHour >= 00.00) && (item.EndHour > 12.00))
                {

                    fhours.Add(new Hours(item.StartHour, 12.00));
                    shours.Add(new Hours(00.00, item.EndHour - 12));
                }
            }
            hdic.Add(key: 10, fhours);
            hdic.Add(key: 11, shours);
            return hdic;
        }

        #endregion

    }
}
