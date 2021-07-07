using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Convert
{
    public static class CityConvert
    {
        public static DAL.City ConvertCityToEF(Entities.CityDTO c)
        {
            return new DAL.City
            {
                Code = c.Code,
                CityName = c.CityName
            };
        }
        public static Entities.CityDTO ConvertCityToEntity(DAL.City c)
        {
            return new Entities.CityDTO
            {
                Code = c.Code,
                CityName = c.CityName
            };
        }

        public static List<Entities.CityDTO> ConvertCityToEntity(IEnumerable<DAL.City> cities)
        {
            return cities.Select(p => ConvertCityToEntity(p)).OrderBy(n => n.Code).ToList();
        }

    }
}
