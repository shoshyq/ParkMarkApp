using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class ParkingSpotSearch
    {
        public int Code { get; set; }
        public Nullable<int> UserId { get; set; }
        public string MyLocationAddress { get; set; }
        public string Place_id { get; set; }
        public Nullable<int> CityCode { get; set; }
        public Nullable<bool> SizeOpt { get; set; }
        public Nullable<double> PreferableWidth { get; set; }
        public Nullable<double> PreferableLength { get; set; }
        public Nullable<bool> RoofOpt { get; set; }
        public Nullable<int> DaysSchedule { get; set; }
        public Nullable<double> MinPrice { get; set; }
        public Nullable<double> MaxPrice { get; set; }
        public Nullable<bool> Regularly { get; set; }
        public Nullable<System.DateTime> SearchDate { get; set; }
    }
}
