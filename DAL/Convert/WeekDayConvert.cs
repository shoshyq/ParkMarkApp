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
                Sunday = w.Sunday,
                Monday = w.Monday,
                Tuesday = w.Tuesday,
                Wednesday = w.Wednesday,
                Thursday = w.Thursday,
                Friday = w.Friday
            };
        }
        public static Entities.WeekDay ConvertWeekDayToEntity(DAL.WeekDay w)
        {
            return new Entities.WeekDay
            {
                Code = w.Code,
                Sunday = w.Sunday,
                Monday = w.Monday,
                Tuesday = w.Tuesday,
                Wednesday = w.Wednesday,
                Thursday = w.Thursday,
                Friday = w.Friday
        
            };
        }
        public static List<Entities.WeekDay> ConvertWeekDaysListToEntity(IEnumerable<DAL.WeekDay> weekdays)
        {
            return weekdays.Select(p => ConvertWeekDayToEntity(p)).OrderBy(n => n.Code).ToList();
        }

    }
}
