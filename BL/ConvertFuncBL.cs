using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

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

        //converts WeekDay table by Code from db/ returns dictionary - key: indexes a day (of weekdays list static prop above), value: list of Hour object per day
        public Dictionary<int, List<Hours>> GetHoursListFromWeekDay(int dscheduleCode)
        {
            WeekDay weekdaylist = (DbHandler.GetAll<WeekDay>()).First(w => w.Code == dscheduleCode);
            //DAL.Converts.WeekDayConvert.ConvertWeekDaysListToEntity(
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
            WeekDay weekdaylist = (DAL.Convert.WeekDayConvert.ConvertWeekDaysListToEntity(DbHandler.GetAll<DAL.WeekDay>())).First(w => w.Code == wdtCode);
            //DAL.Converts.WeekDayConvert.ConvertWeekDaysListToEntity
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
        public WeekDay SchedWeekToWeekDayTbl(Schedule_Week sw)
        {
            Dictionary<int, List<Hours>> lhdic = new Dictionary<int, List<Hours>>();
            lhdic[0] = sw.SundayHours;
            lhdic[1] = sw.MondayHours;
            lhdic[2] = sw.TuedayHours;
            lhdic[3] = sw.WednesdayHours;
            lhdic[4] = sw.ThursdayHours;
            lhdic[5] = sw.FridayHours;
            WeekDay wd = GetWeekDayHours(lhdic);
            wd.Code = sw.Code;

            return wd;
        }
        public Schedule_Week WeekDayTblToSchedWeek(WeekDay w)
        {
            Schedule_Week sw = new Schedule_Week();
            Dictionary<int, List<Hours>> lhdic = GetHoursListFromWeekDay(w.Code);
            sw.SundayHours = lhdic[0];
            sw.MondayHours = lhdic[1];
            sw.TuedayHours = lhdic[2];
           sw.WednesdayHours = lhdic[3];
            sw.ThursdayHours = lhdic[4];
            sw.FridayHours = lhdic[5];
            sw.Code = w.Code;

            return sw;
        }
        // converts time_schedule from dic to WeekDay table
        public WeekDay GetWeekDayHours(Dictionary<int, List<Hours>> hdic)
        {
            var weekdaylist = new WeekDay();

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

    }
}
