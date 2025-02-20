using AutoMapper;
using DomainSharedKernel.Interfaces;
using Identity.Shared.IntegrationEvents;
using MassTransit;
using Microsoft.Extensions.Logging;
using Notification.Application.Interfaces;
using Notification.Domain.Entities;
using Notification.Shared.DTO;
using System.Text.Encodings.Web;

namespace Notification.Application.IntegrationEvents.IdentityModule;

public class ForgotPasswordRequestedEventConsumer : IConsumer<ForgotPasswordRequestedEvent>
{
    private readonly ILogger<ForgotPasswordRequestedEventConsumer> _logger;
    private readonly IEmailService _emailService;
    private readonly IRepository<EmailEntity> _emailRepository;
    private readonly IMapper _mapper;

    public ForgotPasswordRequestedEventConsumer(
        ILogger<ForgotPasswordRequestedEventConsumer> logger, 
        IEmailService emailService, IRepository<EmailEntity> emailRepository, 
        IMapper mapper)
    {
        _logger = logger;
        _emailService = emailService;
        _emailRepository = emailRepository;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<ForgotPasswordRequestedEvent> context)
    {
        _logger.LogInformation("Sending email to User with Id {UserId} by {typeOfEvent} at {Time}",
            context.Message.Email,
            nameof(ForgotPasswordRequestedEventConsumer),
            DateTimeOffset.UtcNow
        );

        var message = new EmailDto(context.Message.Email!, "Forgot Password Request", $"Dear {context.Message.FirstName}, " +
            $"<br>" +
            $"<br> If you are consuming this through a FrontEnd client, Please reset your password using the call back link by <a href={HtmlEncoder.Default.Encode(context.Message.CallBackUrl)}>clicking here</a>. " +
            $"<br>" +
            $"<br> Else Here is your token <br><br> {HtmlEncoder.Default.Encode(context.Message.Token)} " +
            $"<br>" +
            $"<br> If however, you didn't make this request, kindly ignore. " +
            $"<br><br> Thanks. <br><br> anointedMtc");

        await _emailService.Send(message);

        var emailToSave = _mapper.Map<EmailEntity>(message);
        emailToSave.EventType = nameof(ForgotPasswordRequestedEventConsumer);
        await _emailRepository.AddAsync(emailToSave);

        _logger.LogInformation("Successfully sent and saved email to User with Id {UserId} by {typeOfEvent} at {Time}",
            context.Message.Email,
            nameof(ForgotPasswordRequestedEventConsumer),
            DateTimeOffset.UtcNow
        );
    }
}
