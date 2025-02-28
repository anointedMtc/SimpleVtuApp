using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using Notification.Application.Interfaces;
using Notification.Domain.Entities;
using Notification.Domain.Interfaces;
using Notification.Shared.DTO;
using Wallet.Shared.IntegrationEvents;

namespace Notification.Application.IntegrationEvents.WalletModule;

public sealed class NotifyOwnersOfFundsTransferredEventConsumer
    : IConsumer<NotifyOwnersOfFundsTransferredEvent>
{
    private readonly ILogger<NotifyOwnersOfFundsTransferredEventConsumer> _logger;
    private readonly IEmailService _emailService;
    private readonly IEmailRepository<EmailEntity> _emailRepository;
    private readonly IMapper _mapper;

    public NotifyOwnersOfFundsTransferredEventConsumer(
        ILogger<NotifyOwnersOfFundsTransferredEventConsumer> logger, 
        IEmailService emailService, IEmailRepository<EmailEntity> emailRepository, 
        IMapper mapper)
    {
        _logger = logger;
        _emailService = emailService;
        _emailRepository = emailRepository;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<NotifyOwnersOfFundsTransferredEvent> context)
    {
        // SENDER
        _logger.LogInformation("Sending email to User with Id {UserId} by {typeOfEvent} at {Time} with {@Details}",
           context.Message.FromWalletEmail,
           nameof(NotifyOwnersOfFundsTransferredEvent),
           DateTimeOffset.UtcNow,
           context.Message
        );

        var message = new EmailDto(context.Message.FromWalletFirstName!, "Funds Transfer", $"Dear {context.Message.FromWalletFirstName}, " +
            $"<br><br> We wish to inform you that your transfer of <del>N</del> {context.Message.Amount} naira to your friends Wallet {context.Message.ToWalletFirstName} was successful." +
            $"<br><br> Details of this transaction are as follows:" +
            $"<br>" +
            $"<br> TransferId: {context.Message.FromWalletTransferId}," +
            $"<br> AmountTransfered: {context.Message.Amount}" +
            $"<br> InitialWalletBalance: {context.Message.FromWalletBalance + context.Message.Amount}" +
            $"<br> FinalWalletBalance: {context.Message.FromWalletBalance}" +
            $"<br> Time Of Transanction: {context.Message.CreatedAt}" +
            $"<br>" +
            $"<br><br> Don't forget to check our exciting and new offers that offers best value for best price." +
            $"<br> You can always get in touch with our support team which is active 24/7 incase you need any assistance. " +
            $"<br><br> Thanks <br><br> anointedMtc");
        await _emailService.Send(message);

        var emailToSave = _mapper.Map<EmailEntity>(message);
        emailToSave.EventType = nameof(NotifyOwnersOfFundsTransferredEvent);
        await _emailRepository.AddAsync(emailToSave);

        _logger.LogInformation("Successfully sent and saved email to User with Id {UserId} by {typeOfEvent} at {Time}",
            context.Message.FromWalletBalance,
            nameof(NotifyOwnersOfFundsTransferredEvent),
            DateTimeOffset.UtcNow
        );




        // RECEIVER
        _logger.LogInformation("Sending email to User with Id {UserId} by {typeOfEvent} at {Time} with {@Details}",
           context.Message.ToWalletEmail,
           nameof(NotifyOwnersOfFundsTransferredEvent),
           DateTimeOffset.UtcNow,
           context.Message
        );

        var secondMessage = new EmailDto(context.Message.ToWalletFirstName!, "Funds Transfer", $"Dear {context.Message.ToWalletFirstName}, " +
            $"<br><br> We wish to inform you that you received a transfer of <del>N</del> {context.Message.Amount} naira from your friend {context.Message.FromWalletFirstName}." +
            $"<br><br> Details of this transaction are as follows:" +
            $"<br>" +
            $"<br> TransferId: {context.Message.ToWalletTransferId}," +
            $"<br> AmountTransfered: {context.Message.Amount}" +
            $"<br> InitialWalletBalance: {context.Message.ToWalletBalance + context.Message.Amount}" +
            $"<br> FinalWalletBalance: {context.Message.ToWalletBalance}" +
            $"<br> Time Of Transanction: {context.Message.CreatedAt}" +
            $"<br>" +
            $"<br><br> Don't forget to check our exciting and new offers that offers best value for best price." +
            $"<br> You can always get in touch with our support team which is active 24/7 incase you need any assistance. " +
            $"<br><br> Thanks <br><br> anointedMtc");
        await _emailService.Send(secondMessage);

        var secondEmailToSave = _mapper.Map<EmailEntity>(secondMessage);
        secondEmailToSave.EventType = nameof(NotifyOwnersOfFundsTransferredEvent);
        await _emailRepository.AddAsync(secondEmailToSave);

        _logger.LogInformation("Successfully sent and saved email to User with Id {UserId} by {typeOfEvent} at {Time}",
            context.Message.ToWalletBalance,
            nameof(NotifyOwnersOfFundsTransferredEvent),
            DateTimeOffset.UtcNow
        );
    }
}
