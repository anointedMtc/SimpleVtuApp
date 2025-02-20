using ApplicationSharedKernel.Interfaces;
using Identity.Application.Exceptions;
using Identity.Application.Features.RoleManagement.Commands.AddRoleClaim;
using Identity.Domain.Entities;
using Identity.Shared.Constants;
using Identity.Shared.DTO;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Identity.Application.Features.RoleManagement.Queries.GetAllClaimsForARole;

public class GetAllClaimsForARoleQueryHandler : IRequestHandler<GetAllClaimsForARoleQuery, GetAllClaimsForARoleResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<GetAllClaimsForARoleQueryHandler> _logger;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public GetAllClaimsForARoleQueryHandler(UserManager<ApplicationUser> userManager,
        ILogger<GetAllClaimsForARoleQueryHandler> logger, IUserContext userContext,
        IResourceBaseAuthorizationService resourceBaseAuthorizationService,
        RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _logger = logger;
        _userContext = userContext;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
        _roleManager = roleManager;
    }
    public async Task<GetAllClaimsForARoleResponse> Handle(GetAllClaimsForARoleQuery request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.AdminAndAbove))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand!.Email,
                nameof(GetAllClaimsForARoleQuery),
                request);

            throw new CustomForbiddenException();
        }

        var getAllClaimsForARoleResponse = new GetAllClaimsForARoleResponse();
        getAllClaimsForARoleResponse.GetAllClaimsForARoleResponseDto = new GetAllClaimsForARoleResponseDto();

        var roleToGetClaim = await _roleManager.FindByIdAsync(request.RoleId.ToString());

        if (roleToGetClaim is null)
        {
            _logger.LogWarning("User with Id {UserId} tried to add claim {Claim} to non-existing role {roleId}",
                userExecutingCommand!.Email,
                nameof(AddRoleClaimCommand),
                request.RoleId);

            throw new CustomBadRequestException();
        }

        var existingClaims = await _roleManager.GetClaimsAsync(roleToGetClaim);

        foreach (var claim in existingClaims)
        {
            if (!getAllClaimsForARoleResponse.GetAllClaimsForARoleResponseDto.RoleClaims.ContainsKey(claim.Type))
            {
                getAllClaimsForARoleResponse.GetAllClaimsForARoleResponseDto.RoleClaims.Add(claim.Type.ToString(), new List<string> { claim.Value.ToString() });
            }
            else
            {
                getAllClaimsForARoleResponse.GetAllClaimsForARoleResponseDto.RoleClaims[claim.Type.ToString()].Add(claim.Value.ToString());
            }
        }

        _logger.LogInformation("Admin {AdminEmail} retrieved claims for Role with Id {RoleId}",
        userExecutingCommand!.Email,
            roleToGetClaim.Name);

        getAllClaimsForARoleResponse.GetAllClaimsForARoleResponseDto.RoleId = roleToGetClaim.Id;
        getAllClaimsForARoleResponse.GetAllClaimsForARoleResponseDto.RoleName = roleToGetClaim.Name!;
        getAllClaimsForARoleResponse.GetAllClaimsForARoleResponseDto.RoleClaims = getAllClaimsForARoleResponse.GetAllClaimsForARoleResponseDto.RoleClaims.OrderBy(e => e.Key).ToDictionary();

        getAllClaimsForARoleResponse.Success = true;
        getAllClaimsForARoleResponse.Message = "Successfully retrieved claims for User";

        return getAllClaimsForARoleResponse;
    }
}
