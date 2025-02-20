using FluentEmail.Core;
using Notification.Application.Interfaces;
using Notification.Shared.DTO;

namespace Notification.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IFluentEmail _fluentEmail;
    private readonly IFluentEmailFactory _fluentEmailFactory;

    public EmailService(IFluentEmail fluentEmail, IFluentEmailFactory fluentEmailFactory)
    {
        _fluentEmail = fluentEmail ?? throw new ArgumentNullException(nameof(fluentEmail));
        _fluentEmailFactory = fluentEmailFactory ?? throw new ArgumentNullException(nameof(fluentEmailFactory));
    }

    public async Task Send(EmailDto emailMetadata)
    {
        await _fluentEmail.To(emailMetadata.ToAddress)
            //.SetFrom(emailMetadata.ToAddress, "Anointed")   // change the sender address
            .Subject(emailMetadata.Subject)
            .Body(emailMetadata.Body, isHtml: true)
            .CC(emailMetadata.ToAddress)
            .BCC(emailMetadata.ToAddress)
            .SendAsync();
    }

    public async Task SendUsingTemplate(EmailDto emailMetadata, UserEmailDto user, string templateFile)
    {
        await _fluentEmail.To(emailMetadata.ToAddress)
            //.SetFrom(emailMetadata.ToAddress, "Anointed")   // change the sender address
            .Subject(emailMetadata.Subject)
            .UsingTemplateFromFile(templateFile, user)
            .CC(emailMetadata.ToAddress)
            .BCC(emailMetadata.ToAddress)
            .SendAsync();
    }

    public async Task SendUsingLiqTemplate(EmailDto emailMetadata, string template, UserEmailDto user)
    {
        await _fluentEmail.To(emailMetadata.ToAddress)
            //.SetFrom(emailMetadata.ToAddress, "Anointed")   // change the sender address
            .Subject(emailMetadata.Subject)
            .UsingTemplate(template, user)
            .CC(emailMetadata.ToAddress)
            .BCC(emailMetadata.ToAddress)
            .SendAsync();
    }

    public async Task SendWithAttachment(EmailDto emailMetadata)
    {
        await _fluentEmail.To(emailMetadata.ToAddress)  // it can also be "luke.lowrey@example.com", "Luke" which is emailAddress, userName
             //.SetFrom(emailMetadata.ToAddress, "Anointed")   // change the sender address
            .Subject(emailMetadata.Subject)
            //.Attach(Core.Models.Attachmet)
            .AttachFromFilename(emailMetadata.AttachmentPath,
                    attachmentName: Path.GetFileName(emailMetadata.AttachmentPath))
            .Body(emailMetadata.Body)
            .CC(emailMetadata.ToAddress)
            .BCC(emailMetadata.ToAddress)
            .SendAsync();
    }

    public async Task SendMultiple(List<EmailDto> emailMetadataList)
    {
        foreach (var item in emailMetadataList)
        {
            await _fluentEmailFactory
                .Create()
                .To(item.ToAddress)
                .Subject(item.Subject)
                .Body(item.Body)
                .CC(item.ToAddress)
                .BCC(item.ToAddress)
                .SendAsync();
        }
    }
}
