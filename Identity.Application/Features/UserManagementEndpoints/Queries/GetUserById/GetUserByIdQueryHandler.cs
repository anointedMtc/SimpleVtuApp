using AutoMapper;
using Identity.Application.Exceptions;
using Identity.Domain.Entities;
using Identity.Shared.Constants;
using Identity.Shared.DTO;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Interfaces;

namespace Identity.Application.Features.UserManagementEndpoints.Queries.GetUserById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, GetUserByIdResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;
    private readonly IUserContext _userContext;

    public GetUserByIdQueryHandler(UserManager<ApplicationUser> userManager,
        ILogger<GetUserByIdQueryHandler> logger, IMapper mapper,
        IResourceBaseAuthorizationService resourceBaseAuthorizationService,
        IUserContext userContext)
    {
        _userManager = userManager;
        _logger = logger;
        _mapper = mapper;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
        _userContext = userContext;
    }

    public async Task<GetUserByIdResponse> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.AdminAndAbove))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand!.Email,
                typeof(GetUserByIdQuery),
                request);

            throw new CustomForbiddenException("Access Denied. You do not have Permission to view this resource");
        }

        var getUserByIdResponse = new GetUserByIdResponse();
        getUserByIdResponse.UserResponseDto = new ApplicationUserResponseDto();

        _logger.LogInformation("Getting user by id");
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
        {
            _logger.LogError("User not found");

            getUserByIdResponse.Success = false;
            getUserByIdResponse.Message = "Bad Request";

            throw new CustomBadRequestException();
        }

        _logger.LogInformation("User found");
        getUserByIdResponse.UserResponseDto = _mapper.Map<ApplicationUserResponseDto>(user);
        getUserByIdResponse.Success = true;
        getUserByIdResponse.Message = $"This User matched your search";

        return getUserByIdResponse;
    }
}
