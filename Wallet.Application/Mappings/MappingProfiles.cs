using AutoMapper;
using Wallet.Domain.Entities;
using Wallet.Domain.Entities.WalletAggregate;
using Wallet.Shared.DTO;

namespace Wallet.Application.Mappings;

internal sealed class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<WalletDomainEntity, WalletDto>().ReverseMap();

        CreateMap<Owner, OwnerDto>().ReverseMap();

        CreateMap<Transfer, TransferDto>().ReverseMap();
    }
}
