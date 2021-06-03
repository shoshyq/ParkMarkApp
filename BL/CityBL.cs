using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Entities;
using City = DAL.City;

namespace BL
{
   public class CityBL : DbHandler
    {

        public CityBL()
        {
        }
        // adding a new city to cities table. returns 1 if succeeds
        public int AddCity(string cityname)
        {

            AddSet(new City { CityName = cityname });
            return 1;
        }


    }
}
