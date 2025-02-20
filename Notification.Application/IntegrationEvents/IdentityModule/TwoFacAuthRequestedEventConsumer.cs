using AutoMapper;
using DomainSharedKernel.Interfaces;
using Identity.Shared.IntegrationEvents;
using MassTransit;
using Microsoft.Extensions.Logging;
using Notification.Application.Interfaces;
using Notification.Domain.Entities;
using Notification.Shared.DTO;

namespace Notification.Application.IntegrationEvents.IdentityModule;

public class TwoFacAuthRequestedEventConsumer : IConsumer<TwoFacAuthRequestedEvent>
{
    private readonly ILogger<TwoFacAuthRequestedEventConsumer> _logger;
    private readonly IEmailService _emailService;
    private readonly IRepository<EmailEntity> _emailRepository;
    private readonly IMapper _mapper;

    public TwoFacAuthRequestedEventConsumer(
        ILogger<TwoFacAuthRequestedEventConsumer> logger, 
        IEmailService emailService, 
        IRepository<EmailEntity> emailRepository, IMapper mapper)
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

        var message = new EmailDto(context.Message.Email!, "Authentication token", $"Dear {context.Message.FirstName}, " +
            $"<br>" +
            $"<br> Kindly use this OTP to complete your login request.<br><br> {context.Message.TokenFor2Fac}" +
            $"<br><br> But if you didn't request for this, kindly ignore. " +
            $"<br><br> Thanks. <br><br> anointedMtc");

        await _emailService.Send(message);

        var emailToSave = _mapper.Map<EmailEntity>(message);
        emailToSave.EventType = nameof(TwoFacAuthRequestedEventConsumer);
        await _emailRepository.AddAsync(emailToSave);

        _logger.LogInformation("Successfully sent and saved email to User with Id {UserId} by {typeOfEvent} at {Time}",
            context.Message.Email,
            nameof(TwoFacAuthRequestedEventConsumer),
            DateTimeOffset.UtcNow
        );
    }
}
