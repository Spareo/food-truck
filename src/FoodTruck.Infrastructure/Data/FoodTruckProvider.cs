using System;
using System.Collections.Generic;
using System.Linq;
using GeoCoordinatePortable;
using Microsoft.Extensions.Logging;
using FoodTruck.Core.Interfaces;

namespace FoodTruck.Infrastructure.Data
{
    public class FoodTruckProvider : IFoodTruckProvider
    {
        private readonly ILogger _logger;
        private readonly FoodTruckContext _context;

        public FoodTruckProvider(ILogger<FoodTruckProvider> logger, FoodTruckContext context)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Compare the users distance to each food truck and return the closest one
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        public Core.Models.FoodTruck GetClosestFoodTruck(double latitude, double longitude)
        {
            GeoCoordinate userLoc = new GeoCoordinate(latitude, longitude);

            Core.Models.FoodTruck closest = null;
            double closetsDistance = -1;

            foreach (var foodTruck in _context.FoodTrucks)
            {
                GeoCoordinate ftLoc = foodTruck.Location;

                if (closest != null)
                {
                    // Get the disstance of each food truck and return the closest 
                    double distanceMeters = userLoc.GetDistanceTo(ftLoc);
                    if (distanceMeters < closetsDistance)
                    {
                        closest = foodTruck;
                        closetsDistance = distanceMeters;
                        _logger.LogDebug($"Curent closest food truck: {foodTruck.Applicant} is {Math.Round(closetsDistance)} meters away");
                    }
                }
                else
                {
                    // If this is the first item in the loop, set it as our base item and continue
                    closest = foodTruck;
                    closetsDistance = userLoc.GetDistanceTo(ftLoc);
                    continue;
                }
            }

            return closest;
        }

        public IEnumerable<Core.Models.FoodTruck> GetClosestFoodTrucks(double longitude, double latitude, int milesRadius = 10)
        {
            throw new NotImplementedException();
        }
    }
}
