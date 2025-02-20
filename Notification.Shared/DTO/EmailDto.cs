namespace Notification.Shared.DTO;

//public record EmailDto (
//    string ToAddress, 
//    string Subject,
//    string? Body,
//    string? AttachmentPath,
//    string? Cc,
//    string? Bcc,
//    string? Sender,
//    string? EventType,
//    DateTimeOffset? CreatedAt );


public class EmailDto
{
    public string ToAddress { get; set; }
    public string Subject { get; set; }
    public string? Body { get; set; }
    public string? AttachmentPath { get; set; }
    public string? CC { get; set; }
    public string? BCC { get; set; }
    public string? Sender { get; set; }
    public string? EventType { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public EmailDto(string toAddress, string subject, string? body = "",
        string? attachmentPath = "", string? cc = "", string? bcc = "",
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
}


