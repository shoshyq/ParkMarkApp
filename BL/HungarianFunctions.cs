using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
using Entities;


namespace BL
{
    public class HungarianFunctions
    {
        DistanceFunc df;
        ConvertFuncBL convertFuncBL;
        int[,] costs;
        public HungarianFunctions()
        {
            df = new DistanceFunc();
            convertFuncBL = new ConvertFuncBL();
        }
        #region functions for hungarian
        //Main algorithm function  - returns dictionary key:city, value: dictionary - schedule of pspots and searches
        public Dictionary<int, Dictionary<int?, int>> PSpotsAllSearchesByCities(DateTime date)
        {
            Dictionary<int, Dictionary<int?, int>> dic = new Dictionary<int, Dictionary<int?, int>>();
            // dic = key:city, value: list<searches>
            var bycitySearchesDic = DAL.Convert.SearchConvert.ConvertSearchesListToEntity(DbHandler.GetAll<DAL.ParkingSpotSearch>()).Where(e => ((e.Regularly == true)&&(((DateTime)(e.SearchDate)).Date.ToString("d") == date.Date.ToString("d")))).GroupBy(t => t.CityCode).ToDictionary(w => (int)w.Key, w => w.ToList());
            //dic = key:city, value: list<spots>
            var bycitySpotsDic = DAL.Convert.ParkSpotConvert.ConvertParkingSpotsListToEntity(DbHandler.GetAll<DAL.ParkingSpot>()).Where(s => s.AvRegularly == true).GroupBy(y => y.CityCode).ToDictionary(u => (int)(u.Key), u => u.ToList());
            foreach (var item in bycitySearchesDic)
            {
                List<Entities.ParkingSpot> spotslist = bycitySpotsDic[item.Key].ToList();
                var resultInsideDic = ParkingSpotPerUser(spotslist, item.Value);
                dic.Add(key: item.Key, value: resultInsideDic);
            }
            return dic;

        }
        // Inside Main algorithm function - calculates parkspot per search by city code, returns dictionary <spot_code, search_code>
        public Dictionary<int?, int> ParkingSpotPerUser(List<Entities.ParkingSpot> pspots, List<Entities.ParkingSpotSearch> psearches)
        {
            //costs array for hungarian Algorithm - rows: spots, columns: searches
            //costs = new int[pspots.Count, psearches.Count];
            var groupOfPSpots = new List<PSpotHandler>();
            int[] spotsClients;
            int? minCost = int.MaxValue;
            // converting the pspots and searches lists to it handler lists
            List<PSpotHandler> parkSpotsMatrixList = convertFuncBL.ConvertToPSpotHandlerList(pspots);
            List<PSpotSearchHandler> parkSearchesMatrixList = convertFuncBL.ConvertToSpotSearchHandlerList(psearches);

            Dictionary<int?, int> settingPSpotsPerSearch = new Dictionary<int?, int>();
            Dictionary<int?, int> settingPSpotsPerSearchEnding = new Dictionary<int?, int>();
            int? costPerOption = 0;
            for (int i = 0; i < (parkSpotsMatrixList.Count / parkSearchesMatrixList.Count); i++)
            {
                for (int j = 0; j < parkSearchesMatrixList.Count; j++)
                {
                    groupOfPSpots.Add(parkSpotsMatrixList[j + (parkSearchesMatrixList.Count * i)]);
                }
                List<int> resultlist = FillCosts(groupOfPSpots, parkSearchesMatrixList);
                resultlist.RemoveAt(resultlist.Count - 1);
                spotsClients = resultlist.ToArray();
                costPerOption = resultlist[resultlist.Count - 1];
                settingPSpotsPerSearch.Clear();

                if (costPerOption < minCost)
                {
                    minCost = costPerOption;
                    for (int s = 0; s < spotsClients.Count(); s++)
                    {
                        settingPSpotsPerSearch.Add(groupOfPSpots[s].Code, parkSearchesMatrixList[spotsClients[s]].Code);
                    }
                    settingPSpotsPerSearchEnding = settingPSpotsPerSearch;
                }
                groupOfPSpots.Clear();
            }
            if((parkSpotsMatrixList.Count / parkSearchesMatrixList.Count)>1)
            {
                //mutations 
                for (int i = 0; i < (parkSpotsMatrixList.Count / parkSearchesMatrixList.Count); i++)
                {
                    for (int j = i; j < parkSearchesMatrixList.Count * (parkSpotsMatrixList.Count / parkSearchesMatrixList.Count); j += parkSpotsMatrixList.Count / parkSearchesMatrixList.Count)
                    {
                        groupOfPSpots.Add(parkSpotsMatrixList[j]);
                    }
                    List<int> resultlist = FillCosts(groupOfPSpots, parkSearchesMatrixList);
                    resultlist.RemoveAt(resultlist.Count - 1);
                    spotsClients = resultlist.ToArray();
                    costPerOption = resultlist[resultlist.Count - 1];
                    settingPSpotsPerSearch.Clear();
                    if (costPerOption < minCost)
                    {
                        minCost = costPerOption;
                        for (int s = 0; s < spotsClients.Count(); s++)
                        {
                            settingPSpotsPerSearch.Add(groupOfPSpots[s].Code, parkSearchesMatrixList[spotsClients[s]].Code);
                        }
                        settingPSpotsPerSearchEnding = settingPSpotsPerSearch;
                    }
                    groupOfPSpots.Clear();
                }
            }
            
            if ((parkSpotsMatrixList.Count % parkSearchesMatrixList.Count) != 0)
            {
                var rand = new Random();
                int prev = -1;
                for (int l = parkSpotsMatrixList.Count - (parkSpotsMatrixList.Count % parkSearchesMatrixList.Count) - 1; l < parkSpotsMatrixList.Count; l++)
                    groupOfPSpots.Add(parkSpotsMatrixList[l]);
                for (int k = 0; k < parkSearchesMatrixList.Count - (parkSpotsMatrixList.Count % parkSearchesMatrixList.Count); k++)
                {
                    int y = rand.Next(0, parkSpotsMatrixList.Count - (parkSpotsMatrixList.Count % parkSearchesMatrixList.Count));
                    if (y == prev)
                    {
                        while(y==prev)
                        {
                             y = rand.Next(0, parkSpotsMatrixList.Count - (parkSpotsMatrixList.Count % parkSearchesMatrixList.Count));
                        }
                    }
                    prev = y;
                    groupOfPSpots.Add(parkSpotsMatrixList[y]);
                }
                List<int> resultlist = FillCosts(parkSpotsMatrixList, parkSearchesMatrixList);
                resultlist.RemoveAt(resultlist.Count - 1);
                spotsClients = resultlist.ToArray();
                costPerOption = resultlist[resultlist.Count - 1];
             
                settingPSpotsPerSearch.Clear();
                if (costPerOption < minCost)
                {
                    minCost = costPerOption;
                    for (int s = 0; s < spotsClients.Count(); s++)
                    {
                        settingPSpotsPerSearch.Add(groupOfPSpots[s].Code, parkSearchesMatrixList[spotsClients[s]].Code);
                    }
                    settingPSpotsPerSearchEnding = settingPSpotsPerSearch;
                }
            }
            return settingPSpotsPerSearchEnding;
        }
        // calculates costs matrix, sends it to hungarian_function and returns its result array 
        private List<int> FillCosts(List<PSpotHandler> parkingSpotsList, List<PSpotSearchHandler> parkingSearchesList)
        {
            costs = new int[parkingSearchesList.Count, parkingSearchesList.Count];
            int[] finalArray;

            var listofdbcks = DbHandler.GetAll<Feedback>();
            var groupOfSpots = new List<PSpotHandler>();
            int costPerSearchAndSpot = 0;
       
            int cost_sum = 0;
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

                                        if (groupOfSpots[spot].Hours[i].Any(h => (Double.Parse(h.StartHour) <= Double.Parse(item.StartHour)) && (Double.Parse(h.EndHour) >= Double.Parse(item.EndHour))))
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
                    if ((parkingSearchesList[search].SizeOpt == true) && (groupOfSpots[spot].SpotLength != null)
                        && (groupOfSpots[spot].SpotWidth != null))
                    {
                        if (!((parkingSearchesList[search].PreferableWidth >= groupOfSpots[spot].SpotWidth)
                            && (parkingSearchesList[search].PreferableLength >= groupOfSpots[spot].SpotLength)))
                            costPerSearchAndSpot += 10;
                    }
                    // adds 30 points for no price matching
                    if (!((parkingSearchesList[search].MinPrice <= groupOfSpots[spot].PricePerHour)
                        && (parkingSearchesList[search].MaxPrice >= groupOfSpots[spot].PricePerHour)))
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
                    cost_sum += costPerSearchAndSpot;
                    costPerSearchAndSpot = 0;
                }
            }

            //an array of spots, each cell represents the row index and the value represent the column index
            finalArray = HungarianAlgorithm.FindAssignments(costs);
            List<int> flist = finalArray.OfType<int>().ToList();
            flist.Add(cost_sum);
            return flist;
        }
        #endregion
    }

}
