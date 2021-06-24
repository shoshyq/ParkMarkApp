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
                Sunday1HourQuaters = w.Sunday1HourQuaters,
                Sunday2HourQuaters = w.Sunday1HourQuaters,
                Monday1HourQuaters = w.Monday1HourQuaters,
                Monday2HourQuaters = w.Monday2HourQuaters,
                Tueday1HourQuaters = w.Tueday1HourQuaters,
                Tueday2HourQuaters = w.Tueday2HourQuaters,
                Wednesday1HourQuaters = w.Wednesday1HourQuaters,
                Wednesday2HourQuaters = w.Wednesday2HourQuaters,
                Thursday1HourQuaters = w.Thursday1HourQuaters,
                Thursday2HourQuaters = w.Thursday2HourQuaters,
                Friday1HourQuaters = w.Friday1HourQuaters,
                Friday2HourQuaters = w.Friday2HourQuaters
            };
        }
        public static Entities.WeekDay ConvertWeekDayToEntity(DAL.WeekDay w)
        {
            return new Entities.WeekDay
            {
                Code = w.Code,
                Sunday1HourQuaters = w.Sunday1HourQuaters,
                Sunday2HourQuaters = w.Sunday1HourQuaters,
                Monday1HourQuaters = w.Monday1HourQuaters,
                Monday2HourQuaters = w.Monday2HourQuaters,
                Tueday1HourQuaters = w.Tueday1HourQuaters,
                Tueday2HourQuaters = w.Tueday2HourQuaters,
                Wednesday1HourQuaters = w.Wednesday1HourQuaters,
                Wednesday2HourQuaters = w.Wednesday2HourQuaters,
                Thursday1HourQuaters = w.Thursday1HourQuaters,
                Thursday2HourQuaters = w.Thursday2HourQuaters,
                Friday1HourQuaters = w.Friday1HourQuaters,
                Friday2HourQuaters = w.Friday2HourQuaters
            };
        }
        public static List<Entities.WeekDay> ConvertWeekDaysListToEntity(IEnumerable<DAL.WeekDay> weekdays)
        {
            return weekdays.Select(p => ConvertWeekDayToEntity(p)).OrderBy(n => n.Code).ToList();
        }

    }
}
