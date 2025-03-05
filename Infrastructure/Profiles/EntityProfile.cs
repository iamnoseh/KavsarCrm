using AutoMapper;
using Domain.Dtos.BannerDto;
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
    }
}