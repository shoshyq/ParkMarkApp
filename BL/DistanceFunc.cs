using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Http;
using Newtonsoft.Json;

using Entities;
using DAL;
using ParkingSpot = DAL.ParkingSpot;

namespace BL
{
    public class DistanceFunc
    {
        #region distance functions
        public static int status = 1;
        public int GetShortestDistance(string place_id1, string place_id2)
        {
            Task<int> t = FindDistance(place_id1, place_id2);
            return t.Result;
        }
        public static async Task<int> FindDistance(string place_id1, string place_id2)
        {
            string[] idLocations = { place_id1, place_id2 };
            HttpClient http = new HttpClient();

            var responseDistance = await http.GetAsync(BuildUrlForDistance(idLocations[0], idLocations[1]));

            if (responseDistance.IsSuccessStatusCode)
            {
                var result = await responseDistance.Content.ReadAsStringAsync();
                RootDistanceBase root = JsonConvert.DeserializeObject<RootDistanceBase>(result);
                return root.rows[0].elements[0].distance.value;
            }
            else
                return 0;
        }

        public static string BuildUrlForLocationId(string address)
        {
            string location = "";
            string[] locationAsArray;
            locationAsArray = address.Split();

            for (int i = 0; i < locationAsArray.Length; i++)
            {
                if (i < locationAsArray.Length - 1)
                    location += locationAsArray[i] + "+";
                else
                    location += locationAsArray[i];
            }

            return "https://maps.googleapis.com/maps/api/place/textsearch/json?key=AIzaSyDKdp5Tna0InvxI6tDyUSU3FYbpcWA7mYYs&query=" + location;
        }

        public static string BuildUrlForDistance(string place1, string place2)
        {
            string url = "https://maps.googleapis.com/maps/api/distancematrix/json?key=AIzaSyDKdp5Tna0InvxI6tDyUSU3FYbpcWA7mYY&units=metric&origins=";
            return url + "place_id:" + place1 + "&destinations=place_id:" + place2;
        }
        //returns a dictionary= key: pspot code , value: distance to there in 00.000 km
        public Dictionary<int, int> GetDistanceToManyPoints(string origin_place_id, List<ParkingSpot> pspots)
        {
            Task<Dictionary<int, int>> t = GetDistance(origin_place_id, pspots);
            return t.Result;
        }
        //returns a dictionary= key: pspot code , value: distance to there in 00.000 km
        public async Task<Dictionary<int, int>> GetDistance(string origin_place_id, List<ParkingSpot> pspots)
        {
            Dictionary<int, int> result_dict = new Dictionary<int, int>();
            string[] idLocations = new string[pspots.Count];
            HttpClient http = new HttpClient();
            for (int i = 0; i < idLocations.Length; i++)
            {
                idLocations[i] = pspots[i].Place_id;
            }
            string url = BuildUrlForManyDistances(origin_place_id, idLocations);
            var responseDistances = await http.GetAsync(url);

            if (responseDistances.IsSuccessStatusCode)
            {
                var result = await responseDistances.Content.ReadAsStringAsync();
                RootDistanceBase root = JsonConvert.DeserializeObject<RootDistanceBase>(result);
                // string dis_str;


                for (int i = 0; i < idLocations.Length; i++)
                {
                    //dis_str = (root.rows[0].elements[i].distance.value).ToString();
                    //double distance = Double.Parse(dis_str.Insert(dis_str.Length - 3, "."));
                    result_dict.Add(key: pspots[i].Code, value: root.rows[0].elements[i].distance.value);
                }
            }
            return result_dict;
        }
        public string GetPlaceId(string address)
        {
            Task<string> t = ConvertToPlaceId(address);
            return t.Result;
        }
        static async Task<string> ConvertToPlaceId(string address)
        {
            string location = "";
            string place_id;
            string[] locationAsArray = address.Split();
            for (int i = 0; i < locationAsArray.Length; i++)
            {
                if (i < locationAsArray.Length - 1)
                    location += locationAsArray[i] + "+";
                else
                    location += locationAsArray[i];
            }

            string url = "https://maps.googleapis.com/maps/api/place/textsearch/json?key=AIzaSyDKdp5Tna0InvxI6tDyUSU3FYbpcWA7mYY&query=" + location;
            HttpClient http = new HttpClient();

            var responseId = await http.GetAsync(url);

            if (responseId.IsSuccessStatusCode)
            {
                var result = await responseId.Content.ReadAsStringAsync();
                RootLocationBase root = JsonConvert.DeserializeObject<RootLocationBase>(result);
                place_id = root.results[0].place_id;
            }
            else
                place_id = null;
            return place_id;
        }

        static string BuildUrlForManyDistances(string origin, string[] destinations)
        {
            string url = "https://maps.googleapis.com/maps/api/distancematrix/json?key=AIzaSyDKdp5Tna0InvxI6tDyUSU3FYbpcWA7mYY&units=metric&origins=";
            url += "place_id:" + origin + "&destinations=place_id:" + destinations[0];
            for (int i = 1; i < destinations.Length - 1; i++)
            {
                url += "|place_id:" + destinations[i];
            }
            return url;

        }
        #endregion
    }
}
