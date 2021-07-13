using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using WeekDay = DAL.WeekDay;

namespace BL
{
   public  class WeekDayBL :DbHandler
    {
        ConvertFuncBL convertFunc;
        //public static List<string> weekdays = new List<string>() { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };

        public WeekDayBL()
        {
            convertFunc = new ConvertFuncBL();
        }
        // adding a new week to week table. gets a Schedule_Week/// and returns the code if succeeds, if not - 0
        public int AddWeekDays(Schedule_Week sw)
        {
            Entities.WeekDay wd = convertFunc.GetWeekDayHours(sw);
            AddSet(DAL.Convert.WeekDayConvert.ConvertWeekDayToEF(wd));
            return (DAL.Convert.WeekDayConvert.ConvertWeekDaysListToEntity(GetAll<WeekDay>())).LastOrDefault().Code;
        }

        public Schedule_Week GetSchedule(int wcode)
        {
            Entities.WeekDay weekday = DAL.Convert.WeekDayConvert.ConvertWeekDaysListToEntity(GetAll<WeekDay>()).First(w => w.Code == wcode);
            return convertFunc.WeekDayTblToSchedWeek(weekday);
        }

        // update weekday function. returns a schedule week if succeeds.
        public Schedule_Week UpdateWeekDay(Schedule_Week sw)
        {
            Entities.WeekDay wd = convertFunc.GetWeekDayHours(sw);
            UpdateSet(DAL.Convert.WeekDayConvert.ConvertWeekDayToEF(wd));
            return (DAL.Convert.WeekDayConvert.ConvertWeekDaysListToEntity(GetAll<WeekDay>())).Any(w => w.Code == wd.Code) ? convertFunc.WeekDayTblToSchedWeek(DAL.Convert.WeekDayConvert.ConvertWeekDaysListToEntity(GetAll<WeekDay>()).First(w => w.Code == wd.Code)) : null;
        }
        //DAL.Converts.WeekDayConvert.ConvertWeekDaysListToEntity
    }
}
