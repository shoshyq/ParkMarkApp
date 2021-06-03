﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Entities;
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
            Entities.WeekDay wd = convertFunc.SchedWeekToWeekDayTbl(sw);
            AddSet(DAL.Converts.WeekDayConvert.ConvertWeekDayToEF(wd));
            return DAL.Converts.WeekDayConvert.ConvertWeekDaysListToEntity(GetAll<WeekDay>()).Any(w => w.Code == wd.Code) ? DbHandler.GetAll<WeekDay>().First(w => w.Code == wd.Code).Code : 0;
        }
        // update weekday function. returns a schedule week if succeeds.
        public Schedule_Week UpdateWeekDay(Schedule_Week sw)
        {
            Entities.WeekDay wd = convertFunc.SchedWeekToWeekDayTbl(sw);
            UpdateSet(DAL.Converts.WeekDayConvert.ConvertWeekDayToEF(wd));
            return DAL.Converts.WeekDayConvert.ConvertWeekDaysListToEntity(GetAll<WeekDay>()).Any(w => w.Code == wd.Code) ? convertFunc.WeekDayTblToSchedWeek(DAL.Converts.WeekDayConvert.ConvertWeekDaysListToEntity(GetAll<WeekDay>()).First(w => w.Code == wd.Code)) : null;
        }
    }
}
