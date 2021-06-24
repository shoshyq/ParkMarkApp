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
        public Nullable<long> Sunday1HourQuaters { get; set; }
        public Nullable<long> Sunday2HourQuaters { get; set; }
        public Nullable<long> Monday1HourQuaters { get; set; }
        public Nullable<long> Monday2HourQuaters { get; set; }
        public Nullable<long> Tueday1HourQuaters { get; set; }
        public Nullable<long> Tueday2HourQuaters { get; set; }
        public Nullable<long> Wednesday1HourQuaters { get; set; }
        public Nullable<long> Wednesday2HourQuaters { get; set; }
        public Nullable<long> Thursday1HourQuaters { get; set; }
        public Nullable<long> Thursday2HourQuaters { get; set; }
        public Nullable<long> Friday1HourQuaters { get; set; }
        public Nullable<long> Friday2HourQuaters { get; set; }
    }
}
