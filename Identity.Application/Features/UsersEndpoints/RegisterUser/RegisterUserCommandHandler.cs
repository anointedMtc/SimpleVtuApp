using AutoMapper;
using Identity.Application.Interfaces;
using Identity.Domain.Entities;
using Identity.Shared.Constants;
using Identity.Shared.IntegrationEvents;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Notification.Shared.DTO;
using SharedKernel.Application.Interfaces;
using System.Text;
using System.Text.Encodings.Web;

namespace Identity.Application.Features.UsersEndpoints.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterUserResponse>
{
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMassTransitService _massTransitService;

    // https://localhost:7287/api/v1/Account/emailConfirmation?Email=judeugwuanyi%40gmail.com&Token=Q
    private static readonly string _emailConfirmationEndpoint = $"https://localhost:7287/api/v1/Account/emailconfirmation";            // it's job is just to supply the emailConfirmation endpoint link and to that would be attached token and email which are needed by that endpoint... so when you click it, it automatically invokes the endpoint and confirms your email for you

    public RegisterUserCommandHandler(IMapper mapper,
        ILogger<RegisterUserCommandHandler> logger, 
        UserManager<ApplicationUser> userManager,
        IMassTransitService massTransitService)
    {
        _mapper = mapper;
        _logger = logger;
        _userManager = userManager;
        _massTransitService = massTransitService;
    }
    public async Task<RegisterUserResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var registerUserResponse = new RegisterUserResponse();

        _logger.LogInformation("Registering user");
        var existingUser = await _userManager.FindByEmailAsync(request.UserForRegisteration.Email!);
        if (existingUser != null)
        {
            _logger.LogError("Email already exits");    // logging such type of info helps us when debugging our application
            throw new Exception("Email already exists");
        }

        request.UserForRegisteration.Gender = request.UserForRegisteration.Gender.ToLower();
        request.UserForRegisteration.Nationality = request.UserForRegisteration.Nationality!.ToLower();

        var newUser = _mapper.Map<ApplicationUser>(request.UserForRegisteration);

        newUser.ConstUserName = GenerateConstUserName(request.UserForRegisteration.FirstName!, request.UserForRegisteration.LastName!);

        var result = await _userManager.CreateAsync(newUser, request.UserForRegisteration.Password!);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            _logger.LogError("Failed to create user: {errors}", errors);
            throw new Exception($"Failed to create user: {errors}");
        }

        _logger.LogInformation("User created successfully");
        registerUserResponse.UserId = newUser.Id;
        registerUserResponse.Success = true;
        registerUserResponse.Message = $"Kindly confirm your email to complete the registeration process. Check your Email for more info.   Your resource was created successfully at https://localhost:7023/api/ManageAccount/get-user-by-id/{registerUserResponse.UserId}";
        newUser.CreatedAt = DateTime.Now;
        newUser.UpdatedAt = DateTime.Now;

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

        string validToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));        // you can use ASCII or UTF8

        var callbackUrl = $"{_emailConfirmationEndpoint}?Email={request.UserForRegisteration.Email}&Token={validToken}";


        //var message = new EmailDto(request.UserForRegisteration.Email!, "Email Confirmation Token", $"Dear Subscriber, <br><br>Please confirm your Email account by <a href={HtmlEncoder.Default.Encode(callbackUrl)}>clicking here</a>.  <br><br> You can as well choose to copy your Token below and paste in appropriate apiEndpoint: <br><br> {HtmlEncoder.Default.Encode(validToken)} <br><br> If however you didn't make this request, kindly ignore. <br><br> Thanks <br><br> anointedMtc");
        //await _emailService.Send(message);
        await _massTransitService.Publish(new NewApplicationUserRegisteredEvent(request.UserForRegisteration.Email!, newUser.FirstName, callbackUrl, validToken));


        await _userManager.SetTwoFactorEnabledAsync(newUser, request.UserForRegisteration.IsTwoFacAuthEnabled);

        await _userManager.AddToRoleAsync(newUser, AppUserRoles.StandardUser);

        return registerUserResponse;
    }







    private string GenerateConstUserName(string firstName, string lastName)
    {
        var baseUserName = $"{firstName}_{lastName}".ToLower();

        var constUserName = baseUserName;
        var count = 1;
        while (_userManager.Users.Any(u => u.ConstUserName == constUserName))
        {
            constUserName = $"{baseUserName}_{count}";
            count++;
        }
        return constUserName;
    }
}
