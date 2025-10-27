using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public abstract class BaseVehicleDto
    {
        public string? Manufacturer { get; set; }
        public string? Model { get; set; }
        public int Year { get; set; }
        public int Mileage { get; set; }
        public FuelType Fuel { get; set; }
        public BodyType Body { get; set; }
        public string? LicensePlate { get; set; }
        public VehicleCondition Condition { get; set; }
        public DateTime InStockSince { get; set; } = DateTime.UtcNow;
        public string? Notes { get; set; }
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
