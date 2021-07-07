//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class ParkingSpot
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
    
        public virtual WeekDay WeekDay { get; set; }
        public virtual User User { get; set; }
        public virtual City City { get; set; }
    }
}
