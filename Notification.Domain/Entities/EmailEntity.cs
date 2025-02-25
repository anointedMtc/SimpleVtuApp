using SharedKernel.Domain;
using SharedKernel.Domain.Interfaces;

namespace Notification.Domain.Entities;

public class EmailEntity : BaseEntity, IAggregateRoot
{
    public Guid EmailId { get; set; }
    public string ToAddress { get; set; }
    public string Subject { get; set; }
    public string? Body { get; set; }
    public string? AttachmentPath { get; set; }
    public string? CC { get; set; }
    public string? BCC { get; set; }
    public string? Sender { get; set; }
    public string? EventType { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public EmailEntity(string toAddress, string subject, string? body = null,
        string? attachmentPath = null, string? cc = null, string? bcc = null, 
        string? sender = null, string? eventType = null)
    {
        ToAddress = toAddress;
        Subject = subject;
        Body = body;
        AttachmentPath = attachmentPath;
        CC = cc;
        BCC = bcc;
        Sender = sender;
        EventType = eventType;
        CreatedAt = DateTimeOffset.UtcNow;
    }


    //#pragma warning disable CS8618    // Required by Entity Framework
    private EmailEntity() { }



    public override string ToString()
    {
        return $"{EmailId} --- {ToAddress} --- {Subject} --- {Body} --- {CC} --- {BCC} --- {Sender} --- {EventType} --- {CreatedAt}";
    }

}