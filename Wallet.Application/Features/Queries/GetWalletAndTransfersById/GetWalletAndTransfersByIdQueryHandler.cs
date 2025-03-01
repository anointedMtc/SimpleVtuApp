using AutoMapper;
using Identity.Shared.Constants;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Interfaces;
using Wallet.Domain.Entities.WalletAggregate;
using Wallet.Domain.Interfaces;
using Wallet.Domain.Specifications;
using Wallet.Shared.DTO;

namespace Wallet.Application.Features.Queries.GetWalletAndTransfersById;

internal sealed class GetWalletAndTransfersByIdQueryHandler
    : IRequestHandler<GetWalletAndTransfersByIdQuery, GetWalletAndTransfersByIdResponse>
{
    private readonly IWalletRepository<WalletDomainEntity> _walletRepository;
    private readonly ILogger<GetWalletAndTransfersByIdQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;

    public GetWalletAndTransfersByIdQueryHandler(
        IWalletRepository<WalletDomainEntity> walletRepository, 
        ILogger<GetWalletAndTransfersByIdQueryHandler> logger, IMapper mapper, 
        IUserContext userContext, IResourceBaseAuthorizationService resourceBaseAuthorizationService)
    {
        _walletRepository = walletRepository;
        _logger = logger;
        _mapper = mapper;
        _userContext = userContext;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
    }

    public async Task<GetWalletAndTransfersByIdResponse> Handle(GetWalletAndTransfersByIdQuery request, CancellationToken cancellationToken)
    {
        var getWalletAndTransfersByIdResponse = new GetWalletAndTransfersByIdResponse
        {
            WalletDto = new()
        };

        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.AdminAndAbove))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand?.Email ?? "Anonymous User",
                nameof(GetWalletAndTransfersByIdQuery),
                request);

            //throw new ForbiddenAccessException();
            getWalletAndTransfersByIdResponse.Success = false;
            getWalletAndTransfersByIdResponse.Message = $"You are not authorized to access this endpoint.";
            getWalletAndTransfersByIdResponse.WalletDto = null;

            return getWalletAndTransfersByIdResponse;
        }

        var spec = new GetWalletAndTransfersByIdSpecification(request.WalletId);
        var wallet = await _walletRepository.FindAsync(spec);

        if (wallet == null)
        {
            getWalletAndTransfersByIdResponse.Success = false;
            getWalletAndTransfersByIdResponse.Message = $"You made a Bad Request.";

            return getWalletAndTransfersByIdResponse;
        }

        getWalletAndTransfersByIdResponse.WalletDto = _mapper.Map<WalletDto>(wallet);

        getWalletAndTransfersByIdResponse.Success = true;
        getWalletAndTransfersByIdResponse.Message = $"This resource matched your search";

        return getWalletAndTransfersByIdResponse;
    }
}
