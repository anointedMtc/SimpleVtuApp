using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using Notification.Application.Interfaces;
using Notification.Domain.Entities;
using Notification.Shared.DTO;
using SharedKernel.Domain.Interfaces;
using VtuApp.Shared.IntegrationEvents;

namespace Notification.Application.IntegrationEvents.VtuAppModule;

public sealed class FundsRefundedForVtuPurchaseFailureEventConsumer
    : IConsumer<FundsRefundedForVtuPurchaseFailureEvent>
{
    private readonly ILogger<FundsRefundedForVtuPurchaseFailureEventConsumer> _logger;
    private readonly IEmailService _emailService;
    private readonly IRepository<EmailEntity> _emailRepository;
    private readonly IMapper _mapper;

    public FundsRefundedForVtuPurchaseFailureEventConsumer(
        ILogger<FundsRefundedForVtuPurchaseFailureEventConsumer> logger, 
        IEmailService emailService, IRepository<EmailEntity> emailRepository,
        IMapper mapper)
    {
        _logger = logger;
        _emailService = emailService;
        _emailRepository = emailRepository;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<FundsRefundedForVtuPurchaseFailureEvent> context)
    {
        _logger.LogInformation("Sending email to User with Id {UserId} by {typeOfEvent} at {Time} with {@Details}",
            context.Message.Email,
            nameof(FundsRefundedForVtuPurchaseFailureEvent),
            DateTimeOffset.UtcNow,
            context.Message
        );

        var message = new EmailDto(context.Message.Email!, "Payment Refund", $"Dear {context.Message.FirstName}, " +
            $"<br><br> We wish to inform you that your payment for Airtime Purchase transaction with Id {context.Message.VtuTransactionId} has been successfully refunded to your Vtu Account Balance ." +
            $"<br><br> Details of this transaction are as follows:" +
            $"<br>" +
            $"<br> NetworkProvider: {context.Message.NetworkProvider}," +
            $"<br> Value: {context.Message.AmountPurchased}" +
            $"<br> Cost: {context.Message.PricePaid}" +
            $"<br> Time Of Transanction: {context.Message.CreatedAt}" +
            $"<br> Reciever: {context.Message.Receiver}  " +
            $"<br>" +
            $"<br><br> Don't forget to check for exciting and new offers that offers best value for best price." +
            $"<br> You can always get in touch with our support team which is active 24/7 incase you need any assistance. " +
            $"<br><br> Thanks <br><br> anointedMtc");
        await _emailService.Send(message);

        var emailToSave = _mapper.Map<EmailEntity>(message);
        emailToSave.EventType = nameof(FundsRefundedForVtuPurchaseFailureEvent);
        await _emailRepository.AddAsync(emailToSave);

        _logger.LogInformation("Successfully sent and saved email to User with Id {UserId} by {typeOfEvent} at {Time}",
            context.Message.Email,
            nameof(FundsRefundedForVtuPurchaseFailureEvent),
            DateTimeOffset.UtcNow
        );
    }
}
