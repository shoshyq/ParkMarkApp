using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Converts
{
    public static class SearchConvert
    {
        public static DAL.ParkingSpotSearch ConvertSearchToEF(Entities.ParkingSpotSearch pss)
        {
            return new DAL.ParkingSpotSearch
            {
                Code = pss.Code,
                UserId = pss.UserId,
                MyLocationAddress = pss.MyLocationAddress,
                Place_id = pss.Place_id,
                CityCode = pss.CityCode,
                SizeOpt = pss.SizeOpt,
                PreferableWidth = pss.PreferableWidth,
                PreferableLength = pss.PreferableLength,
                RoofOpt = pss.RoofOpt,
                DaysSchedule = pss.DaysSchedule,
                MinPrice = pss.MinPrice,
                MaxPrice = pss.MaxPrice,
                Regularly = pss.Regularly,
                SearchDate = pss.SearchDate
            };
        }
        public static Entities.ParkingSpotSearch ConvertSearchToEntity(DAL.ParkingSpotSearch pss)
        {
            return new Entities.ParkingSpotSearch
            {
                Code = pss.Code,
                UserId = pss.UserId,
                MyLocationAddress = pss.MyLocationAddress,
                Place_id = pss.Place_id,
                CityCode = pss.CityCode,
                SizeOpt = pss.SizeOpt,
                PreferableWidth = pss.PreferableWidth,
                PreferableLength = pss.PreferableLength,
                RoofOpt = pss.RoofOpt,
                DaysSchedule = pss.DaysSchedule,
                MinPrice = pss.MinPrice,
                MaxPrice = pss.MaxPrice,
                Regularly = pss.Regularly,
                SearchDate = pss.SearchDate
            };
        }



        public static List<Entities.ParkingSpotSearch> ConvertSearchesListToEntity(IEnumerable<DAL.ParkingSpotSearch> searches)
        {
            return searches.Select(p => ConvertSearchToEntity(p)).OrderBy(n => n.Code).ToList();
        }

    }
}
