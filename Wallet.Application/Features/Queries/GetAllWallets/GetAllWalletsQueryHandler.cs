using AutoMapper;
using Identity.Shared.Constants;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.HelperClasses;
using SharedKernel.Application.Interfaces;
using Wallet.Domain.Entities.WalletAggregate;
using Wallet.Domain.Interfaces;
using Wallet.Domain.Specifications;
using Wallet.Shared.DTO;

namespace Wallet.Application.Features.Queries.GetAllWallets;

internal sealed class GetAllWalletsQueryHandler
    : IRequestHandler<GetAllWalletsQuery, Pagination<GetAllWalletsResponse>>
{
    private readonly IWalletRepository<WalletDomainEntity> _walletRepository;
    private readonly ILogger<GetAllWalletsQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;

    public GetAllWalletsQueryHandler(IWalletRepository<WalletDomainEntity> walletRepository, 
        ILogger<GetAllWalletsQueryHandler> logger, IMapper mapper, 
        IUserContext userContext, IResourceBaseAuthorizationService resourceBaseAuthorizationService)
    {
        _walletRepository = walletRepository;
        _logger = logger;
        _mapper = mapper;
        _userContext = userContext;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
    }

    public async Task<Pagination<GetAllWalletsResponse>> Handle(GetAllWalletsQuery request, CancellationToken cancellationToken)
    {
        var getAllWalletsResponse = new GetAllWalletsResponse
        {
            WalletShortResponseDtos = []
        };
        int totalUsers = 0;

        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.AdminAndAbove))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand?.Email ?? "Anonymous User",
                nameof(GetAllWalletsQuery),
                request);

            //throw new ForbiddenAccessException();

            getAllWalletsResponse.Success = false;
            getAllWalletsResponse.Message = $"You are not authorized to access this endpoint.";
            getAllWalletsResponse.WalletShortResponseDtos = null;

            return new Pagination<GetAllWalletsResponse>(request.PaginationFilter.PageNumber, request.PaginationFilter.PageSize, totalUsers, getAllWalletsResponse);
        }

        var spec = new GetAllWalletsSpecification(request.PaginationFilter);

        var data = await _walletRepository.GetAllAsync(spec);
        totalUsers = await _walletRepository.CountAsync(spec);

        getAllWalletsResponse.Success = true;
        getAllWalletsResponse.Message = $"your query was successful and this is the list of UserCreatedSagaInstance in {request.PaginationFilter.Sort ?? "Default"} order, matching {request.PaginationFilter.Search ?? "No search or filters"}";
        getAllWalletsResponse.WalletShortResponseDtos = _mapper.Map<List<WalletShortResponseDto>>(data);


        return new Pagination<GetAllWalletsResponse>(request.PaginationFilter.PageNumber, request.PaginationFilter.PageSize, totalUsers, getAllWalletsResponse);
    }
}
