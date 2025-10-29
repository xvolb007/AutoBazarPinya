using Application.Interfaces;
using Domain.Models;
using ViewModels;

namespace Application.Services
{
    public class VehicleFilterMapper : IVehicleFilterMapper
    {
        public QueryFilter[] Map(VehicleFilterVm vm)
        {
            var filters = new List<QueryFilter>();

            void Add(string column, Operation op, object? val)
            {
                if (val == null) return;
                var str = val is Enum e ? ((int)(object)e).ToString() : val.ToString()!;
                filters.Add(new QueryFilter { Column = column, Operation = op, Values = [str] });
            }

            Add("Fuel", Operation.eq, vm.Fuel);
            Add("Body", Operation.eq, vm.Body);
            Add("Condition", Operation.eq, vm.Condition);
            Add("Manufacturer", Operation.like, vm.Manufacturer);
            Add("Model", Operation.like, vm.Model);
            Add("Year", Operation.gte, vm.YearMin);
            Add("Year", Operation.lte, vm.YearMax);
            Add("Mileage", Operation.gte, vm.MileageMin);
            Add("Mileage", Operation.lte, vm.MileageMax);

            return filters.ToArray();
        }
    }
}
