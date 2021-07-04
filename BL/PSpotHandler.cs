using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class PSpotHandler
    {
        public int Code;
        public int UserCode;
        public double? SpotWidth;

        public double? SpotLength;

        public double? PricePerHour;
        public string Place_Id;
        public string Address;
        public bool? HasRoof;
        public Dictionary<int, List<Hours>> Hours;
        public DateTime SearchDate { get; set; }
    }
}
