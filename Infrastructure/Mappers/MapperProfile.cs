using AutoMapper;
using DemoMinimalAPI.DTOs;
using DemoMinimalAPI.Models;

namespace DemoMinimalAPI.Infrastructure.Mappers;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Supplier, SupplierResponseDTO>();
        CreateMap<CreateUpdateSupplierDTO, Supplier>();
    }
}
