using AutoMapper;
using Identity.Application.Exceptions;
using Identity.Domain.Entities;
using Identity.Shared.Constants;
using Identity.Shared.DTO;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Interfaces;

namespace Identity.Application.Features.RoleManagement.Queries.GetRoleByName;

public class GetRoleByNameQueryHandler : IRequestHandler<GetRoleByNameQuery, GetRoleByNameResponse>
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ILogger<GetRoleByNameQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;
    private readonly IUserContext _userContext;

    public GetRoleByNameQueryHandler(RoleManager<ApplicationRole> roleManager,
        ILogger<GetRoleByNameQueryHandler> logger, IMapper mapper, IResourceBaseAuthorizationService resourceBaseAuthorizationService, IUserContext userContext)
    {
        _roleManager = roleManager;
        _logger = logger;
        _mapper = mapper;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
        _userContext = userContext;
    }
    public async Task<GetRoleByNameResponse> Handle(GetRoleByNameQuery request, CancellationToken cancellationToken)
    {
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.AdminAndAbove))
        {
            var userExecutingCommand = _userContext.GetCurrentUser();
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand!.Email,
                typeof(GetRoleByNameQuery),
                request);

            throw new CustomForbiddenException();
        }

        var getRoleByNameResponse = new GetRoleByNameResponse();

        _logger.LogInformation("Getting role by Name {Name}", request.Name);
        var role = await _roleManager.FindByNameAsync(request.Name);
        if (role == null)
        {
            _logger.LogError("Role {Role} not found", request.Name);

            getRoleByNameResponse.Success = false;
            getRoleByNameResponse.Message = "Bad Request";

            throw new CustomBadRequestException();
        }

        _logger.LogInformation("Role {Role} found", request.Name);
        getRoleByNameResponse.ApplicationRoleResponseDto = _mapper.Map<ApplicationRoleResponseDto>(role);
        getRoleByNameResponse.Success = true;
        getRoleByNameResponse.Message = $"This ApplicationRole matched your search";

        return getRoleByNameResponse;
    }
}
