﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using DAL;
using ParkingSpot = DAL.ParkingSpot;

namespace BL
{
    public  class ParkingSpotsBL : DbHandler
    {
        DistanceFunc df;
        List<Entities.ParkingSpot> pslst = DAL.Convert.ParkSpotConvert.ConvertParkingSpotsListToEntity(GetAll<ParkingSpot>());
        public ParkingSpotsBL()//
        {
            df = new DistanceFunc();
        }
        // registering new parking spot . returns parking spot code if succeeds
        public int RegisterUsersParkSpot(Entities.ParkingSpot mp)
        {
            if (!pslst.Any(d => d.Place_id.Trim() == mp.Place_id.Trim()))
            {
                //if (!DbHandler.GetAll<ParkingLocation>().Any(d => d.Place_Id.Trim() == mp.Place_id.Trim()))
                //{
                mp.Place_id = df.GetPlaceId(mp.FullAddress);
               AddSet(DAL.Convert.ParkSpotConvert.ConvertParkingSpotToEF(mp));

                //DbHandler.AddSet<ParkingLocation>(new ParkingLocation
                //{
                //    ParkingCode = mp.Code,
                //    Place_Id = mp.Place_id,
                //    FullAddress = mp.FullAddress
                //    //});
                //}
                return pslst.First(w => w.Code == mp.Code).Code;
            }
            return -1;

        }
        // updating a parking spot . returns code if succeeds
        public int UpdateUsersParkSpot(Entities.ParkingSpot mp)
        {
            mp.Place_id = df.GetPlaceId(mp.FullAddress);
            UpdateSet(DAL.Convert.ParkSpotConvert.ConvertParkingSpotToEF(mp));
            return mp.Code;
        }
        public int DeleteParkingSpotByUser(Entities.User u)
        {
            if (pslst != null)
            {
                if(pslst.Any(ps => ps.UserCode == u.Code))
                    DeleteSet(DAL.Convert.ParkSpotConvert.ConvertParkingSpotToEF(pslst.First(ps => ps.UserCode == u.Code)));                            
                
            }
            return 0;
        }
        public int DeleteParkingSpot(Entities.ParkingSpot p)
        {
            var l = pslst.First(i => i.Code == p.Code);
           DeleteSet(DAL.Convert.ParkSpotConvert.ConvertParkingSpotToEF(l));

            return 0;
        }
    }
}
