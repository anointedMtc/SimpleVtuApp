using AutoMapper;
using DomainSharedKernel.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Wallet.Domain.Entities.WalletAggregate;
using Wallet.Shared.DTO;

namespace Wallet.Application.Features.Queries.GetWalletById;

public class GetWalletByIdQueryHandler : IRequestHandler<GetWalletByIdQuery, GetWalletByIdResponse>
{
    private readonly IRepository<WalletDomainEntity> _repository;
    private readonly ILogger<GetWalletByIdQueryHandler> _logger;
    private readonly IMapper _mapper;

    public GetWalletByIdQueryHandler(IRepository<WalletDomainEntity> repository,
        ILogger<GetWalletByIdQueryHandler> logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<GetWalletByIdResponse> Handle(GetWalletByIdQuery request, CancellationToken cancellationToken)
    {
        var getWalletByIdResponse = new GetWalletByIdResponse();
        getWalletByIdResponse.WalletDto = new WalletDto();

        var spec = new GetWalletByIdSpecification(request.WalletId);
        var wallet = await _repository.FindAsync(spec);

        if (wallet == null)
        {
            getWalletByIdResponse.Success = false;
            getWalletByIdResponse.Message = $"You made a Bad Request.";

            return getWalletByIdResponse;
        }

        getWalletByIdResponse.WalletDto = _mapper.Map<WalletDto>(wallet);

        // this line here returns the total available balance in the wallet
        //wallet.CurrentAmount();

        getWalletByIdResponse.Success = true;
        getWalletByIdResponse.Message = $"This resource matched your search";

        return getWalletByIdResponse;

    }
}
