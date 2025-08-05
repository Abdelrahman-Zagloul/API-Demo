using AutoMapper;
using API_Demo.Dto;
using API_Demo.Model;

namespace API_Demo.Configuration
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Metadata, MetadataDto>().ReverseMap();
        }
    }
}
