using System;
using System.Linq;
using System.Threading.Tasks;
using FoodTruck.Core.Interfaces;
using FoodTruck.Infrastructure.Data;
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
            var closestFoodTruck = _foodTruckProvider.GetClosestFoodTruck(latitude, longitude);
            return Ok(closestFoodTruck); 
        }
    }
}
