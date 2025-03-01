using AutoMapper;
using VtuApp.Domain.Entities.VtuModelAggregate;
using VtuApp.Shared.DTO;

namespace VtuApp.Application.Mappings;

internal sealed class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Customer, CustomerShortResponseDto>().ReverseMap();

        CreateMap<Customer, CustomerDto>()
            //.ForMember(dest => dest.VtuTransactions, src => src.MapFrom(src => src.VtuTransactions))
            //.ForMember(dest => dest.VtuBonusTransfers, src => src.MapFrom(src => src.VtuBonusTransfers))
            .ReverseMap();

        CreateMap<VtuAppTransfer, VtuBonusTransferDto>().ReverseMap();

        CreateMap<VtuTransaction, VtuTransactionDto>().ReverseMap();

    }
}
