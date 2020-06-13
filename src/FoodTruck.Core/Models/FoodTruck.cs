using System;
using System.Text.Json.Serialization;
using FoodTruck.Core.Enums;
using GeoCoordinatePortable;

namespace FoodTruck.Core.Models
{
    public class FoodTruck
    {
        public int LocationId { get; set; }

        public string Applicant { get; set; }

        public FacilityType FacilityType { get; set; }

        public string LocationDescription { get; set; }

        public string Address { get; set; }

        public string Permit { get; set; }

        public PermitStatus Status { get; set; }

        public string FoodItems { get; set; }

        [JsonIgnore]
        public double X { get; set; }

        [JsonIgnore]
        public double Y { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Schedule { get; set; }

        public string DaysHours { get; set; }

        [JsonIgnore]
        public GeoCoordinate Location { get { return new GeoCoordinate(Latitude, Longitude); } }
    }
}
