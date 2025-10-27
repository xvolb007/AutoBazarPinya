using System.ComponentModel.DataAnnotations;

namespace Domain.Enums
{
    public enum FuelType
    {
        [Display(Name = "Benzín")]
        Petrol,

        [Display(Name = "Nafta")]
        Diesel,

        [Display(Name = "Elektřina")]
        Electric,

        [Display(Name = "Hybrid")]
        Hybrid
    }

    public enum BodyType
    {
        [Display(Name = "Sedan")]
        Sedan,

        [Display(Name = "Hatchback")]
        Hatchback,

        [Display(Name = "SUV")]
        SUV,

        [Display(Name = "Kupé")]
        Coupe,

        [Display(Name = "Dodávka")]
        Van
    }

    public enum VehicleCondition
    {
        [Display(Name = "Vynikající")]
        Excellent,

        [Display(Name = "Dobrý")]
        Good,

        [Display(Name = "Uspokojivý")]
        Fair,

        [Display(Name = "Špatný")]
        Poor
    }
}
