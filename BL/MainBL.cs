using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Entities;
using City = DAL.City;
using Feedback = DAL.Feedback;
using ParkingSpot = DAL.ParkingSpot;
using ParkingSpotSearch = DAL.ParkingSpotSearch;
using PaymentDetail = DAL.PaymentDetail;
using User = DAL.User;
using WeekDay = DAL.WeekDay;

namespace BL
{
    public class MainBL
    {
        HungarianFunctions hf;
        DBConnection DBCon;
        DistanceFunc df;
        public MainBL()
        {
            DBCon = new DBConnection();
            df = new DistanceFunc();
            hf = new HungarianFunctions();
        }

        //once a day main-hungarian function - suppose to be here 
        public Dictionary<int, Dictionary<int, int>> ParkingSpotPerUser()
        {
            return hf.PSpotsAllSearchesByCities();
        }
        // confirmation of the result from user about the parking spot. updating the schedule-times table 
        public int ConfirmResult()
        {
            return 1;
        }
       

    }

}
