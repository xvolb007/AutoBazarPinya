using Domain.Enums;
using Domain.Extensions;
using System.Text.Json.Serialization;

namespace Application.Dtos
{
    public abstract class BaseVehicleDto
    {
        public string? Manufacturer { get; set; }
        public string? Model { get; set; }
        public int Year { get; set; }
        public int Mileage { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public FuelType Fuel { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public BodyType Body { get; set; }
        public string? LicensePlate { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public VehicleCondition Condition { get; set; }
        public DateTime InStockSince { get; set; } = DateTime.UtcNow;
        public string? Notes { get; set; }
        public string FuelDisplay => Fuel.GetDisplayName();
        public string BodyDisplay => Body.GetDisplayName();
        public string ConditionDisplay => Condition.GetDisplayName();
    }

    public class VehicleDto : BaseVehicleDto
    {
        public double Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
    public class CreateVehicleDto : BaseVehicleDto
    {
    }
    public class UpdateVehicleDto : BaseVehicleDto
    {
        public long Id { get; set; }
    }
}
