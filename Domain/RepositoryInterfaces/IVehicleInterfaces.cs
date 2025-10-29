using Domain.Entities;
using Domain.Models;

namespace Domain.RepositoryInterfaces
{
    public interface IVehicleRepository
    {
        Task<IEnumerable<Vehicle>> GetAllAsync();
        Task<Domain.Models.PagedResult<Vehicle>> GetPagedAsync(DataTableRequest request);
        Task<Vehicle?> GetByIdAsync(long id);
        Task<Vehicle?> GetByLicensePlateAsync(string plate);
        Task AddAsync(Vehicle vehicle);
        Task UpdateAsync(Vehicle vehicle);
        Task DeleteAsync(long id);
        Task<int> GetFilteredCountAsync(QueryFilter[] filters);
    }
}
