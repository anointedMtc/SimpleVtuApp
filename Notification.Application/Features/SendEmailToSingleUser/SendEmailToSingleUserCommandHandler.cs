using SharedKernel.Application.Exceptions;
using AutoMapper;
using Identity.Shared.Constants;
using MediatR;
using Microsoft.Extensions.Logging;
using Notification.Application.Interfaces;
using Notification.Domain.Entities;
using Notification.Shared.DTO;
using SharedKernel.Application.Interfaces;
using SharedKernel.Domain.Interfaces;
using Notification.Domain.Interfaces;

namespace Notification.Application.Features.SendEmailToSingleUser;

public class SendEmailToSingleUserCommandHandler : IRequestHandler<SendEmailToSingleUserCommand, SendEmailToSingleUserResponse>
{
    private readonly IEmailService _emailService;
    private readonly ILogger<SendEmailToSingleUserCommandHandler> _logger;
    private readonly IEmailRepository<EmailEntity> _emailRepository;
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;

    public SendEmailToSingleUserCommandHandler(IEmailService emailService, 
        ILogger<SendEmailToSingleUserCommandHandler> logger,
        IEmailRepository<EmailEntity> emailRepository, IMapper mapper, 
        IUserContext userContext, IResourceBaseAuthorizationService resourceBaseAuthorizationService)
    {
        _emailService = emailService;
        _logger = logger;
        _emailRepository = emailRepository;
        _mapper = mapper;
        _userContext = userContext;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
    }

    public async Task<SendEmailToSingleUserResponse> Handle(SendEmailToSingleUserCommand request, CancellationToken cancellationToken)
    {

        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.AdminAndAbove))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand!.Email,
                typeof(SendEmailToSingleUserCommand).Name,
                request);

            throw new ForbiddenAccessException();
        }



        var sendEmailToSingleUserResponse = new SendEmailToSingleUserResponse();

        _logger.LogInformation("Sending email to User with Id {UserId} by {admin} at {Time}", 
            request.EmailDto.ToAddress,
            userExecutingCommand!.Email,
            DateTimeOffset.UtcNow);

        var emailMetadata = new EmailDto(
            request.EmailDto.ToAddress,
            request.EmailDto.Subject,
            request.EmailDto.Body,
            request.EmailDto.AttachmentPath,
            request.EmailDto.CC,
            request.EmailDto.BCC,
            
            userExecutingCommand!.Email,
            //request.EmailDto.Sender,

            typeof(SendEmailToSingleUserCommand).Name
            //request.EmailDto.EventType
        );

        await _emailService.Send(emailMetadata);
        
        //var emailToSave = _mapper.Map<EmailEntity>(request.EmailDto);

        var emailToSave = new EmailEntity(
            request.EmailDto.ToAddress,
            request.EmailDto.Subject,
            request.EmailDto.Body,
            request.EmailDto.AttachmentPath,
            request.EmailDto.CC,
            request.EmailDto.BCC,
            request.EmailDto.Sender,
            request.EmailDto.EventType);

        await _emailRepository.AddAsync(emailToSave);

        sendEmailToSingleUserResponse.Success = true;
        sendEmailToSingleUserResponse.Message = $"Email Sent Successfully";

        return sendEmailToSingleUserResponse;

    }
}
