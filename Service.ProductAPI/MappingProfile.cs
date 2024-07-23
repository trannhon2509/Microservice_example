using AutoMapper;
using Service.ProductAPI.Dtos;
using Service.ProductAPI.Models;

namespace Service.ProductAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
        }
    }
}
