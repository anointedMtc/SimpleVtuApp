using AutoMapper;
using Identity.Application.Exceptions;
using Identity.Application.Features.UserManagementEndpoints.Queries.GetAllApplicationUsers;
using Identity.Application.Specifications;
using Identity.Domain.Entities;
using Identity.Shared.Constants;
using Identity.Shared.DTO;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.HelperClasses;
using SharedKernel.Application.Interfaces;
using SharedKernel.Domain.Interfaces;

namespace Identity.Application.Features.UserManagementEndpoints.Queries.GetAllUsersInARole;

public class GetAllUsersInARoleQueryHandler : IRequestHandler<GetAllUsersInARoleQuery, Pagination<GetAllUsersInARoleResponse>>
{
    private readonly IRepository<ApplicationUser> _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllUsersInARoleQueryHandler> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;
    private readonly IUserContext _userContext;

    public GetAllUsersInARoleQueryHandler(IRepository<ApplicationUser> repository,
        IMapper mapper, ILogger<GetAllUsersInARoleQueryHandler> logger,
        UserManager<ApplicationUser> userManager, IResourceBaseAuthorizationService resourceBaseAuthorizationService, IUserContext userContext)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
        _userManager = userManager;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
        _userContext = userContext;
    }
    public async Task<Pagination<GetAllUsersInARoleResponse>> Handle(GetAllUsersInARoleQuery request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.AdminAndAbove))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand!.Email,
                typeof(GetAllUsersInARoleQuery),
                request);

            throw new CustomForbiddenException("Access Denied. You do not have Permission to view this resource");
        }

        var getAllUsersInARoleResponse = new GetAllUsersInARoleResponse();
        var totalUsers = 0;

        var spec = new ApplicationUserSpecification(request.PaginationFilterAppUser);

        var data = await _userManager.GetUsersInRoleAsync(request.PaginationFilterAppUser.Search ?? AppUserRoles.StandardUser);

        if (!data.Any())
        {
            getAllUsersInARoleResponse.Message = $"No resource matched your search. Please try a different entry";

            totalUsers = 0;

            _logger.LogError("Error 404. The resorce could not be found for: {RequestName}, with {@SearchParams} at {DateTimeUtc}",
                typeof(GetApplicationUsersQuery),
                request.PaginationFilterAppUser,
                DateTime.UtcNow);

            return new Pagination<GetAllUsersInARoleResponse>(request.PaginationFilterAppUser.PageNumber, request.PaginationFilterAppUser.PageSize, totalUsers, getAllUsersInARoleResponse);
        }

        totalUsers = await _repository.CountAsync(spec);

        getAllUsersInARoleResponse.ApplicationUserShortResponseDto = _mapper.Map<List<ApplicationUserShortResponseDto>>(data);

        getAllUsersInARoleResponse.Message = $"your query was successful and this is the list of Application Roles in {request.PaginationFilterAppUser.Sort} order";
        getAllUsersInARoleResponse.Success = true;


        return new Pagination<GetAllUsersInARoleResponse>(request.PaginationFilterAppUser.PageNumber, request.PaginationFilterAppUser.PageSize, totalUsers, getAllUsersInARoleResponse);

    }
}
