using AutoMapper;
using RealEstate.Application.DTOs;
using RealEstate.Domain.Entities;

namespace RealEstate.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Property, PropertyDto>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src =>
                    src.Images != null && src.Images.Any(i => i.Enabled)
                        ? src.Images.First(i => i.Enabled).File
                        : string.Empty));

            CreateMap<Owner, OwnerDto>();
            CreateMap<PropertyImage, PropertyImageDto>();
            CreateMap<PropertyTrace, PropertyTraceDto>();
        }
    }
}