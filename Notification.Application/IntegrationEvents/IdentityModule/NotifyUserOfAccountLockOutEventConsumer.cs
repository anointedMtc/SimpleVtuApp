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

public class NotifyUserOfAccountLockOutEventConsumer : IConsumer<NotifyUserOfAccountLockOutEvent>
{
    private readonly ILogger<NotifyUserOfAccountLockOutEventConsumer> _logger;
    private readonly IEmailService _emailService;
    private readonly IEmailRepository<EmailEntity> _emailRepository;
    private readonly IMapper _mapper;

    public NotifyUserOfAccountLockOutEventConsumer(
        ILogger<NotifyUserOfAccountLockOutEventConsumer> logger, 
        IEmailService emailService, IEmailRepository<EmailEntity> emailRepository,
        IMapper mapper)
    {
        _logger = logger;
        _emailService = emailService;
        _emailRepository = emailRepository;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<NotifyUserOfAccountLockOutEvent> context)
    {
        _logger.LogInformation("Sending email to User with Id {UserId} by {typeOfEvent} at {Time}",
            context.Message.Email,
            nameof(NotifyUserOfAccountLockOutEventConsumer),
            DateTimeOffset.UtcNow
        );

        var message = new EmailDto(context.Message.Email, "Account Lock-Out Notification", $"Dear {context.Message.FirstName}, " +
            $"<br><br> Your Account is Locked Due to Multiple Invalid login Attempts." +
            $"<br>" +
            $"<br> If you want to reset the password, you can use the Forgot Password link on the Login Page" +
            $"<br>" +
            $"<br><br> You can always get in touch with our support team which is active 24/7 incase you need any assistance. " +
            $"<br><br> Thanks <br><br> anointedMtc");

        await _emailService.Send(message);

        var emailToSave = _mapper.Map<EmailEntity>(message);
        emailToSave.EventType = nameof(NotifyUserOfAccountLockOutEventConsumer);
        await _emailRepository.AddAsync(emailToSave);

        _logger.LogInformation("Successfully sent and saved email to User with Id {UserId} by {typeOfEvent} at {Time}",
            context.Message.Email,
            nameof(NotifyUserOfAccountLockOutEventConsumer),
            DateTimeOffset.UtcNow
        );
    }
}
