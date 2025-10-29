using Domain.Models;
using ViewModels;

namespace Application.Interfaces
{
    public interface IVehicleFilterMapper
    {
        QueryFilter[] Map(VehicleFilterVm vm);
    }
}
