using FoodTruck.Core.Enums;
using FoodTruck.Core.Models;
using TinyCsvParser.Mapping;
using TinyCsvParser.TypeConverter;

namespace FoodTruck.Infrastructure
{
    public class CsvFoodTruckMapping : CsvMapping<Core.Models.FoodTruck>
    {
        public CsvFoodTruckMapping() : base()
        {
            MapProperty(0, x => x.LocationId);
            MapProperty(1, x => x.Applicant);
            MapProperty(2, x => x.FacilityType, new EnumConverter<FacilityType>(true));
            MapProperty(4, x => x.LocationDescription);
            MapProperty(5, x => x.Address);
            MapProperty(9, x => x.Permit);
            MapProperty(10, x => x.Status, new EnumConverter<PermitStatus>(true));
            MapProperty(11, x => x.FoodItems);
            MapProperty(12, x => x.X);
            MapProperty(13, x => x.Y);
            MapProperty(14, x => x.Latitude);
            MapProperty(15, x => x.Longitude);
            MapProperty(16, x => x.Schedule);
            MapProperty(17, x => x.DaysHours);
        }
    }
}
