using AutoMapper;
using Identity.Shared.Constants;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.HelperClasses;
using SharedKernel.Application.Interfaces;
using VtuApp.Domain.Entities.VtuModelAggregate;
using VtuApp.Domain.Interfaces;
using VtuApp.Domain.Specifications;
using VtuApp.Shared.DTO;

namespace VtuApp.Application.Features.Queries.GetAllVtuCustomers;

internal sealed class GetAllVtuCustomersQueryHandler : IRequestHandler<GetAllVtuCustomersQuery, Pagination<GetAllVtuCustomersResponse>>
{
    private readonly ILogger<GetAllVtuCustomersQueryHandler> _logger;
    private readonly IVtuAppRepository<Customer> _vtuAppRepository;
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;

    public GetAllVtuCustomersQueryHandler(ILogger<GetAllVtuCustomersQueryHandler> logger,
        IVtuAppRepository<Customer> vtuAppRepository, IMapper mapper,
        IUserContext userContext, IResourceBaseAuthorizationService resourceBaseAuthorizationService)
    {
        _logger = logger;
        _vtuAppRepository = vtuAppRepository;
        _mapper = mapper;
        _userContext = userContext;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
    }

    public async Task<Pagination<GetAllVtuCustomersResponse>> Handle(GetAllVtuCustomersQuery request, CancellationToken cancellationToken)
    {
        var getAllCustomersResponse = new GetAllVtuCustomersResponse
        {
            CustomerShortResponseDto = []
        };
        int totalUsers = 0;

        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.AdminAndAbove))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand?.Email ?? "Anonymous User",
                nameof(GetAllVtuCustomersQuery),
                request);

            //throw new ForbiddenAccessException();

            getAllCustomersResponse.Success = false;
            getAllCustomersResponse.Message = $"You are not authorized to access this endpoint.";
            getAllCustomersResponse.CustomerShortResponseDto = null;

            return new Pagination<GetAllVtuCustomersResponse>(request.PaginationFilter.PageNumber, request.PaginationFilter.PageSize, totalUsers, getAllCustomersResponse);
        }

        var spec = new GetAllVtuCustomersSpecification(request.PaginationFilter);

        var data = await _vtuAppRepository.GetAllAsync(spec);
        totalUsers = await _vtuAppRepository.CountAsync(spec);

        getAllCustomersResponse.Success = true;
        getAllCustomersResponse.Message = $"your query was successful and this is the list of UserCreatedSagaInstance in {request.PaginationFilter.Sort ?? "Default"} order, matching {request.PaginationFilter.Search ?? "No search or filters"}";
        getAllCustomersResponse.CustomerShortResponseDto = _mapper.Map<List<CustomerShortResponseDto>>(data);

        return new Pagination<GetAllVtuCustomersResponse>(request.PaginationFilter.PageNumber, request.PaginationFilter.PageSize, totalUsers, getAllCustomersResponse);
    }
}
