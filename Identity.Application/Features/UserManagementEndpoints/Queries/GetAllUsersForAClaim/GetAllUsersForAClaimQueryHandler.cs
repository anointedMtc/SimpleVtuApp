using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Identity.Shared.DTO;
using Identity.Application.Features.UserManagementEndpoints.Queries.GetAllApplicationUsers;
using Identity.Domain.Entities;
using Identity.Shared.Constants;
using Identity.Application.Exceptions;
using Identity.Application.Specifications;
using SharedKernel.Domain.Interfaces;
using SharedKernel.Application.Interfaces;
using SharedKernel.Application.HelperClasses;

namespace Identity.Application.Features.UserManagementEndpoints.Queries.GetAllUsersForAClaim;

public class GetAllUsersForAClaimQueryHandler : IRequestHandler<GetAllUsersForAClaimQuery, Pagination<GetAllUsersForAClaimResponse>>
{
    private readonly IRepository<ApplicationUser> _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllUsersForAClaimQueryHandler> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;
    private readonly IUserContext _userContext;

    public GetAllUsersForAClaimQueryHandler(IRepository<ApplicationUser> repository,
        IMapper mapper, ILogger<GetAllUsersForAClaimQueryHandler> logger,
        UserManager<ApplicationUser> userManager, IResourceBaseAuthorizationService resourceBaseAuthorizationService, IUserContext userContext)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
        _userManager = userManager;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
        _userContext = userContext;
    }
    public async Task<Pagination<GetAllUsersForAClaimResponse>> Handle(GetAllUsersForAClaimQuery request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.AdminAndAbove))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand!.Email,
                typeof(GetAllUsersForAClaimQuery),
                request);

            throw new CustomForbiddenException("Access Denied. You do not have Permission to view this resource");
        }

        var getAllUsersForAClaimResponse = new GetAllUsersForAClaimResponse();
        var totalUsers = 0;
        IEnumerable<ApplicationUser> data = [];

        var spec = new ApplicationUserSpecification(request.PaginationFilterAppUser);

        foreach (var claim in request.GetAllUsersForAClaimRequestDto.UserClaims)
        {
            data = await _userManager.GetUsersForClaimAsync(new Claim(claim.Key, claim.Value));
        }

        var newData = data.Where(e => e.Id.Contains(request.PaginationFilterAppUser.Search, StringComparison.OrdinalIgnoreCase) ||
                                      e.FirstName.Contains(request.PaginationFilterAppUser.Search, StringComparison.OrdinalIgnoreCase) ||
                                      e.LastName.Contains(request.PaginationFilterAppUser.Search, StringComparison.OrdinalIgnoreCase) ||
                                      e.Email.Contains(request.PaginationFilterAppUser.Search, StringComparison.OrdinalIgnoreCase) ||
                                      e.ConstUserName.Contains(request.PaginationFilterAppUser.Search, StringComparison.OrdinalIgnoreCase) ||
                                      e.UserName.Contains(request.PaginationFilterAppUser.Search, StringComparison.OrdinalIgnoreCase) ||
                                      e.Gender.Contains(request.PaginationFilterAppUser.Search, StringComparison.OrdinalIgnoreCase) ||
                                      e.Nationality.Contains(request.PaginationFilterAppUser.Search, StringComparison.OrdinalIgnoreCase) ||
                                      e.DateOfBirth.HasValue.ToString().Contains(request.PaginationFilterAppUser.Search, StringComparison.OrdinalIgnoreCase) ||
                                      e.LastLogin.ToString().Contains(request.PaginationFilterAppUser.Search, StringComparison.OrdinalIgnoreCase) ||
                                      e.UpdatedAt.ToString().Contains(request.PaginationFilterAppUser.Search, StringComparison.OrdinalIgnoreCase) ||

                                      e.Id.Length > 4
                                      );

        //var data = await _userManager.GetUsersInRoleAsync(request.PaginationFilterAppUser.Search ?? AppUserRoles.StandardUser);


        //var data = await _userManager.GetUsersForClaimAsync(claimsToFind);

        if (!newData.Any())
        {
            getAllUsersForAClaimResponse.Message = $"No resource matched your search. Please try a different entry";

            totalUsers = 0;

            _logger.LogError("Error 404. The resorce could not be found for: {RequestName}, with {@SearchParams} at {DateTimeUtc}",
                typeof(GetApplicationUsersQuery),
                request.PaginationFilterAppUser,
                DateTime.UtcNow);

            return new Pagination<GetAllUsersForAClaimResponse>(request.PaginationFilterAppUser.PageNumber, request.PaginationFilterAppUser.PageSize, totalUsers, getAllUsersForAClaimResponse);
        }

        totalUsers = data.Count();

        getAllUsersForAClaimResponse.ApplicationUserShortResponseDto = _mapper.Map<List<ApplicationUserShortResponseDto>>(newData);

        getAllUsersForAClaimResponse.Message = $"your query was successful and this is the list of Application Roles in {request.PaginationFilterAppUser.Sort} order";
        getAllUsersForAClaimResponse.Success = true;


        return new Pagination<GetAllUsersForAClaimResponse>(request.PaginationFilterAppUser.PageNumber, request.PaginationFilterAppUser.PageSize, totalUsers, getAllUsersForAClaimResponse);

    }
}
