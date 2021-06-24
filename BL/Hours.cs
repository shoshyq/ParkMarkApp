using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Hours
    {
        public Hours()
        {
        }

        public Hours(double v1, double v2)
        {
            StartHour = v1;
            EndHour = v2;
        }
        public void SetStartHour(double sh)
        {
            this.StartHour = sh;
        }
        public double StartHour { get; set; }
        public double EndHour { get; set; }

    }

}
