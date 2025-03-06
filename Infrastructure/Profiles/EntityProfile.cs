using AutoMapper;
using Domain.Dtos;
using Domain.Dtos.BannerDto;
using Domain.Dtos.Request;
using Domain.Dtos.User;
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

        CreateMap<ChooseUs, CreateChooseUsDto>().ReverseMap();
        CreateMap<ChooseUs, UpdateChooseUsDto>().ReverseMap();
        CreateMap<ChooseUs, GetChooseUsDto>().ReverseMap();

        CreateMap<Branch, GetBranchDto>().ReverseMap();
        CreateMap<UpdateBranchDto, Branch>().ReverseMap();
        CreateMap<CreateBranchDto, Branch>().ReverseMap();
        
        CreateMap<User, GetUserDto>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName));

        CreateMap<CreateUserDto, User>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.RegistrationDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.ProfileImagePath, opt => opt.Ignore());

        CreateMap<UpdateUserDto, User>()
            .ForMember(dest => dest.UserName, opt => opt.Ignore())
            .ForMember(dest => dest.RegistrationDate, opt => opt.Ignore())
            .ForMember(dest => dest.ProfileImagePath, opt => opt.Ignore());
    }
}