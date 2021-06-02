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

        public double StartHour { get; }
        public double EndHour { get; }

    }

}
