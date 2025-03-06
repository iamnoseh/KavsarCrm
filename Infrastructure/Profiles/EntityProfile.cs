using AutoMapper;
using Domain.Dtos.BannerDto;
using Domain.Dtos.Request;
using Domain.Entities;

namespace Infrastructure.Profiles;

public class EntityProfile : Profile
{
    public EntityProfile()
    {
        CreateMap<CreateBannerDto, Banner>()
            .ForMember(dest => dest.ImagePath, opt => opt.Ignore());
        CreateMap<UpdateBannerDto, Banner>()
            .ForMember(dest => dest.ImagePath, opt => opt.Ignore());

        CreateMap<GetRequestDto, Request>().ReverseMap();
        CreateMap<CreateRequestDto, Request>().ReverseMap();
        CreateMap<UpdateRequestDto, Request>().ReverseMap();
    }
}