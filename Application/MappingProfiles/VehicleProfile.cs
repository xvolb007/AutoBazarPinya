using Application.Dtos;
using AutoMapper;
using Domain.Entities;
using ViewModels;

namespace Application.MappingProfiles
{
    public class VehicleProfile : Profile
    {
        public VehicleProfile()
        {
            // Entity - DTO
            CreateMap<Vehicle, VehicleDto>();
            CreateMap<CreateVehicleDto, Vehicle>();
            CreateMap<UpdateVehicleDto, Vehicle>();
            // DTO - ViewModel
            CreateMap<VehicleDto, VehicleViewModel>().ReverseMap();
            CreateMap<CreateVehicleDto, VehicleViewModel>().ReverseMap();
            CreateMap<UpdateVehicleDto, VehicleViewModel>().ReverseMap();

        }
    }
}
