using Domain.Entities;
using Domain.Extensions;
using Domain.Models;
using Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Infrastructure.Persistence.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly AppDbContext _context;

        public VehicleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Vehicle>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Vehicles
                .AsNoTracking()
                .OrderBy(v => v.Manufacturer)
                .ToListAsync(cancellationToken);
        }
        public async Task<Domain.Models.PagedResult<Vehicle>> GetPagedAsync(DataTableRequest request, CancellationToken cancellationToken = default)
        {
            var query = _context.Vehicles.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Search?.Value))
            {
                var search = request.Search.Value.ToLower();
                query = query.Where(v =>
                    v.Manufacturer.ToLower().Contains(search) ||
                    v.Model.ToLower().Contains(search) ||
                    v.LicensePlate.ToLower().Contains(search));
            }

            var total = await query.CountAsync();

            if (request.Order?.Any() == true)
            {
                var orderColumn = request.Columns?[request.Order[0].Column].Data;
                var dir = request.Order[0].Dir;
                if (!string.IsNullOrEmpty(orderColumn))
                {
                    query = query.OrderBy($"{orderColumn} {dir}");
                }
            }
            var data = await query
                .Skip(request.Start)
                .Take(request.Length)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return new Domain.Models.PagedResult<Vehicle>
            {
                Draw = request.Draw,
                RecordsTotal = total,
                RecordsFiltered = total,
                Data = data
            };
        }
        public async Task<Vehicle?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return await _context.Vehicles
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
        }

        public async Task<Vehicle?> GetByLicensePlateAsync(string plate, CancellationToken cancellationToken = default)
        {
            return await _context.Vehicles
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.LicensePlate == plate, cancellationToken);
        }

        public async Task AddAsync(Vehicle vehicle)
        {
            await _context.Vehicles.AddAsync(vehicle);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Vehicle vehicle)
        {
            _context.Vehicles.Update(vehicle);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var entity = await _context.Vehicles.FindAsync(id);
            if (entity != null)
            {
                _context.Vehicles.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
        #region Stats
        public async Task<int> GetFilteredCountAsync(QueryFilter[] filters, CancellationToken cancellationToken = default)
        {
            return await _context.Vehicles
                .AsNoTracking()
                .ApplyFilters(filters)
                .CountAsync(cancellationToken);
        }
        #endregion
    }
}
