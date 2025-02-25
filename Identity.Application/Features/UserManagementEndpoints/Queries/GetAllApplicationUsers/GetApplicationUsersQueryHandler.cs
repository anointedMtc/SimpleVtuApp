using AutoMapper;
using Identity.Application.Exceptions;
using Identity.Application.Specifications;
using Identity.Domain.Entities;
using Identity.Shared.Constants;
using Identity.Shared.DTO;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.HelperClasses;
using SharedKernel.Application.Interfaces;
using SharedKernel.Domain.Interfaces;

namespace Identity.Application.Features.UserManagementEndpoints.Queries.GetAllApplicationUsers;

public class GetApplicationUsersQueryHandler : IRequestHandler<GetApplicationUsersQuery, Pagination<GetApplicationUsersResponse>>
{
    private readonly IRepository<ApplicationUser> _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetApplicationUsersQueryHandler> _logger;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;
    private readonly IUserContext _userContext;

    public GetApplicationUsersQueryHandler(IRepository<ApplicationUser> repository,
        IMapper mapper, ILogger<GetApplicationUsersQueryHandler> logger,
        IResourceBaseAuthorizationService resourceBaseAuthorizationService,
        IUserContext userContext)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
        _userContext = userContext;
    }

    public async Task<Pagination<GetApplicationUsersResponse>> Handle(GetApplicationUsersQuery request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.AdminAndAbove))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand!.Email,
                typeof(GetApplicationUsersQuery),
                request);

            throw new CustomForbiddenException("Access Denied. You do not have Permission to view this resource");
        }

        var getApplicationUsersResponse = new GetApplicationUsersResponse();
        var totalUsers = 0;

        var spec = new ApplicationUserSpecification(request.PaginationFilterAppUser);

        var data = await _repository.GetAllAsync(spec);

        if (!data.Any())
        {
            getApplicationUsersResponse.Message = $"No resource matched your search. Please try a different entry";

            totalUsers = 0;

            _logger.LogError("Error 404. The resorce could not be found for: {RequestName}, with {@SearchParams} at {DateTimeUtc}",
                typeof(GetApplicationUsersQuery),
                request.PaginationFilterAppUser,
                DateTime.UtcNow);

            return new Pagination<GetApplicationUsersResponse>(request.PaginationFilterAppUser.PageNumber, request.PaginationFilterAppUser.PageSize, totalUsers, getApplicationUsersResponse);
        }

        totalUsers = await _repository.CountAsync(spec);

        getApplicationUsersResponse.ApplicationUserShortResponseDto = _mapper.Map<List<ApplicationUserShortResponseDto>>(data);

        getApplicationUsersResponse.Message = $"your query was successful and this is the list of Application Roles in {request.PaginationFilterAppUser.Sort} order";
        getApplicationUsersResponse.Success = true;


        return new Pagination<GetApplicationUsersResponse>(request.PaginationFilterAppUser.PageNumber, request.PaginationFilterAppUser.PageSize, totalUsers, getApplicationUsersResponse);
         
    }
}
