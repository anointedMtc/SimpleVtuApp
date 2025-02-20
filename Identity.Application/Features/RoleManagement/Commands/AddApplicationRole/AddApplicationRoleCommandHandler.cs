using ApplicationSharedKernel.Interfaces;
using AutoMapper;
using Identity.Application.Exceptions;
using Identity.Domain.Entities;
using Identity.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Identity.Application.Features.RoleManagement.Commands.AddApplicationRole;

public class AddApplicationRoleCommandHandler : IRequestHandler<AddApplicationRoleCommand, AddApplicationRoleResponse>
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ILogger<AddApplicationRoleCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;
    private readonly IUserContext _userContext;

    public AddApplicationRoleCommandHandler(RoleManager<ApplicationRole> roleManager,
        ILogger<AddApplicationRoleCommandHandler> logger, IMapper mapper,
        IResourceBaseAuthorizationService resourceBaseAuthorizationService,
        IUserContext userContext)
    {
        _roleManager = roleManager;
        _logger = logger;
        _mapper = mapper;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
        _userContext = userContext;
    }

    public async Task<AddApplicationRoleResponse> Handle(AddApplicationRoleCommand request, CancellationToken cancellationToken)
    {
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.Create))
        {
            var userExecutingCommand = _userContext.GetCurrentUser();
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand!.Email,
                typeof(AddApplicationRoleCommand),
                request);

            throw new CustomForbiddenException();
        }

        var addApplicationRoleResponse = new AddApplicationRoleResponse();

        // check if role aready exists
        if (await _roleManager.RoleExistsAsync(request.AddApplicationRoleRequestDto.NormalizedName))
        {
            _logger.LogError("Role {Role} already exists", request.AddApplicationRoleRequestDto.Name);

            addApplicationRoleResponse.Success = false;
            addApplicationRoleResponse.Message = "Bad Request. Role already exits";

            throw new CustomBadRequestException("Role exits already");
        }

        var newRole = _mapper.Map<ApplicationRole>(request.AddApplicationRoleRequestDto);

        var result = await _roleManager.CreateAsync(newRole);

        if (!result.Succeeded)
        {
            _logger.LogError("Failed to create new role {Role}", request.AddApplicationRoleRequestDto.Name);

            addApplicationRoleResponse.Success = false;
            addApplicationRoleResponse.Message = "Failed to create new role. Something went wrong";

            throw new CustomInternalServerException("An Error occured while processing your request.\r\n The support team is notified and we are working on the fix. In case it's urgent, please contact us on info@anointedMtc.com");
        }

        addApplicationRoleResponse.Success = true;
        addApplicationRoleResponse.Message = "Successfully created new Role";

        return addApplicationRoleResponse;
    }
}
