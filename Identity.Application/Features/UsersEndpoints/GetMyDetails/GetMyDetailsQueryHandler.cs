using AutoMapper;
using Identity.Application.Exceptions;
using Identity.Domain.Entities;
using Identity.Shared.DTO;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Interfaces;

namespace Identity.Application.Features.UsersEndpoints.GetMyDetails;

public class GetMyDetailsQueryHandler : IRequestHandler<GetMyDetailsQuery, GetMyDetailsResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<GetMyDetailsQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;

    public GetMyDetailsQueryHandler(UserManager<ApplicationUser> userManager,
        ILogger<GetMyDetailsQueryHandler> logger, IMapper mapper, IUserContext userContext)
    {
        _userManager = userManager;
        _logger = logger;
        _mapper = mapper;
        _userContext = userContext;
    }
    public async Task<GetMyDetailsResponse> Handle(GetMyDetailsQuery request, CancellationToken cancellationToken)
    {
        // this is the endpoint individual users should hit to get their details by themselves... since it makes use of context to automatically get the user... it is assumed it would be a link on the page or somewhere... 
        var getMyDetailsResponse = new GetMyDetailsResponse();
        getMyDetailsResponse.UserResponseDto = new ApplicationUserResponseDto();

        var user = _userContext.GetCurrentUser();

        _logger.LogInformation("Fetching user details for: {UserId}", user!.Id);

        var dbUser = await _userManager.FindByIdAsync(user!.Id);

        if (dbUser == null)
        {
            _logger.LogWarning("Non-existing-User with Id {UserId} tried to fetch their data from the database",
                user.Email);

            getMyDetailsResponse.Success = false;
            getMyDetailsResponse.Message = "Bad Request";

            throw new CustomNotFoundException(nameof(ApplicationUser), user!.Id);
        }


        _logger.LogInformation("Successfully fetched user details for User with Id {UserId}",
            dbUser.Email);

        getMyDetailsResponse.UserResponseDto = _mapper.Map<ApplicationUserResponseDto>(dbUser);
        getMyDetailsResponse.Success = true;
        getMyDetailsResponse.Message = $"This User matched your search";

        return getMyDetailsResponse;
    }
}
