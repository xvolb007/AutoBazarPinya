using Application.Dtos;
using Domain.Models;

namespace Application.Interfaces
{
    public interface IVehicleService
    {
        Task<IEnumerable<VehicleDto>> GetAllAsync(CancellationToken cancellationToken);
        Task<PagedResult<VehicleDto>> GetPagedAsync(DataTableRequest request, CancellationToken cancellationToken);
        Task<VehicleDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<VehicleDto?> GetByLicensePlateAsync(string licensePlate, CancellationToken cancellationToken);
        Task AddAsync(CreateVehicleDto dto);
        Task UpdateAsync(UpdateVehicleDto dto);
        Task DeleteAsync(long id);
        Task<int> GetFilteredCountAsync(QueryFilter[] filters, CancellationToken cancellationToken);
    }
}
