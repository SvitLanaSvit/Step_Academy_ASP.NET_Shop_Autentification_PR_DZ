using ASP_Meeting_18.Data;
using ASP_Meeting_18.Models.DTOs.ProductDTOs;
using AutoMapper;

namespace ASP_Meeting_18.AutoMapperProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile() 
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
        }
    }
}
