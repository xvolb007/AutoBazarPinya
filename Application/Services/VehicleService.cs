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

        public async Task<IEnumerable<VehicleDto>> GetAllAsync()
        {
            var vehicles = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<VehicleDto>>(vehicles);
        }
        public async Task<PagedResult<VehicleDto>> GetPagedAsync(DataTableRequest request)
        {
            var pagedEntities = await _repository.GetPagedAsync(request);

            return new PagedResult<VehicleDto>
            {
                Draw = request.Draw,
                RecordsTotal = pagedEntities.RecordsTotal,
                RecordsFiltered = pagedEntities.RecordsFiltered,
                Data = _mapper.Map<IEnumerable<VehicleDto>>(pagedEntities.Data)
            };
        }

        public async Task<VehicleDto?> GetByIdAsync(long id)
        {
            var vehicle = await _repository.GetByIdAsync(id);
            return _mapper.Map<VehicleDto?>(vehicle);
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
    }
}
