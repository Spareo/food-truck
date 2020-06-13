using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoodTruck.Core.Interfaces;
using FoodTruck.Infrastructure;
using FoodTruck.Infrastructure.Data;
using FoodTruck.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using TinyCsvParser;

namespace FoodTruck.Tests
{
    [TestFixture]
    public class FoodTruckControllerTests
    {
        private IFoodTruckProvider foodTruckProvider;
        private FoodTruckContext foodTruckContext;
        private ILogger<FoodTruckProvider> foodTruckProviderLogger;

        [SetUp]
        public void Setup()
        {
            // Setup DB conmtext and provider 
            Mock<ILogger<FoodTruckProvider>> loggerMock = new Mock<ILogger<FoodTruckProvider>>();
            foodTruckProviderLogger = loggerMock.Object;


            var dbOptions = new DbContextOptionsBuilder<FoodTruckContext>()
                            .UseInMemoryDatabase(databaseName: "FoodTrucks")
                            .Options;

            foodTruckContext = new FoodTruckContext(dbOptions);
            foodTruckProvider = new FoodTruckProvider(foodTruckProviderLogger, foodTruckContext);

            // Seed test data
            CsvParserOptions csvParserOptions = new CsvParserOptions(true, ',');
            var csvParser = new CsvParser<Core.Models.FoodTruck>(csvParserOptions, new CsvFoodTruckMapping());
            var records = csvParser.ReadFromFile("Resources/Mobile_Food_Facility_Permit.csv", Encoding.UTF8).ToList();

            foreach (var record in records)
            {
                if (record.IsValid)
                    foodTruckContext.FoodTrucks.Add(record.Result);
            }

            foodTruckContext.SaveChanges();
        }


        [Test]
        public void GetClosestFoodTruck_Returns_Single_FoodTruck()
        {
            // Arrange
            Mock<ILogger<FoodTrucksController>> loggerMock = new Mock<ILogger<FoodTrucksController>>();
            FoodTrucksController controller = new FoodTrucksController(loggerMock.Object, foodTruckProvider);

            // Act
            var result = controller.GetClosestFoodTruck(37.76, -122.41);

            // Assert
            Assert.IsInstanceOf<JsonResult>(result);
            JsonResult jsonResult = result as JsonResult;

            Assert.IsInstanceOf<Core.Models.FoodTruck>(jsonResult.Value);
            Core.Models.FoodTruck ft = jsonResult.Value as Core.Models.FoodTruck;

            Assert.AreEqual(1181513, ft.LocationId);
        }

        [Test]
        public void GetClosestFoodTrucks_Returns_Collection_of_FoodTrucks()
        {
            // Arrange
            Mock<ILogger<FoodTrucksController>> loggerMock = new Mock<ILogger<FoodTrucksController>>();
            FoodTrucksController controller = new FoodTrucksController(loggerMock.Object, foodTruckProvider);

            // Act
            var result = controller.GetClosestFoodTrucks(37.76, -122.41, 100);

            // Assert
            Assert.IsInstanceOf<JsonResult>(result);
            JsonResult jsonResult = result as JsonResult;

            Assert.IsInstanceOf<List<Core.Models.FoodTruck>>(jsonResult.Value);
            var foodTrucks = jsonResult.Value as List<Core.Models.FoodTruck>;

            CollectionAssert.IsNotEmpty(foodTrucks);
            CollectionAssert.AllItemsAreInstancesOfType(foodTrucks, typeof(Core.Models.FoodTruck));
        }
    }
}
