using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class WeekDay
    {
        public int Code { get; set; }
        public Nullable<int> SundayHourQuaters { get; set; }
        public Nullable<int> MondayHourQuaters { get; set; }
        public Nullable<int> TuedayHourQuaters { get; set; }
        public Nullable<int> WednesdayHourQuaters { get; set; }
        public Nullable<int> ThursdayHourQuaters { get; set; }
        public Nullable<int> FridayHourQuaters { get; set; }
    }
}
