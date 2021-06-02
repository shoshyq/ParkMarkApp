using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Entities;
using ParkingSpot = DAL.ParkingSpot;

namespace BL
{
    public  class ParkingSpotsBL
    {
    DBConnection DBCon;
        DistanceFunc df;
        public ParkingSpotsBL()
        {
            DBCon = new DBConnection();
            df = new DistanceFunc();
        }
        // registering new parking spot . returns 1 if succeeds
        public int RegisterUsersParkSpot(Entities.ParkingSpot mp)
        {
            if (!DbHandler.GetAll<ParkingSpot>().Any(d => d.Place_id.Trim() == mp.Place_id.Trim()))
            {
                //if (!DbHandler.GetAll<ParkingLocation>().Any(d => d.Place_Id.Trim() == mp.Place_id.Trim()))
                //{
                mp.Place_id = df.GetPlaceId(mp.FullAddress);
                DbHandler.AddSet(mp);

                //DbHandler.AddSet<ParkingLocation>(new ParkingLocation
                //{
                //    ParkingCode = mp.Code,
                //    Place_Id = mp.Place_id,
                //    FullAddress = mp.FullAddress
                //    //});
                //}
                return DbHandler.GetAll<ParkingSpot>().First(w => w.Code == mp.Code).Code;
            }
            return 0;

        }
        // updating a parking spot . returns code if succeeds
        public int UpdateUsersParkSpot(Entities.ParkingSpot mp)
        {
            mp.Place_id = df.GetPlaceId(mp.FullAddress);

            DBCon.Execute(mp, DBConnection.ExecuteActions.Update);
            return mp.Code;
        }
        public int DeleteParkingSpotByUser(DAL.User u)
        {
            var pslist = DbHandler.GetAll<ParkingSpot>();
            if (pslist != null)
            {
                foreach (var item in pslist)
                {
                    if (item.UserCode == u.Code)
                    {

                        DbHandler.DeleteSet(item);

                    }
                    else
                        return 0;
                }
            }
            return 1;
        }
        public int DeleteParkingSpot(ParkingSpot p)
        {
            var l = DbHandler.GetAll<ParkingSpot>().First(i => i.Code == p.Code);
            DbHandler.DeleteSet(l);

            return 1;
        }
    }
}
