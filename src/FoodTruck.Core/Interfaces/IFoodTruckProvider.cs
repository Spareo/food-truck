using System.Collections.Generic;

namespace FoodTruck.Core.Interfaces
{
    public interface IFoodTruckProvider
    {
        Models.FoodTruck GetClosestFoodTruck(double latitude, double longitude);
        IEnumerable<Models.FoodTruck> GetClosestFoodTrucks(double longitude, double latitude, int milesRadius = 10);
    }
}