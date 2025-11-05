using Application.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Models;
using Domain.RepositoryInterfaces;

namespace Application.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _repository;
        private readonly IMapper _mapper;

        public VehicleService(IVehicleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VehicleDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var vehicles = await _repository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<VehicleDto>>(vehicles);
        }
        public async Task<PagedResult<VehicleDto>> GetPagedAsync(DataTableRequest request, CancellationToken cancellationToken)
        {
            var pagedEntities = await _repository.GetPagedAsync(request, cancellationToken);

            return new PagedResult<VehicleDto>
            {
                Draw = request.Draw,
                RecordsTotal = pagedEntities.RecordsTotal,
                RecordsFiltered = pagedEntities.RecordsFiltered,
                Data = _mapper.Map<IEnumerable<VehicleDto>>(pagedEntities.Data)
            };
        }

        public async Task<VehicleDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            var vehicle = await _repository.GetByIdAsync(id, cancellationToken);
            return _mapper.Map<VehicleDto?>(vehicle);
        }
        public async Task<VehicleDto?> GetByLicensePlateAsync(string licensePlate, CancellationToken cancellationToken)
        {
            var exists = await _repository.GetByLicensePlateAsync(licensePlate, cancellationToken);
            return _mapper.Map<VehicleDto?>(exists);
        }

        public async Task AddAsync(CreateVehicleDto dto)
        {
            var exists = await _repository.GetByLicensePlateAsync(dto.LicensePlate);
            if (exists != null)
                throw new InvalidOperationException($"Vehicle with plate '{dto.LicensePlate}' already exists.");

            var entity = _mapper.Map<Vehicle>(dto);
            await _repository.AddAsync(entity);
        }

        public async Task UpdateAsync(UpdateVehicleDto dto)
        {
            var existing = await _repository.GetByIdAsync(dto.Id);
            if (existing == null)
                throw new KeyNotFoundException($"Vehicle with ID {dto.Id} not found.");

            var duplicate = await _repository.GetByLicensePlateAsync(dto.LicensePlate);
            if (duplicate != null && duplicate.Id != dto.Id)
                throw new InvalidOperationException($"License plate '{dto.LicensePlate}' already belongs to another vehicle.");

            _mapper.Map(dto, existing);
            await _repository.UpdateAsync(existing);
        }

        public async Task DeleteAsync(long id)
        {
            await _repository.DeleteAsync(id);
        }
        public async Task<int> GetFilteredCountAsync(QueryFilter[] filters, CancellationToken cancellationToken)
        {
            return await _repository.GetFilteredCountAsync(filters, cancellationToken);
        }
    }
}
