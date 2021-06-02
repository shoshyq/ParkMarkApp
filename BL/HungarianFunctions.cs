using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
using DAL;

namespace BL
{
    public class HungarianFunctions
    {
        DBConnection DBCon;
        MainBL mbl;
        DistanceFunc df;
        ConvertFuncBL convertFuncBL;
        public HungarianFunctions()
        {
            DBCon = new DBConnection();
            mbl = new MainBL();
            df = new DistanceFunc();
            convertFuncBL = new ConvertFuncBL();
        }
        #region functions for hungarian
        //Main algorithm function  - returns dictionary key:city, value: dictionary - schedule of pspots and searches
        public Dictionary<int, Dictionary<int, int>> PSpotsAllSearchesByCities()
        {
            Dictionary<int, Dictionary<int, int>> dic = new Dictionary<int, Dictionary<int, int>>();
            // dic = key:city, value: list<searches>
            var bycitySearchesDic = DbHandler.GetAll<ParkingSpotSearch>().Where(e => e.Regularly == true).GroupBy(t => t.City).ToDictionary(w => w.Key, w => w.ToList());
            //dic = key:city, value: list<spots>
            var bycitySpotsDic = DbHandler.GetAll<ParkingSpot>().Where(s => s.AvRegularly == true).GroupBy(y => y.City).ToDictionary(u => u.Key, u => u.ToList());
            foreach (var item in bycitySearchesDic)
            {
                List<DAL.ParkingSpot> spotslist = bycitySpotsDic[item.Key].ToList();
                var resultInsideDic = ParkingSpotPerUser(spotslist, item.Value);
                dic.Add(key: item.Key.Code, value: resultInsideDic);
            }
            return dic;

        }
        // Inside Main algorithm function - calculates parkspot per search by city code, returns dictionary <spot_code, search_code>
        public Dictionary<int, int> ParkingSpotPerUser(List<DAL.ParkingSpot> pspots, List<DAL.ParkingSpotSearch> psearches)
        {
            //costs array for hungarian Algorithm - rows: spots, columns: searches
            int[,] costs = new int[pspots.Count, psearches.Count];
            var groupOfPSpots = new List<DAL.ParkingSpot>();
            int[] spotsClients;
            int minCost = int.MaxValue;
            List<PSpotHandler> parkSpotsMatrixList = convertFuncBL.ConvertToPSpotHandlerList(pspots);
            List<PSpotSearchHandler> parkSearchesMatrixList = convertFuncBL.ConvertToSpotSearchHandlerList(psearches);

            Dictionary<int, int> settingPSpotsPerSearch = new Dictionary<int, int>();
            int costPerOption;
            for (int i = 0; i < (parkSpotsMatrixList.Count / parkSearchesMatrixList.Count); i++)
            {
                spotsClients = FillCosts(parkSpotsMatrixList, parkSearchesMatrixList, i, 1);
                costPerOption = 0;
                for (int k = 0; k < spotsClients.Count(); k++)
                {
                    costPerOption += costs[k, spotsClients[k]];
                }
                if (costPerOption < minCost)
                {
                    for (int t = 0; t < spotsClients.Count(); t++)
                    {
                        settingPSpotsPerSearch.Clear();
                        settingPSpotsPerSearch.Add(groupOfPSpots[t].Code, parkSearchesMatrixList[spotsClients[t]].Code);
                    }
                    minCost = costPerOption;
                }
                groupOfPSpots.Clear();
            }
            //mutations ???
            for (int i = 0; i < (parkSpotsMatrixList.Count / parkSearchesMatrixList.Count); i++)
            {
                spotsClients = FillCosts(parkSpotsMatrixList, parkSearchesMatrixList, i, 2);
                costPerOption = 0;
                for (int a = 0; a < spotsClients.Count(); a++)
                {
                    costPerOption += costs[a, spotsClients[a]];
                }
                if (costPerOption < minCost)
                {
                    for (int s = 0; s < spotsClients.Count(); s++)
                    {
                        settingPSpotsPerSearch.Clear();
                        settingPSpotsPerSearch.Add(groupOfPSpots[s].Code, parkSearchesMatrixList[spotsClients[s]].Code);
                    }
                    minCost = costPerOption;
                }
                groupOfPSpots.Clear();
            }
            //חלוקה עם שארית
            if ((parkSpotsMatrixList.Count % parkSearchesMatrixList.Count) != 0)
            {
                spotsClients = FillCosts(parkSpotsMatrixList, parkSearchesMatrixList, 0, 1);
                costPerOption = 0;
                for (int f = 0; f < spotsClients.Count(); f++)
                {
                    costPerOption += costs[f, spotsClients[f]];
                }
                if (costPerOption < minCost)
                {
                    for (int s = 0; s < spotsClients.Count(); s++)
                    {
                        settingPSpotsPerSearch.Clear();
                        settingPSpotsPerSearch.Add(groupOfPSpots[s].Code, parkSearchesMatrixList[spotsClients[s]].Code);
                    }
                    minCost = costPerOption;
                }
            }
            return settingPSpotsPerSearch;
        }
        // calculates costs matrix, sends it to hungarian_function and returns its result array 
        private int[] FillCosts(List<PSpotHandler> parkingSpotsList, List<PSpotSearchHandler> parkingSearchesList, int group, int mutation_num)
        {
            int[,] costs = new int[parkingSearchesList.Count, parkingSearchesList.Count];
            int[] finalArray;

            var listofdbcks = DbHandler.GetAll<Feedback>();
            var groupOfSpots = new List<PSpotHandler>();
            int costPerSearchAndSpot = 0;
            //קבוצה מתוך הנגנים כמספר הכלים
            if (mutation_num == 2)
                for (int j = 0; j < parkingSearchesList.Count; j += 2)
                {
                    groupOfSpots.Add(parkingSpotsList[j + (parkingSearchesList.Count * group)]);
                }
            else
                if (mutation_num == 1)
                for (int j = 0; j < parkingSearchesList.Count; j++)
                {
                    groupOfSpots.Add(parkingSpotsList[j + (parkingSearchesList.Count * group)]);
                }
            else
            {
                for (int k = parkingSpotsList.Count % parkingSearchesList.Count; k < parkingSpotsList.Count; k++)
                {
                    groupOfSpots.Add(parkingSpotsList[k]);
                }
                for (int t = 0; t < parkingSearchesList.Count - groupOfSpots.Count; t++)
                {
                    groupOfSpots.Add(parkingSpotsList[t]);
                }
            }
            //sets the cost value per spotXsearch
            for (int spot = 0; spot < groupOfSpots.Count(); spot++)
            {
                var fdbckPerUser = listofdbcks.Where(l => l.DescriptedUserCode == groupOfSpots[spot].UserCode);
                int? avgRating = fdbckPerUser.Select(r => r.Rating).Sum() / fdbckPerUser.Count();
                for (int search = 0; search < parkingSearchesList.Count(); search++)
                {
                    // var that counts days mathed to days in search
                    int dayscount = 0;
                    // days in search (assumed)
                    int sdayscount = 6;
                    //checks the hours available per parking spot renting
                    // sets maxValue if doesn't fit the hours
                    for (int i = 0; i < 6; i++)
                    {
                        if (parkingSearchesList[search].Hours[i] != null)
                        {
                            //var that counts matched hours at day i in parkspot_hourlist per parksearch_hourlist
                            int scount = parkingSearchesList[search].Hours[i].Count;
                            int inpspot_count = 0;
                            if (scount != 0)
                            {
                                foreach (var item in parkingSearchesList[search].Hours[i])
                                {
                                    if (item != null)
                                    {
                                        //checks if there is any hours in parkspot_hourlist that matches hours in parksearch_hourlist at that day

                                        if (groupOfSpots[spot].Hours[i].Any(h => (h.StartHour <= item.StartHour) && (h.EndHour >= item.EndHour)))
                                            inpspot_count++;
                                    }
                                }
                                if (scount == inpspot_count)
                                    dayscount++;
                            }
                            else
                                sdayscount--;
                        }
                        else
                            sdayscount--;
                    }
                    if (dayscount == 0)
                    {
                        costs[spot, search] = int.MaxValue;
                        continue;
                    }
                    //adds points according to amount of days matching
                    else
                    {
                        if ((sdayscount == 1) && ((sdayscount - dayscount) == 1))
                            costPerSearchAndSpot = int.MaxValue;
                        if (sdayscount == 2)
                        {
                            switch (sdayscount - dayscount)
                            {
                                case (0):
                                    break;
                                case (1):
                                    costPerSearchAndSpot += 50;
                                    break;
                                case (2):
                                    costPerSearchAndSpot = int.MaxValue;
                                    break;
                                default:
                                    break;
                            }
                        }
                        if (sdayscount == 3)
                        {
                            switch (sdayscount - dayscount)
                            {
                                case (0):
                                    break;
                                case (1):
                                    costPerSearchAndSpot += 40;
                                    break;
                                case (2):
                                    costPerSearchAndSpot += 70;
                                    break;
                                case (3):
                                    costPerSearchAndSpot = int.MaxValue;
                                    break;
                                default:
                                    break;
                            }
                        }
                        if (sdayscount == 4)
                        {
                            switch (sdayscount - dayscount)
                            {
                                case (0):
                                    break;
                                case (1):
                                    costPerSearchAndSpot += 30;
                                    break;
                                case (2):
                                    costPerSearchAndSpot += 50;
                                    break;
                                case (3):
                                    costPerSearchAndSpot += 70;
                                    break;
                                case (4):
                                    costPerSearchAndSpot = int.MaxValue;
                                    break;

                                default:
                                    break;
                            }
                        }
                        if (sdayscount == 5)
                        {
                            switch (sdayscount - dayscount)
                            {
                                case (0):
                                    break;
                                case (1):
                                    costPerSearchAndSpot += 10;
                                    break;
                                case (2):
                                    costPerSearchAndSpot += 30;
                                    break;
                                case (3):
                                    costPerSearchAndSpot += 60;
                                    break;
                                case (4):
                                    costPerSearchAndSpot += 80;
                                    break;
                                case (5):
                                    costPerSearchAndSpot = int.MaxValue;
                                    break;
                                default:
                                    break;
                            }
                        }
                        if (sdayscount == 6)
                        {
                            switch (sdayscount - dayscount)
                            {
                                case (0):
                                    break;
                                case (1):
                                    costPerSearchAndSpot += 10;
                                    break;
                                case (2):
                                    costPerSearchAndSpot += 30;
                                    break;
                                case (3):
                                    costPerSearchAndSpot += 50;
                                    break;
                                case (4):
                                    costPerSearchAndSpot += 70;
                                    break;
                                case (5):
                                    costPerSearchAndSpot += 100;
                                    break;
                                case (6):
                                    costPerSearchAndSpot = int.MaxValue;
                                    break;
                                default:
                                    break;
                            }
                        }

                    }
                    //adds 300 points if it's farther than 500 m
                    int distance = df.GetShortestDistance(parkingSearchesList[search].Place_Id, parkingSpotsList[spot].Place_Id);
                    if (distance > 50)
                    {
                        costPerSearchAndSpot += 10;
                    }
                    else
                    {

                        if (distance > 100)
                        {
                            costPerSearchAndSpot += 20;
                        }
                        else
                        {

                            if (distance > 200)
                            {
                                costPerSearchAndSpot += 50;
                            }
                            else
                            {

                                if (distance > 400)
                                {
                                    costPerSearchAndSpot += 100;
                                }
                                else
                                {

                                    if (distance > 500)
                                    {
                                        costPerSearchAndSpot = int.MaxValue;
                                    }
                                }
                            }
                        }
                    }


                    // adds  10 points for no size matching
                    if ((parkingSearchesList[search].SizeOpt == true) && (groupOfSpots[spot].SpotLength != null) && (groupOfSpots[spot].SpotWidth != null))
                    {
                        if (!((parkingSearchesList[search].PreferableWidth >= groupOfSpots[spot].SpotWidth) && (parkingSearchesList[search].PreferableLength >= groupOfSpots[spot].SpotLength)))
                            costPerSearchAndSpot += 10;
                    }
                    // adds 30 points for no price matching
                    if (!((parkingSearchesList[search].MinPrice <= groupOfSpots[spot].PricePerHour) && (parkingSearchesList[search].MaxPrice >= groupOfSpots[spot].PricePerHour)))
                    {
                        costPerSearchAndSpot += 30;
                    }
                    // adds 5 points for no roof matching
                    if ((parkingSearchesList[search].RoofOpt == true) && (groupOfSpots[spot].HasRoof == false))
                    {
                        costPerSearchAndSpot += 5;
                    }
                    // calculates the rating points (5 minus rating (from 1 to 5))*3
                    if (avgRating != null)
                    {
                        costPerSearchAndSpot += ((5 - (int)avgRating) * 3);
                    }

                    costs[spot, search] = costPerSearchAndSpot;
                    costPerSearchAndSpot = 0;
                }
            }

            //an array of spots, each cell represents the row index and the value represent the column index
            return finalArray = HungarianAlgorithm.FindAssignments(costs);
        }
        #endregion
    }

}
