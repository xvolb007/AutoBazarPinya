using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ViewModels
{
    public class VehicleViewModel
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Pole {0} je povinné.")]
        public string Manufacturer { get; set; } = string.Empty;

        [Required(ErrorMessage = "Pole {0} je povinné.")]
        public string Model { get; set; } = string.Empty;

        [Range(1900, 2100, ErrorMessage = "Rok musí být mezi 1900 a 2100.")]
        public int Year { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Najeté kilometry musí být nezáporné číslo.")]
        public int Mileage { get; set; }

        [Required(ErrorMessage = "Pole {0} je povinné.")]
        [RegularExpression(@"^[1-9][A-Z]{1,2}\s?[0-9]{1,4}$", ErrorMessage = "Neplatný formát SPZ.")]
        public string LicensePlate { get; set; } = string.Empty;

        public FuelType Fuel { get; set; }
        public BodyType Body { get; set; }
        public VehicleCondition Condition { get; set; }
        public string? Notes { get; set; }
    }
}
