using ApplicationSharedKernel.Interfaces;
using AutoMapper;
using Identity.Application.Exceptions;
using Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Identity.Application.Features.UsersEndpoints.UpdateUserDetails;

public class UpdateUserDetailsCommandHandler(ILogger<UpdateUserDetailsCommandHandler> _logger,
    IUserContext _userContext, IMapper _mapper,
    IUserStore<ApplicationUser> _userStore) : IRequestHandler<UpdateUserDetailsCommand, UpdateUserDetailsResponse>
{
    public async Task<UpdateUserDetailsResponse> Handle(UpdateUserDetailsCommand request, CancellationToken cancellationToken)
    {
        // this is the endpoint individual users should hit to update their details by themselves... since it makes use of context to automatically get the user... it is assumed it would be a link on the page or somewhere... 
        var updateUserDetailsResponse = new UpdateUserDetailsResponse();

        var user = _userContext.GetCurrentUser();

        _logger.LogInformation("Updating user: {UserId}, with {@Request}", user!.Id, request);

        var dbUser = await _userStore.FindByIdAsync(user!.Id, cancellationToken);

        if (dbUser == null)
        {
            throw new CustomNotFoundException(nameof(ApplicationUser), user!.Id);
        }
        //for logging
        var formerDetails = dbUser;

        request.UpdateUserRequestDto.Nationality = request.UpdateUserRequestDto.Nationality!.ToLower();
        request.UpdateUserRequestDto.Gender = request.UpdateUserRequestDto.Gender.ToLower();

        _mapper.Map(request.UpdateUserRequestDto, dbUser);
        dbUser.UpdatedAt = DateTime.Now;

        var result = await _userStore.UpdateAsync(dbUser, cancellationToken);

        if (!result.Succeeded)
        {
            _logger.LogError("User update failed");

            updateUserDetailsResponse.Success = false;
            updateUserDetailsResponse.Message = "User update failed please try again later";

            throw new CustomInternalServerException("Something went wrong. Please try again after some time");
        }

        _logger.LogInformation("User {UserId} was updated successfully from {@FormerDetails} to {@CurrentDetials}",
            dbUser.Email,
            formerDetails,
            dbUser);

        updateUserDetailsResponse.Success = true;
        updateUserDetailsResponse.Message = $"Successfully updated User";

        return updateUserDetailsResponse;
    }
}