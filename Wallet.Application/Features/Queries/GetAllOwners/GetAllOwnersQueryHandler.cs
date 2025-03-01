using AutoMapper;
using Identity.Shared.Constants;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.HelperClasses;
using SharedKernel.Application.Interfaces;
using Wallet.Domain.Entities;
using Wallet.Domain.Interfaces;
using Wallet.Domain.Specifications;
using Wallet.Shared.DTO;

namespace Wallet.Application.Features.Queries.GetAllOwners;

internal sealed class GetAllOwnersQueryHandler
    : IRequestHandler<GetAllOwnersQuery, Pagination<GetAllOwnersResponse>>
{
    private readonly IWalletRepository<Owner> _walletRepository;
    private readonly ILogger<GetAllOwnersQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;

    public GetAllOwnersQueryHandler(IWalletRepository<Owner> walletRepository, 
        ILogger<GetAllOwnersQueryHandler> logger, IMapper mapper, 
        IUserContext userContext, IResourceBaseAuthorizationService resourceBaseAuthorizationService)
    {
        _walletRepository = walletRepository;
        _logger = logger;
        _mapper = mapper;
        _userContext = userContext;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
    }

    public async Task<Pagination<GetAllOwnersResponse>> Handle(GetAllOwnersQuery request, CancellationToken cancellationToken)
    {
        var getAllOwnersResponse = new GetAllOwnersResponse
        {
            OwnerDtos = []
        };
        int totalUsers = 0;

        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.AdminAndAbove))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand?.Email ?? "Anonymous User",
                nameof(GetAllOwnersQuery),
                request);

            //throw new ForbiddenAccessException();

            getAllOwnersResponse.Success = false;
            getAllOwnersResponse.Message = $"You are not authorized to access this endpoint.";
            getAllOwnersResponse.OwnerDtos = null;

            return new Pagination<GetAllOwnersResponse>(request.PaginationFilter.PageNumber, request.PaginationFilter.PageSize, totalUsers, getAllOwnersResponse);
        }

        var spec = new GetAllOwnersSpecification(request.PaginationFilter);

        var data = await _walletRepository.GetAllAsync(spec);
        totalUsers = await _walletRepository.CountAsync(spec);

        getAllOwnersResponse.Success = true;
        getAllOwnersResponse.Message = $"your query was successful and this is the list of UserCreatedSagaInstance in {request.PaginationFilter.Sort ?? "Default"} order, matching {request.PaginationFilter.Search ?? "No search or filters"}";
        getAllOwnersResponse.OwnerDtos = _mapper.Map<List<OwnerDto>>(data);

        return new Pagination<GetAllOwnersResponse>(request.PaginationFilter.PageNumber, request.PaginationFilter.PageSize, totalUsers, getAllOwnersResponse);
    }
}
