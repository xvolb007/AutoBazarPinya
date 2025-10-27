using Application.Dtos;

namespace Application.Interfaces
{
    public interface IVehicleService
    {
        Task<IEnumerable<VehicleDto>> GetAllAsync();
        Task<VehicleDto?> GetByIdAsync(long id);
        Task AddAsync(CreateVehicleDto dto);
        Task UpdateAsync(UpdateVehicleDto dto);
        Task DeleteAsync(long id);
    }
}
