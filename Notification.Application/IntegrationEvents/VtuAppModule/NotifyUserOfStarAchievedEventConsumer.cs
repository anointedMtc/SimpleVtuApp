using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using Notification.Application.Interfaces;
using Notification.Domain.Entities;
using Notification.Domain.Interfaces;
using Notification.Shared.DTO;
using VtuApp.Shared.IntegrationEvents;

namespace Notification.Application.IntegrationEvents.VtuAppModule;

public sealed class NotifyUserOfStarAchievedEventConsumer
    : IConsumer<NotifyUserOfStarAchievedEvent>
{
    private readonly ILogger<NotifyUserOfStarAchievedEventConsumer> _logger;
    private readonly IEmailService _emailService;
    private readonly IEmailRepository<EmailEntity> _emailRepository;
    private readonly IMapper _mapper;

    public NotifyUserOfStarAchievedEventConsumer(ILogger<NotifyUserOfStarAchievedEventConsumer> logger, 
        IEmailService emailService, IEmailRepository<EmailEntity> emailRepository, IMapper mapper)
    {
        _logger = logger;
        _emailService = emailService;
        _emailRepository = emailRepository;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<NotifyUserOfStarAchievedEvent> context)
    {
        _logger.LogInformation("Sending email to User with Id {UserId} by {typeOfEvent} at {Time} with {@Details}",
           context.Message.Email,
           nameof(NotifyUserOfStarAchievedEvent),
           DateTimeOffset.UtcNow,
           context.Message
        );
                                                                    // if a customer adds up to 3 new transactions within the span of one hour, then he gets a star and 10% bonus of the 3 transactions made
        var message = new EmailDto(context.Message.Email!, "New Star Achieved", $"Dear {context.Message.FirstName}, " +
           $"<br><br> We wish to congratulate you on the achievement of a star because you made up to 3 transactions within the span of one hour. As a result, the sum of <del>N</del> {context.Message.DiscountGiven} naira which is 10% of the 3 transactions made within that hour has been credited to your vtuBonus Balance." +
           $"<br><br> Details of this transaction are as follows:" +
           $"<br>" +
           $"<br> Total_Value_of_Transactions_Made: {context.Message.TotalOfTransactionsMade}" +
           $"<br> Amount_Transfered: {context.Message.DiscountGiven}" +
           $"<br> Initial_Wallet_Balance: {context.Message.FinalVtuBonusBalance - context.Message.DiscountGiven}" +
           $"<br> Final_Wallet_Balance: {context.Message.FinalVtuBonusBalance}" +
           $"<br> Time Of Transanction: {context.Message.CreatedAt}" +
           $"<br>" +
           $"<br><br> One hour would need to elapse before you can qualify for a new star" +
           $"<br>" +
           $"<br><br> Don't forget to check our exciting and new offers that offers best value for best price." +
           $"<br> You can always get in touch with our support team which is active 24/7 incase you need any assistance. " +
           $"<br><br> Thanks <br><br> anointedMtc");
        await _emailService.Send(message);

        var emailToSave = _mapper.Map<EmailEntity>(message);
        emailToSave.EventType = nameof(NotifyUserOfStarAchievedEvent);
        await _emailRepository.AddAsync(emailToSave);

        _logger.LogInformation("Successfully sent and saved email to User with Id {UserId} by {typeOfEvent} at {Time}",
            context.Message.Email,
            nameof(NotifyUserOfStarAchievedEvent),
            DateTimeOffset.UtcNow
        );
    }
}
