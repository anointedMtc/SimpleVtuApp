using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using Notification.Application.Interfaces;
using Notification.Domain.Entities;
using Notification.Domain.Interfaces;
using Notification.Shared.DTO;
using VtuApp.Shared.IntegrationEvents;

namespace Notification.Application.IntegrationEvents.VtuAppModule;

public sealed class NotifyUserOfFundsAddedEventConsumer
    : IConsumer<NotifyUserOfFundsAddedEvent>
{
    private readonly ILogger<NotifyUserOfFundsAddedEventConsumer> _logger;
    private readonly IEmailService _emailService;
    private readonly IEmailRepository<EmailEntity> _emailRepository;
    private readonly IMapper _mapper;

    public NotifyUserOfFundsAddedEventConsumer(ILogger<NotifyUserOfFundsAddedEventConsumer> logger, 
        IEmailService emailService, IEmailRepository<EmailEntity> emailRepository, 
        IMapper mapper)
    {
        _logger = logger;
        _emailService = emailService;
        _emailRepository = emailRepository;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<NotifyUserOfFundsAddedEvent> context)
    {
        _logger.LogInformation("Sending email to User with Id {UserId} by {typeOfEvent} at {Time} with {@Details}",
            context.Message.Email,
            nameof(NotifyUserOfFundsAddedEvent),
            DateTimeOffset.UtcNow,
            context.Message
        );

        var message = new EmailDto(context.Message.Email!, "Credit Alert! Funds Added to Wallet", $"Dear {context.Message.FirstName}, " +
           $"<br><br> We wish to inform you that the sum of <del>N</del> {context.Message.Amount} naira has been credited to your wallet." +
           $"<br><br> Details of this transaction are as follows:" +
           $"<br>" +
           $"<br> TransferId: {context.Message.TransferId}," +
           $"<br> AmountTransfered: {context.Message.Amount}" +
           $"<br> ReasonWhy: {context.Message.ReasonWhy}" +
           $"<br> InitialWalletBalance: {context.Message.FinalBalance - context.Message.Amount}" +
           $"<br> FinalWalletBalance: {context.Message.FinalBalance}" +
           $"<br> Time Of Transanction: {context.Message.CreatedAt}" +
           $"<br>" +
           $"<br><br> Don't forget to check our exciting and new offers that offers best value for best price." +
           $"<br> You can always get in touch with our support team which is active 24/7 incase you need any assistance. " +
           $"<br><br> Thanks <br><br> anointedMtc");
        await _emailService.Send(message);

        var emailToSave = _mapper.Map<EmailEntity>(message);
        emailToSave.EventType = nameof(NotifyUserOfFundsAddedEvent);
        await _emailRepository.AddAsync(emailToSave);

        _logger.LogInformation("Successfully sent and saved email to User with Id {UserId} by {typeOfEvent} at {Time}",
            context.Message.Email,
            nameof(NotifyUserOfFundsAddedEvent),
            DateTimeOffset.UtcNow
        );
    }
}
