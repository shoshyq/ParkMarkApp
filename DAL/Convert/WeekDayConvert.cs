using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Convert
{
    public static class WeekDayConvert
    {
        public static DAL.WeekDay ConvertWeekDayToEF(Entities.WeekDay w)
        {
            return new DAL.WeekDay
            {
                Code = w.Code,
                SundayHourQuaters = w.SundayHourQuaters,
                MondayHourQuaters = w.MondayHourQuaters,
                TuedayHourQuaters = w.TuedayHourQuaters,
                WednesdayHourQuaters = w.WednesdayHourQuaters,
                ThursdayHourQuaters = w.ThursdayHourQuaters,
                FridayHourQuaters = w.FridayHourQuaters
            };
        }
        public static Entities.WeekDay ConvertWeekDayToEntity(DAL.WeekDay w)
        {
            return new Entities.WeekDay
            {
                Code = w.Code,
                SundayHourQuaters = w.SundayHourQuaters,
                MondayHourQuaters = w.MondayHourQuaters,
                TuedayHourQuaters = w.TuedayHourQuaters,
                WednesdayHourQuaters = w.WednesdayHourQuaters,
                ThursdayHourQuaters = w.ThursdayHourQuaters,
                FridayHourQuaters = w.FridayHourQuaters
            };
        }
        public static List<Entities.WeekDay> ConvertWeekDaysListToEntity(IEnumerable<DAL.WeekDay> weekdays)
        {
            return weekdays.Select(p => ConvertWeekDayToEntity(p)).OrderBy(n => n.Code).ToList();
        }

    }
}
