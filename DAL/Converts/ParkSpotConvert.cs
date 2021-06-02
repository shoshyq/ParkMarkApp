using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Converts
{
    public static class ParkSpotConvert
    {
        public static DAL.ParkingSpot ConvertParkingSpotToEF(Entities.ParkingSpot ps)
        {
            return new DAL.ParkingSpot
            {
                Code = ps.Code,
                UserCode = ps.UserCode,
                CityCode = ps.CityCode,
                Place_id = ps.Place_id,
                FullAddress = ps.FullAddress,
                SpotWidth = ps.SpotWidth,
                SpotLength = ps.SpotLength,
                PricePerHour = ps.PricePerHour,
                HasRoof = ps.HasRoof,
                DaysSchedule = ps.DaysSchedule,
                IsOccupied = ps.IsOccupied,
                AvRegularly = ps.AvRegularly

            };
        }
        public static Entities.ParkingSpot ConvertParkingSpotToEntity(DAL.ParkingSpot ps)
        {
            return new Entities.ParkingSpot
            {

                Code = ps.Code,
                UserCode = ps.UserCode,
                CityCode = ps.CityCode,
                Place_id = ps.Place_id,
                FullAddress = ps.FullAddress,
                SpotWidth = ps.SpotWidth,
                SpotLength = ps.SpotLength,
                PricePerHour = ps.PricePerHour,
                HasRoof = ps.HasRoof,
                DaysSchedule = ps.DaysSchedule,
                IsOccupied = ps.IsOccupied,
                AvRegularly = ps.AvRegularly
            };
        }
        public static List<Entities.ParkingSpot> ConvertParkingSpotsListToEntity(IEnumerable<DAL.ParkingSpot> pspots)
        {
            return pspots.Select(p => ConvertParkingSpotToEntity(p)).OrderBy(n => n.Code).ToList();
        }


    }
}
