using AutoMapper;
using Wallet.Domain.Entities;
using Wallet.Domain.Entities.WalletAggregate;
using Wallet.Shared.DTO;

namespace Wallet.Application.Mappings;

internal sealed class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<WalletDomainEntity, WalletDto>()
            .ForMember(dest => dest.WalletId, src => src.MapFrom(src => src.WalletDomainEntityId))
            .ReverseMap();

        CreateMap<WalletDomainEntity, WalletShortResponseDto>()
          .ForMember(dest => dest.WalletId, src => src.MapFrom(src => src.WalletDomainEntityId))
          .ReverseMap();


        CreateMap<Transfer, TransferDto>()
            .ForMember(dest => dest.WalletId, src => src.MapFrom(src => src.WalletDomainEntityId))
            .ReverseMap();


        CreateMap<Owner, OwnerDto>().ReverseMap();

        CreateMap<Owner, OwnerLongResponseDto>()
            .ForMember(dest => dest.WalletDto, src => src.MapFrom(src => src.WalletDomainEntity))
            .ReverseMap();

       
    }
}
