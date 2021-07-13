using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace BL
{
   public class CityBL : DbHandler
    {
        List<CityDTO> ctlst = DAL.Convert.CityConvert.ConvertCityToEntity(GetAll<DAL.City>());
        public CityBL()
        {
        }
        // adding a new city to cities table. returns 1 if succeeds
        public int AddCity(CityDTO city)
        {

            AddSet(DAL.Convert.CityConvert.ConvertCityToEF(city));
            return 1;
        }
        public List<CityDTO> GetAllCities()
        {
            return ctlst;
        }



    }
}
