using Domain.Entities;
using Domain.Models;

namespace Domain.RepositoryInterfaces
{
    public interface IVehicleRepository
    {
        Task<IEnumerable<Vehicle>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Domain.Models.PagedResult<Vehicle>> GetPagedAsync(DataTableRequest request, CancellationToken cancellationToken = default);
        Task<Vehicle?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<Vehicle?> GetByLicensePlateAsync(string plate, CancellationToken cancellationToken = default);
        Task AddAsync(Vehicle vehicle);
        Task UpdateAsync(Vehicle vehicle);
        Task DeleteAsync(long id);
        Task<int> GetFilteredCountAsync(QueryFilter[] filters, CancellationToken cancellationToken = default);
    }
}
