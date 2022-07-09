using AutoMapper;
using SmartPay.Models;

namespace SmartPay.Mapping;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<Check, CheckViewModel>();
    }
}