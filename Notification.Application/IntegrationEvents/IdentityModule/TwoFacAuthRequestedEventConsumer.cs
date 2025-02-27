using AutoMapper;
using Identity.Shared.IntegrationEvents;
using MassTransit;
using Microsoft.Extensions.Logging;
using Notification.Application.Interfaces;
using Notification.Domain.Entities;
using Notification.Domain.Interfaces;
using Notification.Shared.DTO;
using SharedKernel.Domain.Interfaces;

namespace Notification.Application.IntegrationEvents.IdentityModule;

public class TwoFacAuthRequestedEventConsumer : IConsumer<TwoFacAuthRequestedEvent>
{
    private readonly ILogger<TwoFacAuthRequestedEventConsumer> _logger;
    private readonly IEmailService _emailService;
    private readonly IEmailRepository<EmailEntity> _emailRepository;
    private readonly IMapper _mapper;

    public TwoFacAuthRequestedEventConsumer(
        ILogger<TwoFacAuthRequestedEventConsumer> logger, 
        IEmailService emailService,
        IEmailRepository<EmailEntity> emailRepository, IMapper mapper)
    {
        _logger = logger;
        _emailService = emailService;
        _emailRepository = emailRepository;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<TwoFacAuthRequestedEvent> context)
    {
        _logger.LogInformation("Sending email to User with Id {UserId} by {typeOfEvent} at {Time}",
            context.Message.Email,
            nameof(TwoFacAuthRequestedEventConsumer),
            DateTimeOffset.UtcNow
        );

        var receiver = context.Message.Email;
        var subject = "Authentication token";
        var emailBody = $"Dear {context.Message.FirstName}, " +
            $"<br>" +
            $"<br> Kindly use this OTP to complete your login request.<br><br> {context.Message.TokenFor2Fac}" +
            $"<br><br> But if you didn't request for this, kindly ignore. " +
            $"<br><br> Thanks. <br><br> anointedMtc";

        //var message = new EmailDto(context.Message.Email!, "Authentication token", $"Dear {context.Message.FirstName}, " +
        //    $"<br>" +
        //    $"<br> Kindly use this OTP to complete your login request.<br><br> {context.Message.TokenFor2Fac}" +
        //    $"<br><br> But if you didn't request for this, kindly ignore. " +
        //    $"<br><br> Thanks. <br><br> anointedMtc");

        var message = new EmailDto(receiver, subject, emailBody);

        await _emailService.Send(message);

        //var emailToSave = _mapper.Map<EmailEntity>(message);

        //var emailToSave = new EmailEntity(receiver, subject, emailBody);
        //emailToSave.EventType = nameof(TwoFacAuthRequestedEventConsumer);

        var emailToSave = new EmailEntity(receiver, subject, emailBody)
        {
            EventType = nameof(TwoFacAuthRequestedEventConsumer)
        };

        await _emailRepository.AddAsync(emailToSave);

        _logger.LogInformation("Successfully sent and saved email to User with Id {UserId} by {typeOfEvent} at {Time}",
            context.Message.Email,
            nameof(TwoFacAuthRequestedEventConsumer),
            DateTimeOffset.UtcNow
        );
    }
}
