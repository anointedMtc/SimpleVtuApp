using AutoMapper;
using Notification.Domain.Entities;
using Notification.Shared.DTO;

namespace Notification.Application.Mappings;

internal sealed class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<EmailEntity, EmailDto>().ReverseMap();
    }
}
