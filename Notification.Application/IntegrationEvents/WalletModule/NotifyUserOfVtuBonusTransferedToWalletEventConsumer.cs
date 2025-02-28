using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using Notification.Application.Interfaces;
using Notification.Domain.Entities;
using Notification.Domain.Interfaces;
using Notification.Shared.DTO;
using Wallet.Shared.IntegrationEvents;

namespace Notification.Application.IntegrationEvents.WalletModule;

public sealed class NotifyUserOfVtuBonusTransferedToWalletEventConsumer
    : IConsumer<NotifyUserOfVtuBonusTransferedToWalletEvent>
{
    private readonly ILogger<NotifyUserOfVtuBonusTransferedToWalletEventConsumer> _logger;
    private readonly IEmailService _emailService;
    private readonly IEmailRepository<EmailEntity> _emailRepository;
    private readonly IMapper _mapper;

    public NotifyUserOfVtuBonusTransferedToWalletEventConsumer(
        ILogger<NotifyUserOfVtuBonusTransferedToWalletEventConsumer> logger, 
        IEmailService emailService, IEmailRepository<EmailEntity> emailRepository,
        IMapper mapper)
    {
        _logger = logger;
        _emailService = emailService;
        _emailRepository = emailRepository;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<NotifyUserOfVtuBonusTransferedToWalletEvent> context)
    {
        _logger.LogInformation("Sending email to User with Id {UserId} by {typeOfEvent} at {Time} with {@Details}",
           context.Message.Email,
           nameof(NotifyUserOfVtuBonusTransferedToWalletEvent),
           DateTimeOffset.UtcNow,
           context.Message
       );

        var message = new EmailDto(context.Message.Email!, "VtuBonus Transferred To Wallet", $"Dear {context.Message.FirstName}, " +
            $"<br><br> We wish to inform you that your transfer of <del>N</del> {context.Message.AmountTransfered} naira to your Wallet was successful." +
            $"<br><br> Details of this transaction are as follows:" +
            $"<br>" +
            $"<br> TransferId: {context.Message.VtuBonusTransferId}," +
            $"<br> AmountTransfered: {context.Message.AmountTransfered}" +
            $"<br> InitialVtuBonusBalance: {context.Message.InitialBalance}" +
            $"<br> FinalVtuBonusBalance: {context.Message.FinalBalance}" +
            $"<br> Time Of Transanction: {context.Message.CreatedAt}" +
            $"<br>" +
            $"<br><br> Don't forget to check for exciting and new offers that offers best value for best price." +
            $"<br> You can always get in touch with our support team which is active 24/7 incase you need any assistance. " +
            $"<br><br> Thanks <br><br> anointedMtc");
        await _emailService.Send(message);

        var emailToSave = _mapper.Map<EmailEntity>(message);
        emailToSave.EventType = nameof(NotifyUserOfVtuBonusTransferedToWalletEvent);
        await _emailRepository.AddAsync(emailToSave);

        _logger.LogInformation("Successfully sent and saved email to User with Id {UserId} by {typeOfEvent} at {Time}",
            context.Message.Email,
            nameof(NotifyUserOfVtuBonusTransferedToWalletEvent),
            DateTimeOffset.UtcNow
        );
    }
}
