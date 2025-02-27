using AutoMapper;
using Identity.Shared.IntegrationEvents;
using MassTransit;
using Microsoft.Extensions.Logging;
using Notification.Application.Interfaces;
using Notification.Domain.Entities;
using Notification.Domain.Interfaces;
using Notification.Shared.DTO;
using SharedKernel.Domain.Interfaces;
using System.Text.Encodings.Web;

namespace Notification.Application.IntegrationEvents.IdentityModule;

public sealed class ChangeEmailRequestedEventConsumer : IConsumer<ChangeEmailRequestedEvent>
{
    private readonly ILogger<ChangeEmailRequestedEventConsumer> _logger;
    private readonly IEmailService _emailService;
    private readonly IEmailRepository<EmailEntity> _emailRepository;
    private readonly IMapper _mapper;

    public ChangeEmailRequestedEventConsumer(
        ILogger<ChangeEmailRequestedEventConsumer> logger, 
        IEmailService emailService, IEmailRepository<EmailEntity> emailRepository,
        IMapper mapper)
    {
        _logger = logger;
        _emailService = emailService;
        _emailRepository = emailRepository;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<ChangeEmailRequestedEvent> context)
    {
        // To the new Email
        _logger.LogInformation("Sending email to User with Id {UserId} by {typeOfEvent} at {Time}",
            context.Message.NewEmail,
            nameof(ChangeEmailRequestedEventConsumer),
            DateTimeOffset.UtcNow
        );

        var message = new EmailDto(context.Message.NewEmail!, "Change Email Request Token", $"Dear {context.Message.FirstName}, " +
            $"<br><br> If you are consuming this through a FrontEnd client, Please confirm the change of email request by using the call back link by <a href={HtmlEncoder.Default.Encode(context.Message.CallBackUrl)}>clicking here</a>. " +
            $"<br>" +
            $"<br> Else Here is your token <br><br> {HtmlEncoder.Default.Encode(context.Message.Token)} " +
            $"<br>" +
            $"<br> If however, you didn't make this request, kindly ignore. " +
            $"<br>" +
            $"<br> Thanks. <br><br> anointedMtc");

        await _emailService.Send(message);

        var emailToSave = _mapper.Map<EmailEntity>(message);
        emailToSave.EventType = nameof(ChangeEmailRequestedEventConsumer);
        await _emailRepository.AddAsync(emailToSave);

        _logger.LogInformation("Successfully sent and saved email to User with Id {UserId} by {typeOfEvent} at {Time}",
            context.Message.NewEmail,
            nameof(ChangeEmailRequestedEventConsumer),
            DateTimeOffset.UtcNow
        );



        // To the Old Email
        _logger.LogInformation("Sending email to User with Id {UserId} by {typeOfEvent} at {Time}",
            context.Message.OldEmail,
            nameof(ChangeEmailRequestedEventConsumer),
            DateTimeOffset.UtcNow
        );

        var messageTwo = new EmailDto(context.Message.OldEmail, "Change of Email Request", $"Dear {context.Message.FirstName}, " +
            $"<br>" +
            $"<br> A request was made to change your email with us from this email {context.Message.OldEmail} to another email." +
            $"<br>" +
            $"<br> If it wasn't you, kindly reach out to us at <br><br> info@anointedMtc.com   " +
            $"<br>" +
            $"<br>  Thanks.  <br><br> anointedMtc");

        await _emailService.Send(messageTwo);

        var emailToSaveTwo = _mapper.Map<EmailEntity>(messageTwo);
        emailToSaveTwo.EventType = nameof(ChangeEmailRequestedEventConsumer);
        await _emailRepository.AddAsync(emailToSaveTwo);

        _logger.LogInformation("Successfully sent and saved email to User with Id {UserId} by {typeOfEvent} at {Time}",
            context.Message.OldEmail,
            nameof(ChangeEmailRequestedEventConsumer),
            DateTimeOffset.UtcNow
        );
    }
}
