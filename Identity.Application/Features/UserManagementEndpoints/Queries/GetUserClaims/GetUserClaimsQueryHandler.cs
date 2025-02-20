using ApplicationSharedKernel.Interfaces;
using Identity.Application.Exceptions;
using Identity.Domain.Entities;
using Identity.Shared.Constants;
using Identity.Shared.DTO;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Identity.Application.Features.UserManagementEndpoints.Queries.GetUserClaims;

public class GetUserClaimsQueryHandler : IRequestHandler<GetUserClaimsQuery, GetUserClaimsResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<GetUserClaimsQueryHandler> _logger;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;

    public GetUserClaimsQueryHandler(UserManager<ApplicationUser> userManager,
        ILogger<GetUserClaimsQueryHandler> logger, IUserContext userContext,
        IResourceBaseAuthorizationService resourceBaseAuthorizationService)
    {
        _userManager = userManager;
        _logger = logger;
        _userContext = userContext;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
    }

    public async Task<GetUserClaimsResponse> Handle(GetUserClaimsQuery request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.AdminAndAbove))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand!.Email,
                typeof(GetUserClaimsQuery),
                request);

            throw new CustomForbiddenException("Access Denied. You do not have Permission to view this resource");
        }

        var getUserClaimsResponse = new GetUserClaimsResponse();
        getUserClaimsResponse.GetUserClaimsResponseDto = new GetUserClaimsResponseDto();

        var user = await _userManager.FindByIdAsync(request.UserId.ToString());

        if (user == null)
        {
            _logger.LogError("User with Id {UserId} not found while performing {typeOfRequest} by {AdminId}",
                request.UserId,
                typeof(GetUserClaimsQuery),
                userExecutingCommand!.Email);

            getUserClaimsResponse.Success = false;
            getUserClaimsResponse.Message = "Bad Request";

            throw new CustomBadRequestException();
        }

        var existingClaims = await _userManager.GetClaimsAsync(user);

        foreach (var claim in existingClaims)
        {
            if (!getUserClaimsResponse.GetUserClaimsResponseDto.UserClaims.ContainsKey(claim.Type))
            {
                getUserClaimsResponse.GetUserClaimsResponseDto.UserClaims.Add(claim.Type.ToString(), new List<string> { claim.Value.ToString() });
            }
            else
            {
                getUserClaimsResponse.GetUserClaimsResponseDto.UserClaims[claim.Type.ToString()].Add(claim.Value.ToString());
            }
        }

        _logger.LogInformation("Admin {AdminEmail} retrieved claims for User with Id {UserId}",
            userExecutingCommand!.Email,
            user.Email);

        getUserClaimsResponse.GetUserClaimsResponseDto.UserId = user.Id;
        getUserClaimsResponse.GetUserClaimsResponseDto.Email = user.Email!;
        getUserClaimsResponse.GetUserClaimsResponseDto.UserClaims = getUserClaimsResponse.GetUserClaimsResponseDto.UserClaims.OrderBy(e => e.Key).ToDictionary();
        getUserClaimsResponse.GetUserClaimsResponseDto.Roles = await _userManager.GetRolesAsync(user);

        getUserClaimsResponse.Success = true;
        getUserClaimsResponse.Message = "Successfully retrieved claims for User";

        return getUserClaimsResponse;
    }
}
