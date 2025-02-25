using AutoMapper;
using Identity.Application.Exceptions;
using Identity.Domain.Entities;
using Identity.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Interfaces;

namespace Identity.Application.Features.RoleManagement.Commands.UpdateApplicationRole;

public class UpdateApplicationRoleCommandHandler : IRequestHandler<UpdateApplicationRoleCommand, UpdateApplicationRoleResponse>
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ILogger<UpdateApplicationRoleCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;
    private readonly IUserContext _userContext;

    public UpdateApplicationRoleCommandHandler(RoleManager<ApplicationRole> roleManager,
        ILogger<UpdateApplicationRoleCommandHandler> logger, IMapper mapper,
        IResourceBaseAuthorizationService resourceBaseAuthorizationService, IUserContext userContext)
    {
        _roleManager = roleManager;
        _logger = logger;
        _mapper = mapper;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
        _userContext = userContext;
    }

    public async Task<UpdateApplicationRoleResponse> Handle(UpdateApplicationRoleCommand request, CancellationToken cancellationToken)
    {
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.Update))
        {
            var userExecutingCommand = _userContext.GetCurrentUser();
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand!.Email,
                typeof(UpdateApplicationRoleCommand),
                request);

            throw new CustomForbiddenException();
        }

        var updateApplicationRoleResponse = new UpdateApplicationRoleResponse();

        var existingRole = await _roleManager.FindByIdAsync(request.RoleId.ToString());

        if (existingRole == null)
        {
            _logger.LogError("Role {Role} does not exist", request.UpdateApplicationRoleRequestDto.Name);
            updateApplicationRoleResponse.Success = false;
            updateApplicationRoleResponse.Message = "Bad Request";

            throw new CustomBadRequestException("Bad Request");
        }

        _mapper.Map(request.UpdateApplicationRoleRequestDto, existingRole);

        var result = await _roleManager.UpdateAsync(existingRole);

        if (!result.Succeeded)
        {
            _logger.LogError("Failed to update role {Role}",
                request.UpdateApplicationRoleRequestDto.Name);

            updateApplicationRoleResponse.Success = false;
            updateApplicationRoleResponse.Message = "Failed to update role. Something went wrong";

            throw new CustomInternalServerException("Failed to Update role. Please try again later");
        }

        updateApplicationRoleResponse.Success = true;
        updateApplicationRoleResponse.Message = "Successfully updated role";

        return updateApplicationRoleResponse;
    }
}
