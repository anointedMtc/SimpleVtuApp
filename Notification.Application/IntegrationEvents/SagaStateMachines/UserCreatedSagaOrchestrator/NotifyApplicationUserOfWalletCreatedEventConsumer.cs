using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using Notification.Application.Interfaces;
using Notification.Domain.Entities;
using Notification.Shared.DTO;
using SagaOrchestrationStateMachines.Shared.IntegrationEvents.UserCreatedSaga;
using SharedKernel.Domain.Interfaces;

namespace Notification.Application.IntegrationEvents.SagaStateMachines.UserCreatedSagaOrchestrator;

public sealed class NotifyApplicationUserOfWalletCreatedEventConsumer : IConsumer<NotifyApplicationUserOfWalletCreatedEvent>
{
    private readonly ILogger<NotifyApplicationUserOfWalletCreatedEventConsumer> _logger;
    private readonly IEmailService _emailService;
    private readonly IRepository<EmailEntity> _emailRepository;
    private readonly IMapper _mapper;

    public NotifyApplicationUserOfWalletCreatedEventConsumer(
        ILogger<NotifyApplicationUserOfWalletCreatedEventConsumer> logger, 
        IEmailService emailService, IRepository<EmailEntity> emailRepository, 
        IMapper mapper)
    {
        _logger = logger;
        _emailService = emailService;
        _emailRepository = emailRepository;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<NotifyApplicationUserOfWalletCreatedEvent> context)
    {
        _logger.LogInformation("Sending email to User with Id {UserId} by {typeOfEvent} at {Time} with {@Details}",
            context.Message.Email,
            nameof(NotifyApplicationUserOfWalletCreatedEvent),
            DateTimeOffset.UtcNow,
            context.Message
        );

        var message = new EmailDto(context.Message.Email!, "Wallet Created With Bonus Balance", $"Dear {context.Message.FirstName}, <br><br>A new Wallet has been created for you with a registeration bonus of <del>N</del> {context.Message.BonusBalance} naira.   <br><br>   To start enjoying our wonderful services, you can simply log-in to your account, Transfer your bonus to the main Wallet and start Vtu Transactions. <br><br>    Get in contact with our support team which is active 24/7 incase you need any assistance. <br><br> Thanks <br><br> anointedMtc");
        await _emailService.Send(message);

        var emailToSave = _mapper.Map<EmailEntity>(message);
        emailToSave.EventType = nameof(NotifyApplicationUserOfWalletCreatedEvent);
        await _emailRepository.AddAsync(emailToSave);

        _logger.LogInformation("Successfully sent and saved email to User with Id {UserId} by {typeOfEvent} at {Time}",
            context.Message.Email,
            nameof(NotifyApplicationUserOfWalletCreatedEvent),
            DateTimeOffset.UtcNow
        );
    }
}
