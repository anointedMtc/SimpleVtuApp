using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Wallet.Domain.Entities.WalletAggregate;
using Wallet.Domain.Interfaces;
using Wallet.Domain.Specifications;
using Wallet.Shared.DTO;

namespace Wallet.Application.Features.Queries.GetWalletById;

public class GetWalletByIdQueryHandler : IRequestHandler<GetWalletByIdQuery, GetWalletByIdResponse>
{
    private readonly IWalletRepository<WalletDomainEntity> _walletRepository;
    private readonly ILogger<GetWalletByIdQueryHandler> _logger;
    private readonly IMapper _mapper;

    public GetWalletByIdQueryHandler(IWalletRepository<WalletDomainEntity> walletRepository,
        ILogger<GetWalletByIdQueryHandler> logger, IMapper mapper)
    {
        _walletRepository = walletRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<GetWalletByIdResponse> Handle(GetWalletByIdQuery request, CancellationToken cancellationToken)
    {
        var getWalletByIdResponse = new GetWalletByIdResponse();
        getWalletByIdResponse.WalletDto = new WalletDto();

        var spec = new GetWalletByIdSpecification(request.WalletId);
        var wallet = await _walletRepository.FindAsync(spec);

        if (wallet == null)
        {
            getWalletByIdResponse.Success = false;
            getWalletByIdResponse.Message = $"You made a Bad Request.";

            return getWalletByIdResponse;
        }

        getWalletByIdResponse.WalletDto = _mapper.Map<WalletDto>(wallet);

        getWalletByIdResponse.Success = true;
        getWalletByIdResponse.Message = $"This resource matched your search";

        return getWalletByIdResponse;
    }
}
