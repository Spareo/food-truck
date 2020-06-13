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
            var closest = GetClosestFoodTrucks(latitude, longitude).FirstOrDefault();
            return closest;
        }

        /// <summary>
        /// Return a collection of all the food trucks within the given radius
        /// </summary>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <param name="milesRadius"></param>
        /// <returns></returns>
        public IEnumerable<Core.Models.FoodTruck> GetClosestFoodTrucks(double latitude, double longitude, int milesRadius = 100)
        {
            List<Core.Models.FoodTruck> closestFoodTrucks = new List<Core.Models.FoodTruck>();

            // Setup GeoCoordinate for the given latitude and longitude
            GeoCoordinate userLoc = new GeoCoordinate(latitude, longitude);

            // Convert the milessRadius to meters 
            double milesInMeters = milesRadius / 0.00062137;

            // Calculate distances for all of the food trucks
            var distanceCalculations = _context.FoodTrucks.Select(x => new
            {
                LocationId = x.LocationId,
                Applicant = x.Applicant,
                Distance = userLoc.GetDistanceTo(x.Location)
            })
            .AsEnumerable()
            .OrderBy(x => x.Distance);

            // Capture the food trucks within the radius
            var closestLocationIds = distanceCalculations.Where(x => x.Distance <= milesInMeters)
                                               .Select(y => y.LocationId)
                                               .ToList();

            // Iterate over the closest location IDs to maintain the order
            for (int i = 0; i < closestLocationIds.Count; i++)
            {
                int locationId = closestLocationIds[i];
                var ft = _context.FoodTrucks.Where(x => x.LocationId == locationId).First();
                closestFoodTrucks.Add(ft);

            }

            return closestFoodTrucks;
        }
    }
}
