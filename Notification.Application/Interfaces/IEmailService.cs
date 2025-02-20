using Notification.Shared.DTO;

namespace Notification.Application.Interfaces;

public interface IEmailService
{
    Task Send(EmailDto emailMetadata);

    Task SendUsingTemplate(EmailDto emailMetadata, UserEmailDto user, string templateFile);

    Task SendUsingLiqTemplate(EmailDto emailMetadata, string template, UserEmailDto user);    // this is for using liquid template or when you want to type the razor syntax directly

    Task SendWithAttachment(EmailDto emailMetadata);

    Task SendMultiple(List<EmailDto> emailMetadataList);
}