using Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BL
{
    public class ConvertFuncBL
    {
        WeekDayBL wbl;
       
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
                    Hours = hourdic2,
                    SearchDate = psearches[search].SearchDate
                };
                parkSearchesMatrixList.Add(psh);
            }
            return parkSearchesMatrixList;
        }

        public ParkingSpot UpdatePSpotSchedule(int dayi, string sh, string eh, ParkingSpot pspot)
        {
            var spotHours = GetHoursListFromWeekDay((int)pspot.DaysSchedule);
            WeekDay weekdaylist = (DAL.Convert.WeekDayConvert.ConvertWeekDaysListToEntity(DbHandler.GetAll<DAL.WeekDay>()).First(w => w.Code == pspot.DaysSchedule));
            Schedule_Week sw = WeekDayTblToSchedWeek(weekdaylist);
    
                bool keyExists = spotHours.ContainsKey(dayi);
            if (keyExists)
            {
                if (spotHours[dayi].Any(h => (Double.Parse(h.StartHour) <= Double.Parse(sh)) && (Double.Parse(h.EndHour) >= Double.Parse(eh))))
                {
                    Hours hrs = spotHours[dayi].First(h => (Double.Parse(h.StartHour) <= Double.Parse(sh)) && (Double.Parse(h.EndHour) >= Double.Parse(eh)));
                    Hours newh1 = new Hours();
                    Hours newh2 = new Hours();
                    if ((Double.Parse(hrs.StartHour) < Double.Parse(sh)) && (Double.Parse(hrs.EndHour) > Double.Parse(eh)))
                    {
                        newh1.StartHour = hrs.StartHour;
                        newh1.EndHour = sh;
                        newh2.StartHour = eh;
                        newh2.EndHour = hrs.EndHour;
                    }
                    else
                    {
                        if ((Double.Parse(hrs.StartHour) == Double.Parse(sh)) && (Double.Parse(hrs.EndHour) > Double.Parse(eh)))
                        {
                            newh1.StartHour = eh;
                            newh1.EndHour = hrs.EndHour;
                        }
                        else
                        {
                            if ((Double.Parse(hrs.StartHour) < Double.Parse(sh)) && (Double.Parse(hrs.EndHour) == Double.Parse(eh)))
                            {
                                newh1.StartHour = hrs.StartHour;
                                newh1.EndHour = sh;
                            }
                        }
                    }
                    switch (dayi)
                    {
                        case 0:
                            Hours r1 = sw.SundayHours.Find(h => ((h.StartHour == hrs.StartHour) && (h.EndHour == hrs.EndHour)));
                            sw.SundayHours.Remove(r1);
                            if (newh2 != null)
                                sw.SundayHours.Add(newh2);
                            if (newh1 != null)
                                sw.SundayHours.Add(newh1);
                            break;
                        case 1:
                            Hours r2 = sw.MondayHours.Find(h => ((h.StartHour == hrs.StartHour) && (h.EndHour == hrs.EndHour)));
                            sw.MondayHours.Remove(r2);
                            if (newh2 != null)
                                sw.MondayHours.Add(newh2);
                            if (newh1 != null)
                                sw.MondayHours.Add(newh1); break;
                        case 2:
                            Hours r3 = sw.TuesdayHours.Find(h => ((h.StartHour == hrs.StartHour) && (h.EndHour == hrs.EndHour)));
                            sw.TuesdayHours.Remove(r3);
                            if (newh2 != null)
                                sw.TuesdayHours.Add(newh2);
                            if (newh1 != null)
                                sw.TuesdayHours.Add(newh1); break;
                        case 3:
                            Hours r4 = sw.WednesdayHours.Find(h => ((h.StartHour == hrs.StartHour) && (h.EndHour == hrs.EndHour)));
                            sw.WednesdayHours.Remove(r4);
                            if (newh2 != null)
                                sw.WednesdayHours.Add(newh2);
                            if (newh1 != null)
                                sw.WednesdayHours.Add(newh1);
                            break;
                        case 4:
                            Hours r5 = sw.ThursdayHours.Find(h => ((h.StartHour == hrs.StartHour) && (h.EndHour == hrs.EndHour)));
                            sw.ThursdayHours.Remove(r5);
                            if (newh2 != null)
                                sw.ThursdayHours.Add(newh2);
                            if (newh1 != null)
                                sw.ThursdayHours.Add(newh1);
                            break;
                        case 5:
                            Hours r6 = sw.FridayHours.Find(h => ((h.StartHour == hrs.StartHour) && (h.EndHour == hrs.EndHour)));
                            sw.FridayHours.Remove(r6);
                            if (newh2 != null)
                                sw.FridayHours.Add(newh2);
                            if (newh1 != null)
                                sw.FridayHours.Add(newh1);
                            break;
                        default:
                            break;
                    }

                }
            }
               
            wbl = new WeekDayBL();
            var sw_updated = wbl.UpdateWeekDay(sw);
            pspot.DaysSchedule = sw_updated.Code;
            WeekDay weekdaytbl = (DAL.Convert.WeekDayConvert.ConvertWeekDaysListToEntity(DbHandler.GetAll<DAL.WeekDay>()).First(w => w.Code == pspot.DaysSchedule));
            Schedule_Week spotSch = WeekDayTblToSchedWeek(weekdaytbl);
            return pspot;
        }

        //updated - converts WeekDay table by Code from db/ returns dictionary - key: indexes a day ], value: list of Hour object per day
        public Dictionary<int, List<Hours>> GetHoursListFromWeekDay(int dscheduleCode)
        {
            Dictionary<int, List<Hours>> wkdhdic = new Dictionary<int, List<Hours>>();
            WeekDay weekdaylist = (DAL.Convert.WeekDayConvert.ConvertWeekDaysListToEntity(DbHandler.GetAll<DAL.WeekDay>()).First(w => w.Code == dscheduleCode));
            Schedule_Week sw = WeekDayTblToSchedWeek(weekdaylist);
            if (sw.SundayHours != null)
            {
                List<Hours> shrs = new List<Hours>();
                foreach (var item in sw.SundayHours)
                {
                    shrs.Add(new Hours() { StartHour = item.StartHour.ToString().Trim(), EndHour = item.EndHour.ToString().Trim() });
                }
                wkdhdic.Add(key: 0, value: shrs);
            }
            if (sw.MondayHours != null)
            {
                List<Hours> mhrs = new List<Hours>();
                foreach (var item in sw.MondayHours)
                {
                    mhrs.Add(new Hours() { StartHour = item.StartHour.ToString().Trim(), EndHour = item.EndHour.ToString().Trim() });
                }
                wkdhdic.Add(key: 1, value: mhrs);
            }
            if (sw.TuesdayHours != null)
            {
                List<Hours> tuhrs = new List<Hours>();
                foreach (var item in sw.TuesdayHours)
                {
                    tuhrs.Add(new Hours() { StartHour = item.StartHour.ToString().Trim(), EndHour = item.EndHour.ToString().Trim() });
                }
                wkdhdic.Add(key: 2, value: tuhrs);
            }
            if (sw.WednesdayHours != null)
            {
                List<Hours> wedhrs = new List<Hours>();
                foreach (var item in sw.WednesdayHours)
                {
                    wedhrs.Add(new Hours() { StartHour = item.StartHour.ToString().Trim(), EndHour = item.EndHour.ToString().Trim() });
                }
                wkdhdic.Add(key: 3, value: wedhrs);
            }
            if (sw.ThursdayHours != null)
            {
                List<Hours> thdhrs = new List<Hours>();
                foreach (var item in sw.ThursdayHours)
                {
                    thdhrs.Add(new Hours() { StartHour = item.StartHour.ToString().Trim(), EndHour = item.EndHour.ToString().Trim() });
                }
                wkdhdic.Add(key: 4, value: thdhrs);
            }
            if (sw.FridayHours != null)
            {
                List<Hours> fridhrs = new List<Hours>();
                foreach (var item in sw.FridayHours)
                {
                    fridhrs.Add(new Hours() { StartHour = item.StartHour.ToString().Trim(), EndHour = item.EndHour.ToString().Trim() });
                }
                wkdhdic.Add(key: 5, value: fridhrs);
            }
            return wkdhdic;
        }
        //{
        //    WeekDay weekdaylist = (DbHandler.GetAll<WeekDay>()).First(w => w.Code == dscheduleCode);
        //    //DAL.Converts.WeekDayConvert.ConvertWeekDaysListToEntity(
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
        //    Dictionary<int, List<Hours>> wkdhdic = new Dictionary<int, List<Hours>>();
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
        //                Hours h = new Hours((Convert.ToDouble(((j - count) / 4).ToString() + "." + (((j - count) % 4) * 15).ToString()) + add).ToString(), end.ToString());
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
        //                    Hours h = new Hours((00.00 + add).ToString(), (Convert.ToDouble(((j / 4).ToString() + "." + ((j % 4) * 15).ToString())) + add).ToString());
        //                    hourList.Add(h);
        //                    count = 0;
        //                    flag = false;
        //                    startflag = false;
        //                    continue;
        //                }
        //                if (flag)
        //                {
        //                    Hours h = new Hours((Convert.ToDouble(((j - count) / 4).ToString() + "." + (((j - count) % 4) * 15).ToString()) + add).ToString(),( Convert.ToDouble((j / 4).ToString() + "." + ((j % 4) * 15).ToString()) + add).ToString());
        //                    hourList.Add(h);
        //                    count = 0;
        //                    flag = false;
        //                    continue;
        //                }

        //            }

        //        }
        //        wkdhdic.Add(key: index, value: hourList);
        //        index++;
        //    }
        //    return wkdhdic;
        //}

        //public int ConvertToNumOfBits(double num)
        //{
        //    int s_h = Convert.ToInt32(num) * 4;
        //    switch (num % (Convert.ToInt32(num)))
        //    {
        //        case (0):
        //            s_h += 0;
        //            break;
        //        case (0.15):
        //            s_h += 1;
        //            break;
        //        case (0.30):
        //            s_h += 2;
        //            break;
        //        case (0.45):
        //            s_h += 3;
        //            break;
        //    }
        //    return s_h;
        //}
        public Schedule_Week WeekDayTblToSchedWeek(WeekDay w)
        {
            //Dictionary<int, List<Hours>> lhdic =  GetHoursListFromWeekDay(w.Code);
            Schedule_Week sw = new Schedule_Week();
            sw.Code = w.Code;
            if ((w.Sunday != null) && (w.Sunday != ""))
            {
                List<Hours> hours1list = new List<Hours>();
                string[] sundaylst = w.Sunday.Split(',');
                foreach (var hours in sundaylst)
                {
                    if (!(hours == " ") || (hours == ""))
                    {
                        string[] resHours = hours.Trim().Split('-');
                        hours1list.Add(new Hours()
                        {
                            StartHour = resHours[0],
                            EndHour = resHours[1]
                        });
                    }


                }
                sw.SundayHours = hours1list;
            }
            if ((w.Monday != null) && (w.Monday != ""))
            {
                List<Hours> hours2list = new List<Hours>();

                string[] mondaylst = w.Monday.Split(',');
                foreach (var hours in mondaylst)
                {
                    if (!(hours == " ") || (hours == ""))
                    {
                        string[] resHours = hours.Trim().Split('-');
                        hours2list.Add(new Hours()
                        {
                            StartHour = resHours[0],
                            EndHour = resHours[1]
                        });
                    }

                }
                sw.MondayHours = hours2list;
            }
            if ((w.Tuesday != null) && (w.Tuesday != ""))
            {
                string[] tuelst = w.Tuesday.Split(',');
                List<Hours> hours3list = new List<Hours>();

                foreach (var hours in tuelst)
                {
                    if (!(hours == " ") || (hours == ""))
                    {
                        string[] resHours = hours.Trim().Split('-');
                        hours3list.Add(new Hours()
                        {
                            StartHour = resHours[0],
                            EndHour = resHours[1]
                        });
                    }

                }
                sw.TuesdayHours = hours3list;
            }
            if ((w.Wednesday != null) && (w.Wednesday != ""))
            {
                string[] wedlst = w.Wednesday.Split(',');
                List<Hours> hours4list = new List<Hours>();

                foreach (var hours in wedlst)
                {
                    if (!(hours == " ") || (hours == ""))
                    {
                        string[] resHours = hours.Trim().Split('-');
                        hours4list.Add(new Hours()
                        {
                            StartHour = resHours[0],
                            EndHour = resHours[1]
                        });
                    }

                }
                sw.WednesdayHours = hours4list;
            }
            if ((w.Thursday != null) && (w.Thursday != ""))
            {
                string[] thrlst = w.Thursday.Split(',');
                List<Hours> hours5list = new List<Hours>();

                foreach (var hours in thrlst)
                {
                    if (!(hours == " ") || (hours == ""))
                    {
                        string[] resHours = hours.Trim().Split('-');
                        hours5list.Add(new Hours()
                        {
                            StartHour = resHours[0],
                            EndHour = resHours[1]
                        });
                    }

                }
                sw.ThursdayHours = hours5list;
            }
            if ((w.Friday != null) && (w.Friday != ""))
            {
                List<Hours> hours6list = new List<Hours>();

                string[] frilst = w.Friday.Split(',');
                foreach (var hours in frilst)
                {
                    if (!(hours == " ") || (hours == ""))
                    {
                        string[] resHours = hours.Trim().Split('-');
                        hours6list.Add(new Hours()
                        {
                            StartHour = resHours[0],
                            EndHour = resHours[1]
                        });
                    }

                }
                sw.FridayHours = hours6list;
            }

            //sw.SundayHours = lhdic[0];
            //sw.MondayHours = lhdic[1];
            //sw.TuedayHours = lhdic[2];
            //sw.WednesdayHours = lhdic[3];
            //sw.ThursdayHours = lhdic[4];
            //sw.FridayHours = lhdic[5];
            //sw.Code = w.Code;

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

            // Dictionary<int, List<Hours>> hoursdic = GetHoursListFromScheduleWeek(sw);
            // Dictionary<int, List<Hours>> hoursdic = new Dictionary<int, List<Hours>>();
            WeekDay weekday = new WeekDay();
            weekday.Code = sw.Code;
            if (sw.SundayHours != null)
            {
                foreach (var item in sw.SundayHours)
                {
                    if ((item.EndHour == "00.00") || (item.EndHour == "0.00") || (item.EndHour == "0.0"))
                        item.EndHour = "24.00";
                    weekday.Sunday += item.StartHour.ToString().Trim() + "-" + item.EndHour.ToString().Trim() + ", ";
                }
            }
            if (sw.MondayHours != null)
            {
                foreach (var item in sw.MondayHours)
                {
                    if ((item.EndHour == "00.00") || (item.EndHour == "0.00") || (item.EndHour == "0.0"))
                        item.EndHour = "24.00";
                    weekday.Monday += item.StartHour.ToString().Trim() + "-" + item.EndHour.ToString().Trim() + ", ";
                }
            }
            if (sw.TuesdayHours != null)
            {
                foreach (var item in sw.TuesdayHours)
                {
                    if ((item.EndHour == "00.00") || (item.EndHour == "0.00") || (item.EndHour == "0.0"))
                        item.EndHour = "24.00";
                    weekday.Tuesday += item.StartHour.ToString().Trim() + "-" + item.EndHour.ToString().Trim() + ", ";
                }
            }
            if (sw.WednesdayHours != null)
            {
                foreach (var item in sw.WednesdayHours)
                {
                    if ((item.EndHour == "00.00") || (item.EndHour == "0.00") || (item.EndHour == "0.0"))
                        item.EndHour = "24.00";
                    weekday.Wednesday += item.StartHour.ToString().Trim() + "-" + item.EndHour.ToString().Trim() + ", ";
                }
            }
            if (sw.ThursdayHours != null)
            {
                foreach (var item in sw.ThursdayHours)
                {
                    if ((item.EndHour == "00.00") || (item.EndHour == "0.00") || (item.EndHour == "0.0"))
                        item.EndHour = "24.00";
                    weekday.Thursday += item.StartHour.ToString().Trim() + "-" + item.EndHour.ToString().Trim() + ", ";
                }
            }
            if (sw.FridayHours != null)
            {
                foreach (var item in sw.FridayHours)
                {
                    if ((item.EndHour == "00.00") || (item.EndHour == "0.00") || (item.EndHour == "0.0"))
                        item.EndHour = "24.00";
                    weekday.Friday += item.StartHour.ToString().Trim() + "-" + item.EndHour.ToString().Trim() + ", ";
                }
            }

            //foreach (var item in hoursdic)
            //{
            //int[] hinbits = new int[48];
            //foreach (var hours in item.Value)
            //{
            //    if (hours != null)
            //    {
            //        int s_h = ConvertToNumOfBits(Double.Parse(hours.StartHour));
            //        int e_h = ConvertToNumOfBits(Double.Parse(hours.EndHour));
            //        for (int j = s_h; j < e_h; j++)
            //        {
            //            hinbits[j] = 1;
            //        }
            //    }

            //}
            //string trystr = string.Join("", hinbits);
            //long encryptedhours = Convert.ToInt64(trystr, 2);

            //switch (item.Key)
            //{

            //    case (0):
            //        weekday.Sunday1HourQuaters = encryptedhours;
            //        break;
            //    case (1):
            //        weekday.Sunday2HourQuaters = encryptedhours;
            //        break;
            //    case (2):
            //        weekday.Monday1HourQuaters = encryptedhours;
            //        break;
            //    case (3):
            //        weekday.Monday2HourQuaters = encryptedhours;
            //        break;
            //    case (4):
            //        weekday.Tueday1HourQuaters = encryptedhours;
            //        break;
            //    case (5):
            //        weekday.Tueday2HourQuaters = encryptedhours;
            //        break;
            //    case (6):
            //        weekday.Wednesday1HourQuaters = encryptedhours;
            //        break;
            //    case (7):
            //        weekday.Wednesday2HourQuaters = encryptedhours;
            //        break;
            //    case (8):
            //        weekday.Thursday1HourQuaters = encryptedhours;
            //        break;
            //    case (9):
            //        weekday.Thursday2HourQuaters = encryptedhours;
            //        break;
            //    case (10):
            //        weekday.Friday1HourQuaters = encryptedhours;
            //        break;
            //    case (11):
            //        weekday.Friday2HourQuaters = encryptedhours;
            //        break;
            //}
            //}

            return weekday;
        }
        public Schedule_Week GetSchedule_WeekFromHoursList(int code, Dictionary<int, List<Hours>> hdic)
        {
            Schedule_Week sw = new Schedule_Week();
            Hours midlleh = new Hours();
            foreach (var item in hdic[0])
            {
                if ((Double.Parse(item.StartHour) >= 00.00) && (Double.Parse(item.EndHour) < 12.00))
                {
                    sw.SundayHours.Add(item);
                }
                if ((Double.Parse(item.StartHour) >= 00.00) && (Double.Parse(item.EndHour) == 12.00))
                {
                    midlleh.StartHour = item.StartHour;
                }
            }
            foreach (var item in hdic[1])
            {
                if ((Double.Parse(item.StartHour) > 12.00) && (Double.Parse(item.EndHour) <= 00.00))
                {
                    sw.SundayHours.Add(item);
                }
                if ((Double.Parse(item.StartHour) == 12.00) && (Double.Parse(item.EndHour) <= 00.00))
                {
                    midlleh.EndHour = item.EndHour;
                }
            }
            sw.SundayHours.Add(midlleh);
            foreach (var item in hdic[2])
            {
                if ((Double.Parse(item.StartHour) >= 00.00) && (Double.Parse(item.EndHour) < 12.00))
                {
                    sw.MondayHours.Add(item);
                }
                if ((Double.Parse(item.StartHour) >= 00.00) && (Double.Parse(item.EndHour) == 12.00))
                {
                    midlleh.StartHour = item.StartHour;
                }
            }
            foreach (var item in hdic[3])
            {
                if ((Double.Parse(item.StartHour) > 12.00) && (Double.Parse(item.EndHour) <= 00.00))
                {
                    sw.MondayHours.Add(item);
                }
                if ((Double.Parse(item.StartHour) == 12.00) && (Double.Parse(item.EndHour) <= 00.00))
                {
                    midlleh.EndHour = item.EndHour;
                }
            }
            sw.MondayHours.Add(midlleh);
            foreach (var item in hdic[4])
            {
                if ((Double.Parse(item.StartHour) >= 00.00) && (Double.Parse(item.EndHour) < 12.00))
                {
                    sw.TuesdayHours.Add(item);
                }
                if ((Double.Parse(item.StartHour) >= 00.00) && (Double.Parse(item.EndHour) == 12.00))
                {
                    midlleh.StartHour = item.StartHour;
                }
            }
            foreach (var item in hdic[5])
            {
                if ((Double.Parse(item.StartHour) > 12.00) && (Double.Parse(item.EndHour) <= 00.00))
                {
                    sw.TuesdayHours.Add(item);
                }
                if ((Double.Parse(item.StartHour) == 12.00) && (Double.Parse(item.EndHour) <= 00.00))
                {
                    midlleh.EndHour = item.EndHour;
                }
            }
            sw.TuesdayHours.Add(midlleh);
            foreach (var item in hdic[6])
            {
                if ((Double.Parse(item.StartHour) >= 00.00) && (Double.Parse(item.EndHour) < 12.00))
                {
                    sw.WednesdayHours.Add(item);
                }
                if ((Double.Parse(item.StartHour) >= 00.00) && (Double.Parse(item.EndHour) == 12.00))
                {
                    midlleh.StartHour = item.StartHour;
                }
            }
            foreach (var item in hdic[7])
            {
                if ((Double.Parse(item.StartHour) > 12.00) && (Double.Parse(item.EndHour) <= 00.00))
                {
                    sw.WednesdayHours.Add(item);
                }
                if ((Double.Parse(item.StartHour) == 12.00) && (Double.Parse(item.EndHour) <= 00.00))
                {
                    midlleh.EndHour = item.EndHour;
                }
            }
            sw.WednesdayHours.Add(midlleh);
            foreach (var item in hdic[8])
            {
                if ((Double.Parse(item.StartHour) >= 00.00) && (Double.Parse(item.EndHour) < 12.00))
                {
                    sw.ThursdayHours.Add(item);
                }
                if ((Double.Parse(item.StartHour) >= 00.00) && (Double.Parse(item.EndHour) == 12.00))
                {
                    midlleh.StartHour = item.StartHour;
                }
            }
            foreach (var item in hdic[9])
            {
                if ((Double.Parse(item.StartHour) > 12.00) && (Double.Parse(item.EndHour) <= 00.00))
                {
                    sw.ThursdayHours.Add(item);
                }
                if ((Double.Parse(item.StartHour) == 12.00) && (Double.Parse(item.EndHour) <= 00.00))
                {
                    midlleh.EndHour = item.EndHour;
                }
            }
            sw.ThursdayHours.Add(midlleh);
            foreach (var item in hdic[10])
            {
                if ((Double.Parse(item.StartHour) >= 00.00) && (Double.Parse(item.EndHour) < 12.00))
                {
                    sw.FridayHours.Add(item);
                }
                if ((Double.Parse(item.StartHour) >= 00.00) && (Double.Parse(item.EndHour) == 12.00))
                {
                    midlleh.StartHour = item.StartHour;
                }
            }
            foreach (var item in hdic[11])
            {
                if ((Double.Parse(item.StartHour) > 12.00) && (Double.Parse(item.EndHour) <= 00.00))
                {
                    sw.FridayHours.Add(item);
                }
                if ((Double.Parse(item.StartHour) == 12.00) && (Double.Parse(item.EndHour) <= 00.00))
                {
                    midlleh.EndHour = item.EndHour;
                }
            }
            sw.FridayHours.Add(midlleh);
            sw.Code = code;
            return sw;
        }

        //    public Dictionary<int, List<Hours>> GetHoursListFromScheduleWeek(Schedule_Week sw)
        //{
        //    Dictionary<int, List<Hours>> hdic = new Dictionary<int, List<Hours>>();
        //    List<Hours> fhours = new List<Hours>();
        //    List<Hours> shours = new List<Hours>();

        //    if ((sw.SundayHours!=null)&&(sw.SundayHours.Count!=0))
        //    {
        //        List<Hours> ifhours = new List<Hours>();
        //        List<Hours> ishours = new List<Hours>();
        //        foreach (var item in sw.SundayHours)
        //    {
        //            if (((Double.Parse(item.StartHour) >= 00.00) && (Double.Parse(item.EndHour) <= 12.00)))
        //            {
        //                fhours.Add(item);
        //            }
        //            if(((Double.Parse(item.StartHour) >= 12.00) && ((Double.Parse(item.EndHour) < 24.00) || (Double.Parse(item.EndHour) == 00.00))))
        //            {
        //                shours.Add(new Hours((Double.Parse(item.StartHour) - 12).ToString(), (Double.Parse(item.EndHour) - 12).ToString()));
        //            }
        //            if (((Double.Parse(item.StartHour) >= 00.00) && ((Double.Parse(item.StartHour) < 12.00))) && (Double.Parse(item.EndHour) > 12.00))
        //            {

        //                fhours.Add(new Hours(item.StartHour, "12.00"));
        //                shours.Add(new Hours("00.00", (Double.Parse(item.EndHour) - 12).ToString()));
        //            }
        //        }
        //        fhours.ForEach(r => ifhours.Add(r));
        //        shours.ForEach(r => ishours.Add(r));
        //        hdic.Add(key: 0, value:ifhours);
        //    hdic.Add(key: 1, value:ishours);
        //    fhours.Clear();
        //    shours.Clear();

        //    }
        //    if((sw.MondayHours!=null) && (sw.MondayHours.Count != 0))
        //    {
        //        List<Hours> ifhours = new List<Hours>();
        //        List<Hours> ishours = new List<Hours>();
        //        foreach (var item in sw.MondayHours)
        //        {
        //            if (((Double.Parse(item.StartHour) >= 00.00) && (Double.Parse(item.EndHour) <= 12.00)))
        //            {
        //                fhours.Add(item);
        //            }
        //            if (((Double.Parse(item.StartHour) >= 12.00) && ((Double.Parse(item.EndHour) < 24.00) || (Double.Parse(item.EndHour) == 00.00))))
        //            {
        //                shours.Add(new Hours((Double.Parse(item.StartHour) - 12).ToString(), (Double.Parse(item.EndHour) - 12).ToString()));
        //            }
        //            if (((Double.Parse(item.StartHour) >= 00.00) && ((Double.Parse(item.StartHour) < 12.00))) && (Double.Parse(item.EndHour) > 12.00))
        //            {

        //                fhours.Add(new Hours(item.StartHour, "12.00"));
        //                shours.Add(new Hours("00.00", (Double.Parse(item.EndHour) - 12).ToString()));
        //            }
        //        }
        //        fhours.ForEach(r => ifhours.Add(r));
        //        shours.ForEach(r => ishours.Add(r));
        //        hdic.Add(key: 2, value: ifhours);
        //        hdic.Add(key: 3, value: ishours);
        //        fhours.Clear();
        //        shours.Clear();
        //    }
        //    if((sw.TuedayHours!=null) && (sw.TuedayHours.Count != 0))
        //    {
        //        List<Hours> ifhours = new List<Hours>();
        //        List<Hours> ishours = new List<Hours>();
        //        foreach (var item in sw.TuedayHours)
        //    {
        //            if (((Double.Parse(item.StartHour) >= 00.00) && (Double.Parse(item.EndHour) <= 12.00)))
        //            {
        //                fhours.Add(item);
        //            }
        //            if (((Double.Parse(item.StartHour) >= 12.00) && ((Double.Parse(item.EndHour) < 24.00) || (Double.Parse(item.EndHour) == 00.00))))
        //            {
        //                shours.Add(new Hours((Double.Parse(item.StartHour) - 12).ToString(), (Double.Parse(item.EndHour) - 12).ToString()));
        //            }
        //            if (((Double.Parse(item.StartHour) >= 00.00) && ((Double.Parse(item.StartHour) < 12.00))) && (Double.Parse(item.EndHour) > 12.00))
        //            {

        //                fhours.Add(new Hours(item.StartHour, "12.00"));
        //                shours.Add(new Hours("00.00", (Double.Parse(item.EndHour) - 12).ToString()));
        //            }
        //        }
        //        fhours.ForEach(r => ifhours.Add(r));
        //        shours.ForEach(r => ishours.Add(r));
        //        hdic.Add(key: 4, value: ifhours);
        //        hdic.Add(key: 5, value: ishours);
        //        fhours.Clear();
        //        shours.Clear();
        //    }
        //    if((sw.WednesdayHours!=null) && (sw.WednesdayHours.Count != 0))
        //    {
        //        List<Hours> ifhours = new List<Hours>();
        //        List<Hours> ishours = new List<Hours>();
        //        foreach (var item in sw.WednesdayHours)
        //    {
        //            if (((Double.Parse(item.StartHour) >= 00.00) && (Double.Parse(item.EndHour) <= 12.00)))
        //            {
        //               fhours.Add(item);
        //            }
        //            if (((Double.Parse(item.StartHour) >= 12.00) && ((Double.Parse(item.EndHour) < 24.00) || (Double.Parse(item.EndHour) == 00.00))))
        //            {
        //                shours.Add(new Hours((Double.Parse(item.StartHour) - 12).ToString(), (Double.Parse(item.EndHour) - 12).ToString()));
        //            }
        //            if (((Double.Parse(item.StartHour) >= 00.00) && ((Double.Parse(item.StartHour) < 12.00))) && (Double.Parse(item.EndHour) > 12.00))
        //            {

        //                fhours.Add(new Hours(item.StartHour, "12.00"));
        //                shours.Add(new Hours("00.00", (Double.Parse(item.EndHour) - 12).ToString()));
        //            }
        //        }
        //        fhours.ForEach(r => ifhours.Add(r));
        //        shours.ForEach(r => ishours.Add(r));
        //        hdic.Add(key: 6, value: ifhours);
        //        hdic.Add(key: 7, value: ishours);
        //        fhours.Clear();
        //        shours.Clear();
        //    }
        //    if((sw.ThursdayHours!=null) && (sw.ThursdayHours.Count != 0))
        //    {
        //        List<Hours> ifhours = new List<Hours>();
        //        List<Hours> ishours = new List<Hours>();
        //        foreach (var item in sw.ThursdayHours)
        //    {
        //            if (((Double.Parse(item.StartHour) >= 00.00) && (Double.Parse(item.EndHour) <= 12.00)))
        //            {
        //                fhours.Add(item);
        //            }
        //            if (((Double.Parse(item.StartHour) >= 12.00) && ((Double.Parse(item.EndHour) < 24.00) || (Double.Parse(item.EndHour) == 00.00))))
        //            {
        //                shours.Add(new Hours((Double.Parse(item.StartHour) - 12).ToString(), (Double.Parse(item.EndHour) - 12).ToString()));
        //            }
        //            if (((Double.Parse(item.StartHour) >= 00.00) && ((Double.Parse(item.StartHour) < 12.00))) && (Double.Parse(item.EndHour) > 12.00))
        //            {

        //                fhours.Add(new Hours(item.StartHour, "12.00"));
        //                shours.Add(new Hours("00.00", (Double.Parse(item.EndHour) - 12).ToString()));
        //            }
        //        }
        //        fhours.ForEach(r => ifhours.Add(r));
        //        shours.ForEach(r => ishours.Add(r));
        //        hdic.Add(key: 8, value: ifhours);
        //        hdic.Add(key: 9, value: ishours);
        //        fhours.Clear();
        //        shours.Clear();
        //    }
        //    if((sw.FridayHours!=null) && (sw.FridayHours.Count != 0))
        //    {
        //        List<Hours> ifhours = new List<Hours>();
        //        List<Hours> ishours = new List<Hours>();
        //        foreach (var item in sw.FridayHours)
        //    {
        //            if (((Double.Parse(item.StartHour) >= 00.00) && (Double.Parse(item.EndHour) <= 12.00)))
        //            {
        //                fhours.Add(item);
        //            }
        //            if (((Double.Parse(item.StartHour) >= 12.00) && ((Double.Parse(item.EndHour) < 24.00) || (Double.Parse(item.EndHour) == 00.00))))
        //            {
        //                shours.Add(new Hours((Double.Parse(item.StartHour) - 12).ToString(), (Double.Parse(item.EndHour) - 12).ToString()));
        //            }
        //            if (((Double.Parse(item.StartHour) >= 00.00) && ((Double.Parse(item.StartHour) < 12.00))) && (Double.Parse(item.EndHour) > 12.00))
        //            {

        //                fhours.Add(new Hours(item.StartHour, "12.00"));
        //                shours.Add(new Hours("00.00", (Double.Parse(item.EndHour) - 12).ToString()));
        //            }
        //        }
        //        fhours.ForEach(r => ifhours.Add(r));
        //        shours.ForEach(r => ishours.Add(r));
        //        hdic.Add(key: 10, value: ifhours);
        //        hdic.Add(key: 11, value: ishours);
        //        fhours.Clear();
        //        shours.Clear();
        //    }

        //    return hdic;
        //}

        #endregion

    }
}
