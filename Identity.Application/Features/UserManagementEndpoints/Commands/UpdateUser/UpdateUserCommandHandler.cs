using AutoMapper;
using Identity.Application.Exceptions;
using Identity.Domain.Entities;
using Identity.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Interfaces;

namespace Identity.Application.Features.UserManagementEndpoints.Commands.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UpdateUserResponse>
{
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;
    private readonly IUserContext _userContext;

    public UpdateUserCommandHandler(ILogger<UpdateUserCommandHandler> logger,
        UserManager<ApplicationUser> userManager, IMapper mapper,
        IUserContext userContext,
        IResourceBaseAuthorizationService resourceBaseAuthorizationService)
    {
        _logger = logger;
        _userManager = userManager;
        _mapper = mapper;
        _userContext = userContext;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
    }
    public async Task<UpdateUserResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        // this endpoint should be used by management since it does not automatically detect user through userContext
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.Update))
        {
            var userExecutingCommand = _userContext.GetCurrentUser();
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand!.Email,
                typeof(UpdateUserCommand),
                request);

            throw new CustomForbiddenException("Access Denied. You do not have Permission to view this resource");
        }

        var updateUserResponse = new UpdateUserResponse();

        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
        {
            _logger.LogError("User not found");
            throw new Exception("User not found");
        }

        request.UpdateUserRequestDto.Nationality = request.UpdateUserRequestDto.Nationality!.ToLower();
        request.UpdateUserRequestDto.Gender = request.UpdateUserRequestDto.Gender.ToLower();

        _mapper.Map(request.UpdateUserRequestDto, user);
        user.UpdatedAt = DateTime.Now;
        
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            _logger.LogError("User update failed");

            updateUserResponse.Success = false;
            updateUserResponse.Message = "User update failed please try again later";

            return updateUserResponse;
        }

        updateUserResponse.Success = true;
        updateUserResponse.Message = $"Successfully updated User";

        return updateUserResponse;
    }
}
