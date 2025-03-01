using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using Notification.Application.Interfaces;
using Notification.Domain.Entities;
using Notification.Domain.Interfaces;
using Notification.Shared.DTO;
using VtuApp.Shared.IntegrationEvents;

namespace Notification.Application.IntegrationEvents.VtuAppModule;

public sealed class NotifyUserOfFiveVtuTransactionsAchievedEventConsumer
    : IConsumer<NotifyUserOfFiveVtuTransactionsAchievedEvent>
{
    private readonly ILogger<NotifyUserOfFiveVtuTransactionsAchievedEventConsumer> _logger;
    private readonly IEmailService _emailService;
    private readonly IEmailRepository<EmailEntity> _emailRepository;
    private readonly IMapper _mapper;

    public NotifyUserOfFiveVtuTransactionsAchievedEventConsumer(ILogger<NotifyUserOfFiveVtuTransactionsAchievedEventConsumer> logger, 
        IEmailService emailService, IEmailRepository<EmailEntity> emailRepository, IMapper mapper)
    {
        _logger = logger;
        _emailService = emailService;
        _emailRepository = emailRepository;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<NotifyUserOfFiveVtuTransactionsAchievedEvent> context)
    {
        _logger.LogInformation("Sending email to User with Id {UserId} by {typeOfEvent} at {Time} with {@Details}",
           context.Message.Email,
           nameof(NotifyUserOfFiveVtuTransactionsAchievedEvent),
           DateTimeOffset.UtcNow,
           context.Message
        );

        var message = new EmailDto(context.Message.Email!, "New Star Achieved", $"Dear {context.Message.FirstName}, " +
           $"<br><br> We wish to congratulate you on the achievement of Five-Vtu-Transactions. As a result, the sum of <del>N</del> {context.Message.BonusForFiveTransactions} naira has been credited to your vtuBonus Balance." +
           $"<br><br> Details of this transaction are as follows:" +
           $"<br>" +
           $"<br> AmountTransfered: {context.Message.BonusForFiveTransactions}" +
           $"<br> InitialWalletBalance: {context.Message.FinalVtuBonusBalance - context.Message.BonusForFiveTransactions}" +
           $"<br> FinalWalletBalance: {context.Message.FinalVtuBonusBalance}" +
           $"<br> Time Of Transanction: {context.Message.CreatedAt}" +
           $"<br>" +
           $"<br><br> You would always recieve this bonus for every new Five transactions you make." +
           $"<br>" +
           $"<br><br> Don't forget to check our exciting and new offers that offers best value for best price." +
           $"<br> You can always get in touch with our support team which is active 24/7 incase you need any assistance. " +
           $"<br><br> Thanks <br><br> anointedMtc");
        await _emailService.Send(message);

        var emailToSave = _mapper.Map<EmailEntity>(message);
        emailToSave.EventType = nameof(NotifyUserOfFiveVtuTransactionsAchievedEvent);
        await _emailRepository.AddAsync(emailToSave);

        _logger.LogInformation("Successfully sent and saved email to User with Id {UserId} by {typeOfEvent} at {Time}",
            context.Message.Email,
            nameof(NotifyUserOfFiveVtuTransactionsAchievedEvent),
            DateTimeOffset.UtcNow
        );
    }
}
