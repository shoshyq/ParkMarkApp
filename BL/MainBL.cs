using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;


namespace BL
{
    public class MainBL
    {
        HungarianFunctions hf;
        DistanceFunc df;
        public MainBL()
        {
            df = new DistanceFunc();
            hf = new HungarianFunctions();
        }

        //once a day main-hungarian function - suppose to be here 
        public Dictionary<int, Dictionary<int, int>> ParkingSpotPerUser()
        {
            return hf.PSpotsAllSearchesByCities();
        }
        // confirmation of the result from user about the parking spot. updating the schedule-times table 
        public int ConfirmResult(Entities.ParkingSpotSearch psr, int usercode, Entities.ParkingSpot ps)
        {
            return 1;
        }


    }

}
