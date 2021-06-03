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
    public  class ParkingSpotsBL : DbHandler
    {
        DistanceFunc df;
        List<Entities.ParkingSpot> pslst = DAL.Converts.ParkSpotConvert.ConvertParkingSpotsListToEntity(GetAll<ParkingSpot>());
        public ParkingSpotsBL()
        {
            df = new DistanceFunc();
        }
        // registering new parking spot . returns 1 if succeeds
        public int RegisterUsersParkSpot(Entities.ParkingSpot mp)
        {
            if (!pslst.Any(d => d.Place_id.Trim() == mp.Place_id.Trim()))
            {
                //if (!DbHandler.GetAll<ParkingLocation>().Any(d => d.Place_Id.Trim() == mp.Place_id.Trim()))
                //{
                mp.Place_id = df.GetPlaceId(mp.FullAddress);
               AddSet(DAL.Converts.ParkSpotConvert.ConvertParkingSpotToEF(mp));

                //DbHandler.AddSet<ParkingLocation>(new ParkingLocation
                //{
                //    ParkingCode = mp.Code,
                //    Place_Id = mp.Place_id,
                //    FullAddress = mp.FullAddress
                //    //});
                //}
                return pslst.First(w => w.Code == mp.Code).Code;
            }
            return 0;

        }
        // updating a parking spot . returns code if succeeds
        public int UpdateUsersParkSpot(Entities.ParkingSpot mp)
        {
            mp.Place_id = df.GetPlaceId(mp.FullAddress);
            UpdateSet(DAL.Converts.ParkSpotConvert.ConvertParkingSpotToEF(mp));
            return mp.Code;
        }
        public int DeleteParkingSpotByUser(Entities.User u)
        {
            if (pslst != null)
            {
                foreach (var item in pslst)
                {
                    if (item.UserCode == u.Code)
                    {

                        DeleteSet(DAL.Converts.ParkSpotConvert.ConvertParkingSpotToEF(item));

                    }
                    else
                        return 0;
                }
            }
            return 1;
        }
        public int DeleteParkingSpot(ParkingSpot p)
        {
            var l = pslst.First(i => i.Code == p.Code);
           DeleteSet(DAL.Converts.ParkSpotConvert.ConvertParkingSpotToEF(l));

            return 1;
        }
    }
}
