using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class ParkingSpot
    {
        public int Code { get; set; }
        public int UserCode { get; set; }
        public Nullable<int> CityCode { get; set; }
        public string Place_id { get; set; }
        public string FullAddress { get; set; }
        public Nullable<double> SpotWidth { get; set; }
        public Nullable<double> SpotLength { get; set; }
        public Nullable<double> PricePerHour { get; set; }
        public Nullable<bool> HasRoof { get; set; }
        public Nullable<int> DaysSchedule { get; set; }
        public Nullable<bool> IsOccupied { get; set; }
        public Nullable<bool> AvRegularly { get; set; }

    }
}
