using System;
using System.Linq;
using FoodTruck.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FoodTruck.Web.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class FoodTrucksController : Controller
    {
        private readonly ILogger _logger;
        private readonly IFoodTruckProvider _foodTruckProvider;

        public FoodTrucksController(ILogger<FoodTrucksController> logger, IFoodTruckProvider foodTruckProvider)
        {
            _logger = logger;
            _foodTruckProvider = foodTruckProvider;
        }

        [HttpGet("closest/{latitude}/{longitude}")]
        public IActionResult GetClosestFoodTruck(double latitude, double longitude)
        {
            try
            {
                var closestFoodTruck = _foodTruckProvider.GetClosestFoodTruck(latitude, longitude);
                if (closestFoodTruck != null)
                    return Ok(closestFoodTruck);
                else
                    return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Getting closest food truck failed.");
                return BadRequest();
            }
        }


        [HttpGet("closest/{latitude}/{longitude}/{milesRadius}")]
        public IActionResult GetClosestFoodTrucks(double latitude, double longitude, int milesRadius)
        {
            try
            {
                var closestFoodTrucks = _foodTruckProvider.GetClosestFoodTrucks(latitude, longitude, milesRadius);
                if (closestFoodTrucks != null && closestFoodTrucks.Count() > 0)
                    return Ok(closestFoodTrucks);
                else
                    return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Getting closest food truck failed.");
                return BadRequest();
            }
        }
    }
}
