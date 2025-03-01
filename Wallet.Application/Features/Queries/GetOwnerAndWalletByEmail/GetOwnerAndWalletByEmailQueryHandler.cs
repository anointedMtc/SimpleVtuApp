using AutoMapper;
using Identity.Shared.Constants;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Interfaces;
using Wallet.Application.Features.Queries.GetWalletAndTransfersById;
using Wallet.Domain.Entities;
using Wallet.Domain.Interfaces;
using Wallet.Domain.Specifications;
using Wallet.Shared.DTO;

namespace Wallet.Application.Features.Queries.GetOwnerAndWalletByEmail;

internal sealed class GetOwnerAndWalletByEmailQueryHandler
    : IRequestHandler<GetOwnerAndWalletByEmailQuery, GetOwnerAndWalletByEmailResponse>
{
    private readonly IWalletRepository<Owner> _walletRepository;
    private readonly ILogger<GetWalletAndTransfersByIdQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;

    public GetOwnerAndWalletByEmailQueryHandler(IWalletRepository<Owner> walletRepository, 
        ILogger<GetWalletAndTransfersByIdQueryHandler> logger, IMapper mapper, 
        IUserContext userContext, IResourceBaseAuthorizationService resourceBaseAuthorizationService)
    {
        _walletRepository = walletRepository;
        _logger = logger;
        _mapper = mapper;
        _userContext = userContext;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
    }

    public async Task<GetOwnerAndWalletByEmailResponse> Handle(GetOwnerAndWalletByEmailQuery request, CancellationToken cancellationToken)
    {
        var getOwnerAndWalletByEmailResponse = new GetOwnerAndWalletByEmailResponse
        {
            OwnerLongResponseDto = new()
        };

        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.AdminAndAbove))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand?.Email ?? "Anonymous User",
                nameof(GetOwnerAndWalletByEmailQuery),
                request);

            //throw new ForbiddenAccessException();
            getOwnerAndWalletByEmailResponse.Success = false;
            getOwnerAndWalletByEmailResponse.Message = $"You are not authorized to access this endpoint.";
            getOwnerAndWalletByEmailResponse.OwnerLongResponseDto = null;

            return getOwnerAndWalletByEmailResponse;
        }

        var spec = new GetOwnerAndWalletByEmailSpecification(request.Email);
        var owner = await _walletRepository.FindAsync(spec);

        if (owner == null)
        {
            getOwnerAndWalletByEmailResponse.Success = false;
            getOwnerAndWalletByEmailResponse.Message = $"You made a Bad Request.";

            return getOwnerAndWalletByEmailResponse;
        }

        getOwnerAndWalletByEmailResponse.OwnerLongResponseDto = _mapper.Map<OwnerLongResponseDto>(owner);

        getOwnerAndWalletByEmailResponse.Success = true;
        getOwnerAndWalletByEmailResponse.Message = $"This resource matched your search";

        return getOwnerAndWalletByEmailResponse;
    }
}
