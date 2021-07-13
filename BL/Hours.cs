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

        public Hours(string v1, string v2)
        {
            StartHour = v1;
            EndHour = v2;
        }
        public void SetStartHour(double sh)
        {
            this.StartHour = sh.ToString();
        }
        public string StartHour { get; set; }
        public string EndHour { get; set; }

    }

}
