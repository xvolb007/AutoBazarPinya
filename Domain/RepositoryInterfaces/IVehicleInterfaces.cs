using Domain.Entities;

namespace Domain.RepositoryInterfaces
{
    public interface IVehicleRepository
    {
        Task<IEnumerable<Vehicle>> GetAllAsync();
        Task<Vehicle?> GetByIdAsync(long id);
        Task<Vehicle?> GetByLicensePlateAsync(string plate);
        Task AddAsync(Vehicle vehicle);
        Task UpdateAsync(Vehicle vehicle);
        Task DeleteAsync(long id);
    }
}
