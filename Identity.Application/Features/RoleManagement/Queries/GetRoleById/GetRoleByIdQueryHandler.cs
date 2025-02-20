using ApplicationSharedKernel.Interfaces;
using AutoMapper;
using Identity.Application.Exceptions;
using Identity.Domain.Entities;
using Identity.Shared.Constants;
using Identity.Shared.DTO;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Identity.Application.Features.RoleManagement.Queries.GetRoleById;

public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, GetRoleByIdResponse>
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ILogger<GetRoleByIdQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;
    private readonly IUserContext _userContext;

    public GetRoleByIdQueryHandler(RoleManager<ApplicationRole> roleManager,
        ILogger<GetRoleByIdQueryHandler> logger, IMapper mapper, IResourceBaseAuthorizationService resourceBaseAuthorizationService, IUserContext userContext)
    {
        _roleManager = roleManager;
        _logger = logger;
        _mapper = mapper;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
        _userContext = userContext;
    }
    public async Task<GetRoleByIdResponse> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.AdminAndAbove))
        {
            var userExecutingCommand = _userContext.GetCurrentUser();
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand!.Email,
                typeof(GetRoleByIdQuery),
                request);

            throw new CustomForbiddenException();
        }

        var getRoleByIdResponse = new GetRoleByIdResponse();

        _logger.LogInformation("Getting role by id");
        var role = await _roleManager.FindByIdAsync(request.RoleId.ToString());
        if (role == null)
        {
            _logger.LogError("Role {Role} not found", request.RoleId);

            getRoleByIdResponse.Success = false;
            getRoleByIdResponse.Message = "Bad Request";

            throw new CustomBadRequestException();
        }

        _logger.LogInformation("Role {Role} found", request.RoleId);
        getRoleByIdResponse.ApplicationRoleResponseDto = _mapper.Map<ApplicationRoleResponseDto>(role);
        getRoleByIdResponse.Success = true;
        getRoleByIdResponse.Message = $"This ApplicationRole matched your search";

        return getRoleByIdResponse;
    }
}
