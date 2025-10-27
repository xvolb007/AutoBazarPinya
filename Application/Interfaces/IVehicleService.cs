using Application.Dtos;
using Domain.Models;

namespace Application.Interfaces
{
    public interface IVehicleService
    {
        Task<IEnumerable<VehicleDto>> GetAllAsync();
        Task<PagedResult<VehicleDto>> GetPagedAsync(DataTableRequest request);
        Task<VehicleDto?> GetByIdAsync(long id);
        Task AddAsync(CreateVehicleDto dto);
        Task UpdateAsync(UpdateVehicleDto dto);
        Task DeleteAsync(long id);
    }
}
