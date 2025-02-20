namespace Notification.Infrastructure.Models;

public class EmailAppSettings
{
    public string DefaultFromEmail { get; set; }
    public string DefaultFromName { get; set; }
    public string SmtpServer { get; set; }
    public int Port { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}
