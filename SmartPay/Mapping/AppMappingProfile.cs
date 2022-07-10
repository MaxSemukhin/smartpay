using AutoMapper;
using SmartPay.Models;

namespace SmartPay.Mapping;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<Check, CheckViewModel>();
        CreateMap<Category, CategoryViewModel>();
        CreateMap<SubCategory, SubCategoryViewModel>();
        CreateMap<SubCategory, SubCategoryProductViewModel>();
        CreateMap<Product, ProductViewModel>();
        CreateMap<Merchant, MerchantViewModel>();
    }
}