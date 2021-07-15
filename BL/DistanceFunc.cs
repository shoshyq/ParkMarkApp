using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Http;
using Newtonsoft.Json;

using Entities;


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

        //public static string BuildUrlForLocationId(string address)
        //{
        //    string location = "";
        //    string[] locationAsArray;
        //    locationAsArray = address.Split();

        //    for (int i = 0; i < locationAsArray.Length; i++)
        //    {
        //        if (i < locationAsArray.Length - 1)
        //            location += locationAsArray[i] + "+";
        //        else
        //            location += locationAsArray[i];
        //    }

        //    return "https://maps.googleapis.com/maps/api/place/textsearch/json?key=AIzaSyBDl3290lOEG_WUo66K6HzCfd-rO36-Poc&query=" + location;
        //}

        public static string BuildUrlForDistance(string place1, string place2)
        {
            string url = "https://maps.googleapis.com/maps/api/distancematrix/json?key=AIzaSyBDl3290lOEG_WUo66K6HzCfd-rO36-Poc&units=metric&mode=driving&origins=";
            return url + "place_id:" + place1 + "&destinations=place_id:" + place2;
        }
        //returns a dictionary= key: pspot code , value: distance to there in 00.000 km
        public Dictionary<int, string> GetDistanceToManyPoints(string origin_place_id, List<ParkingSpot> pspots)
        {
            string[] idLocations = new string[pspots.Count];
            for (int i = 0; i < idLocations.Length; i++)
            {
                idLocations[i] = pspots[i].Place_id;
            }
            var t = GetDistance(origin_place_id, idLocations);
            Dictionary<int, string> result_dict = new Dictionary<int, string>();
            for (int i = 0; i < t.Result.rows[0].elements.Length; i++)
            {
                result_dict.Add(key: pspots[i].Code, value: t.Result.rows[0].elements[i].distance.text);
            }
            return result_dict;
        }

        //returns a dictionary= key: pspot code , value: distance to there in 00.000 km
        public async Task<RootDistanceBase> GetDistance(string origin_place_id,string[] idLocations)
        {
            HttpClient http = new HttpClient();
           
            string url = BuildUrlForManyDistances(origin_place_id, idLocations);
            var responseDistances = Task.Run(() => http.GetAsync(url));
            
            if (responseDistances!=null)
            {
                var result = await responseDistances.Result.Content.ReadAsStringAsync();
                RootDistanceBase root = JsonConvert.DeserializeObject<RootDistanceBase>(result);
                return root;
             
            }
            else
               return null;
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

            string url = "https://maps.googleapis.com/maps/api/place/textsearch/json?key=AIzaSyBDl3290lOEG_WUo66K6HzCfd-rO36-Poc&query=" + location;
            HttpClient http = new HttpClient();

            var responseId = Task.Run(()=> http.GetAsync(url));

            if (responseId != null)
            {
                var result = await responseId.Result.Content.ReadAsStringAsync();
                RootLocationBase root = JsonConvert.DeserializeObject<RootLocationBase>(result);
                place_id = root.results[0].place_id;
            }
            else
                place_id = null;
            return place_id;
        }

        static string BuildUrlForManyDistances(string origin, string[] destinations)
        {
            string url = "https://maps.googleapis.com/maps/api/distancematrix/" +
                "json?key=AIzaSyBDl3290lOEG_WUo66K6HzCfd-rO36-Poc&units=metric&mode=driving&origins=";
            url += "place_id:" + origin + "&destinations=place_id:" + destinations[0];
            for (int i = 1; i < destinations.Length; i++)
            {
                url += "|place_id:" + destinations[i];
            }
            return url;

        }
        #endregion
    }
}
