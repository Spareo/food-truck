using System;
using System.Collections.Generic;
using System.Linq;
using FoodTruck.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

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
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [SwaggerOperation("GetClosestFoodTruck", "Returns the closest food truck within a 100 mile radius based on the given geographic coordinates")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", type: typeof(Core.Models.FoodTruck))]
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
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [SwaggerOperation("GetClosestFoodTruck", "Returns a collection of the closest foodtrucks within the given radius for the given geographic coordinates")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", type: typeof(List<Core.Models.FoodTruck>))]
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
