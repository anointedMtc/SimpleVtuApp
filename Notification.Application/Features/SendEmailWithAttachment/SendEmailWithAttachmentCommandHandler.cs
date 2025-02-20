using ApplicationSharedKernel.Exceptions;
using ApplicationSharedKernel.Interfaces;
using AutoMapper;
using DomainSharedKernel.Interfaces;
using Identity.Shared.Constants;
using MediatR;
using Microsoft.Extensions.Logging;
using Notification.Application.Features.SendEmailUsingLiquidSyntax;
using Notification.Application.Features.SendEmailWithRazorTemplate;
using Notification.Application.Interfaces;
using Notification.Domain.Entities;
using Notification.Shared.DTO;

namespace Notification.Application.Features.SendEmailWithAttachment;

public class SendEmailWithAttachmentCommandHandler : IRequestHandler<SendEmailWithAttachmentCommand, SendEmailWithAttachmentResponse>
{
    private readonly IEmailService _emailService;
    private readonly ILogger<SendEmailWithAttachmentCommandHandler> _logger;
    private readonly IRepository<EmailEntity> _emailRepository;
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;

    public SendEmailWithAttachmentCommandHandler(IEmailService emailService,
        ILogger<SendEmailWithAttachmentCommandHandler> logger,
        IRepository<EmailEntity> emailRepository, IMapper mapper,
        IUserContext userContext, IResourceBaseAuthorizationService resourceBaseAuthorizationService)
    {
        _emailService = emailService;
        _logger = logger;
        _emailRepository = emailRepository;
        _mapper = mapper;
        _userContext = userContext;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
    }
    public async Task<SendEmailWithAttachmentResponse> Handle(SendEmailWithAttachmentCommand request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.AdminAndAbove))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand!.Email,
                typeof(SendEmailWithAttachmentCommand).Name,
                request);

            throw new ForbiddenAccessException();
        }

        var sendEmailWithAttachmentResponse = new SendEmailWithAttachmentResponse();

        var emailMetadata = new EmailDto(
           request.EmailDto.ToAddress,
           request.EmailDto.Subject,

           // NOT REQUIRED
           //request.EmailDto.Body,
           //request.EmailDto.AttachmentPath,

           request.EmailDto.CC,
           request.EmailDto.BCC,

            userExecutingCommand!.Email,
           //request.EmailDto.Sender,

           typeof(SendEmailWithAttachmentCommand).Name
            //request.EmailDto.EventType
        );


        // sample of file path
        // $"{Directory.GetCurrentDirectory()}/EmailAttachment.txt");   // Path to the embedded resource eg [YourAssembly].[YourResourceFolder].[YourFilename.txt]

        await _emailService.SendWithAttachment(emailMetadata);

        var emailToSave = _mapper.Map<EmailEntity>(request.EmailDto);
        await _emailRepository.AddAsync(emailToSave);

        sendEmailWithAttachmentResponse.Success = true;
        sendEmailWithAttachmentResponse.Message = $"Email Sent Successfully";

        return sendEmailWithAttachmentResponse;
    }
}
