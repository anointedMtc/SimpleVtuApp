using AutoMapper;
using Identity.Domain.Entities;
using Identity.Shared.DTO;

namespace Identity.Application.Mappings;

internal sealed class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        // IDENTITY WAHALA 
        CreateMap<ApplicationUserRegisterationRequestDto, ApplicationUser>()
            .ForMember(u => u.DateOfBirth, opt => opt.MapFrom(x => x.DateOfBirth));

        // FOR DATE OF BIRTH EXCHANGE
        CreateMap<DateOfBirthResponseDto, DateOnly>().ReverseMap();

        CreateMap<ApplicationUser, ApplicationUserResponseDto>()
            .ForMember(dest => dest.IsTwoFacAuthEnabled, src => src.MapFrom(src => src.TwoFactorEnabled))
            .ReverseMap();

        CreateMap<ApplicationUser, ApplicationUserShortResponseDto>()
            .ForMember(dest => dest.DateOfBirth, src => src.MapFrom(src => src.DateOfBirth))
            .ReverseMap();

        CreateMap<ApplicationUser, CurrentUserResponseDto>().ReverseMap();
        CreateMap<UpdateUserRequestDto, ApplicationUser>()
            .ForMember(dest => dest.DateOfBirth, src => src.MapFrom(src => src.DateOfBirth))
            .ReverseMap();


        // Role
        CreateMap<ApplicationRole, AddApplicationRoleRequestDto>().ReverseMap();
        CreateMap<ApplicationRole, UpdateApplicationRoleRequestDto>().ReverseMap();
        CreateMap<ApplicationRole, ApplicationRoleResponseDto>()
            .ForMember(u => u.RoleId, opt => opt.MapFrom(x => x.Id))
            .ReverseMap();

        CreateMap<ApplicationRoleForUserResponseDto, ApplicationRole>()
            .ReverseMap();


    }
}
