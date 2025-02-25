using AutoMapper;
using Identity.Shared.Constants;
using MediatR;
using Microsoft.Extensions.Logging;
using Notification.Application.Interfaces;
using Notification.Domain.Entities;
using Notification.Shared.DTO;
using SharedKernel.Application.Exceptions;
using SharedKernel.Application.Interfaces;
using SharedKernel.Domain.Interfaces;

namespace Notification.Application.Features.SendEmailUsingLiquidSyntax;

public class SendEmailUsingLiquidSyntaxCommandHandler : IRequestHandler<SendEmailUsingLiquidSyntaxCommand, SendEmailUsingLiquidSyntaxResponse>
{
    private readonly IEmailService _emailService;
    private readonly ILogger<SendEmailUsingLiquidSyntaxCommandHandler> _logger;
    private readonly IRepository<EmailEntity> _emailRepository;
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;

    public SendEmailUsingLiquidSyntaxCommandHandler(IEmailService emailService,
        ILogger<SendEmailUsingLiquidSyntaxCommandHandler> logger,
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
    public async Task<SendEmailUsingLiquidSyntaxResponse> Handle(SendEmailUsingLiquidSyntaxCommand request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.AdminAndAbove))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand!.Email,
                typeof(SendEmailUsingLiquidSyntaxCommand).Name,
                request);

            throw new ForbiddenAccessException();
        }

        var sendEmailUsingLiquidSyntaxResponse = new SendEmailUsingLiquidSyntaxResponse();

        var liquidModel = new UserEmailDto(
            request.LiquidModel.Name,
            request.LiquidModel.Email,
            request.LiquidModel.MemberType);

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

           typeof(SendEmailUsingLiquidSyntaxCommand).Name
            //request.EmailDto.EventType
       );

        var defaultTemplate = @"Dear <b>{{ Name }}</b>,</br>
            Thank you for being an esteemed <b>{{ MemberType }}</b> member.";

        var template = request.LiquidSyntax ?? defaultTemplate;

        // you can do it like this also if you want to write the razorSyntax directly
        //var template = "Dear <b>@Model.Name</b>, </br>" +
        //        "Thank you for being an esteemed <b>@Model.MemberType</b> member.";

        await _emailService.SendUsingLiqTemplate(emailMetadata, template, liquidModel);



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

        sendEmailUsingLiquidSyntaxResponse.Success = true;
        sendEmailUsingLiquidSyntaxResponse.Message = $"Email Sent Successfully";

        return sendEmailUsingLiquidSyntaxResponse;
    }
}
