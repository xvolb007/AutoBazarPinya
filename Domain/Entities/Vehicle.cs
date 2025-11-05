using Domain.Enums;

namespace Domain.Entities
{
    public class Vehicle : BaseEntity<long>
    {
        public string? Manufacturer { get; set; }
        public string? Model { get; set; }
        public int Year { get; set; }
        public int Mileage { get; set; }
        public FuelType Fuel { get; set; }
        public BodyType Body { get; set; }
        public string? LicensePlate { get; set; }
        public VehicleCondition Condition { get; set; }
        public DateTime InStockSince { get; set; }
        public string? Notes { get; set; }
    }
}
