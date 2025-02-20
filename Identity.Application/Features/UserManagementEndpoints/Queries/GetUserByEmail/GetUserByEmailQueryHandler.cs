using ApplicationSharedKernel.Interfaces;
using AutoMapper;
using Identity.Application.Exceptions;
using Identity.Domain.Entities;
using Identity.Shared.Constants;
using Identity.Shared.DTO;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Identity.Application.Features.UserManagementEndpoints.Queries.GetUserByEmail;

public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, GetUserByEmailResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;
    private readonly IUserContext _userContext;

    public GetUserByEmailQueryHandler(UserManager<ApplicationUser> userManager,
        ILogger<GetUserByEmailQueryHandler> logger, IMapper mapper,
        IResourceBaseAuthorizationService resourceBaseAuthorizationService,
        IUserContext userContext)
    {
        _userManager = userManager;
        _logger = logger;
        _mapper = mapper;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
        _userContext = userContext;
    }
    public async Task<GetUserByEmailResponse> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.AdminAndAbove))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand!.Email,
                typeof(GetUserByEmailQuery),
                request);

            throw new CustomForbiddenException("Access Denied. You do not have Permission to view this resource");
        }

        var getUserByEmailResponse = new GetUserByEmailResponse();
        getUserByEmailResponse.UserResponseDto = new ApplicationUserResponseDto();

        _logger.LogInformation("Getting user by id");
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            _logger.LogError("User not found");

            getUserByEmailResponse.Success = false;
            getUserByEmailResponse.Message = "Bad Request";

            throw new CustomBadRequestException();
        }

        _logger.LogInformation("User found");
        getUserByEmailResponse.UserResponseDto = _mapper.Map<ApplicationUserResponseDto>(user);
        getUserByEmailResponse.Success = true;
        getUserByEmailResponse.Message = $"This User matched your search";

        return getUserByEmailResponse;
    }
}
