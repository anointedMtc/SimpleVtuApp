using AutoMapper;
using DomainSharedKernel.Interfaces;
using Identity.Shared.IntegrationEvents;
using MassTransit;
using Microsoft.Extensions.Logging;
using Notification.Application.Interfaces;
using Notification.Domain.Entities;
using Notification.Shared.DTO;
using System.Text.Encodings.Web;

namespace Notification.Application.IntegrationEvents.IdentityModule;

public sealed class NewApplicationUserRegisteredEventConsumer : IConsumer<NewApplicationUserRegisteredEvent>
{
    private readonly ILogger<NewApplicationUserRegisteredEventConsumer> _logger;
    private readonly IEmailService _emailService;
    private readonly IRepository<EmailEntity> _emailRepository;
    private readonly IMapper _mapper;

    public NewApplicationUserRegisteredEventConsumer(ILogger<NewApplicationUserRegisteredEventConsumer> logger, 
        IEmailService emailService, IRepository<EmailEntity> emailRepository, IMapper mapper)
    {
        _logger = logger;
        _emailService = emailService;
        _emailRepository = emailRepository;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<NewApplicationUserRegisteredEvent> context)
    {
        _logger.LogInformation("Sending email to User with Id {UserId} by {typeOfEvent} at {Time}",
            context.Message.Email,
            nameof(NewApplicationUserRegisteredEventConsumer),
            DateTimeOffset.UtcNow
        );

        var message = new EmailDto(context.Message.Email!, "Email Confirmation Token", $"Dear Subscriber, <br><br>Please confirm your Email account by <a href={HtmlEncoder.Default.Encode(context.Message.CallbackUrl)}>clicking here</a>.  <br><br> You can as well choose to copy your Token below and paste in appropriate apiEndpoint: <br><br> {HtmlEncoder.Default.Encode(context.Message.ValidToken)} <br><br> If however you didn't make this request, kindly ignore. <br><br> Thanks <br><br> anointedMtc");
        await _emailService.Send(message);

        var emailToSave = _mapper.Map<EmailEntity>(message);
        emailToSave.EventType = nameof(NewApplicationUserRegisteredEventConsumer);
        await _emailRepository.AddAsync(emailToSave);

        _logger.LogInformation("Successfully sent and saved email to User with Id {UserId} by {typeOfEvent} at {Time}",
            context.Message.Email,
            nameof(NewApplicationUserRegisteredEventConsumer),
            DateTimeOffset.UtcNow
        );
    }
}
