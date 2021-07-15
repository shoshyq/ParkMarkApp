using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Schedule_Week
    {
        public int Code { get; set; }
        public List<Hours> SundayHours { get; set; }
        public List<Hours> MondayHours { get; set; }
        public List<Hours> TuesdayHours { get; set; }
        public List<Hours> WednesdayHours { get; set; }
        public List<Hours> ThursdayHours { get; set; }
        public List<Hours> FridayHours { get; set; }
    }
}
