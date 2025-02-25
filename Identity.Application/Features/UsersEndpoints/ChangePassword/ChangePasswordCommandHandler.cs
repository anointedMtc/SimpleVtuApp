using Identity.Application.Exceptions;
using Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Interfaces;

namespace Identity.Application.Features.UsersEndpoints.ChangePassword;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, ChangePasswordResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<ChangePasswordCommandHandler> _logger;
    private readonly IUserContext _userContext;

    public ChangePasswordCommandHandler(UserManager<ApplicationUser> userManager,
        ILogger<ChangePasswordCommandHandler> logger, IUserContext userContext)
    {
        _userManager = userManager;
        _logger = logger;
        _userContext = userContext;
    }
    public async Task<ChangePasswordResponse> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        // this is the endpoint individual users should hit to update their details by themselves... since it makes use of context to automatically get the user... it is assumed it would be a link on the page or somewhere... 
        var changePasswordResponse = new ChangePasswordResponse();

        var user = _userContext.GetCurrentUser();

        _logger.LogInformation("Updating user: {UserId}, with {@Request}", user!.Id, request.ChangePasswordRequestDto);

        var dbUser = await _userManager.FindByIdAsync(user!.Id);

        if (dbUser == null)
        {
            throw new CustomNotFoundException(nameof(ApplicationUser), user!.Id);
        }

        var result = await _userManager.ChangePasswordAsync(dbUser, request.ChangePasswordRequestDto.CurrentPassword, request.ChangePasswordRequestDto.NewPassword);
        if (!result.Succeeded)
        {
            changePasswordResponse.Success = false;
            changePasswordResponse.Message = "Bad Request";

            throw new CustomInternalServerException("Something went wrong. Please try again after some time");
        }


        changePasswordResponse.Success = true;
        changePasswordResponse.Message = "Password reset was successful";

        
        return changePasswordResponse;
    }
}
