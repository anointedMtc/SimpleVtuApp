using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using Notification.Application.IntegrationEvents.SagaStateMachines.VtuAirtimeOrderedSagaOrchestrator;
using Notification.Application.Interfaces;
using Notification.Domain.Entities;
using Notification.Domain.Interfaces;
using Notification.Shared.DTO;
using SagaOrchestrationStateMachines.Shared.IntegrationEvents.VtuAirtimeSaga;
using SagaOrchestrationStateMachines.Shared.IntegrationEvents.VtuDataSaga;
using SharedKernel.Domain.Interfaces;

namespace Notification.Application.IntegrationEvents.SagaStateMachines.VtuDataOrderedSagaOrchestrator;

public sealed class NotifyCustomerOfVtuDataPurchaseFailedEventConsumer
    : IConsumer<NotifyCustomerOfVtuDataPurchaseFailedEvent>
{
    private readonly ILogger<NotifyCustomerOfVtuDataPurchaseFailedEventConsumer> _logger;
    private readonly IEmailService _emailService;
    private readonly IEmailRepository<EmailEntity> _emailRepository;
    private readonly IMapper _mapper;

    public NotifyCustomerOfVtuDataPurchaseFailedEventConsumer(
        ILogger<NotifyCustomerOfVtuDataPurchaseFailedEventConsumer> logger, 
        IEmailService emailService, IEmailRepository<EmailEntity> emailRepository, 
        IMapper mapper)
    {
        _logger = logger;
        _emailService = emailService;
        _emailRepository = emailRepository;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<NotifyCustomerOfVtuDataPurchaseFailedEvent> context)
    {
        _logger.LogInformation("Sending email to User with Id {UserId} by {typeOfEvent} at {Time} with {@Details}",
            context.Message.Email,
            nameof(NotifyCustomerOfVtuDataPurchaseFailedEvent),
            DateTimeOffset.UtcNow,
            context.Message
        );

        var message = new EmailDto(context.Message.Email!, "Data Purchase Failed", $"Dear {context.Message.FirstName}, " +
            $"<br><br> We wish to inform you that your Data Purchase transaction with Id {context.Message.VtuTransactionId} failed." +
            $"However, immediate refund of {context.Message.PricePaid} has been made to your Vtu Account Balance  " +
            $"<br><br> Details of this transaction are as follows:" +
            $"<br>" +
            $"<br> NetworkProvider: {context.Message.NetworkProvider}," +
            $"<br> DataPlanName: {context.Message.DataPlanPurchased}" +
            $"<br> Value: {context.Message.AmountPurchased}" +
            $"<br> Cost: {context.Message.PricePaid}" +
            $"<br> Time Of Transanction: {context.Message.CreatedAt}" +
            $"<br> Reciever: {context.Message.Reciever}  " +
            $"<br>" +
            $"<br><br> We are very sorry for any inconvenience this might cause. You can always get in touch with our support team which is active 24/7 incase you need any assistance. " +
            $"<br><br> Thanks <br><br> anointedMtc");
        await _emailService.Send(message);

        var emailToSave = _mapper.Map<EmailEntity>(message);
        emailToSave.EventType = nameof(NotifyCustomerOfVtuDataPurchaseFailedEvent);
        await _emailRepository.AddAsync(emailToSave);

        _logger.LogInformation("Successfully sent and saved email to User with Id {UserId} by {typeOfEvent} at {Time}",
            context.Message.Email,
            nameof(NotifyCustomerOfVtuDataPurchaseFailedEvent),
            DateTimeOffset.UtcNow
        );
    }
}
