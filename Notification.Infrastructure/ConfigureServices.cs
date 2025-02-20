using DomainSharedKernel.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notification.Application.Interfaces;
using Notification.Infrastructure.Models;
using Notification.Infrastructure.Persistence.Repositories;
using Notification.Infrastructure.Services;

namespace Notification.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddNotificationInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(typeof(IRepository<>), typeof(EmailRepository<>));


        // EMAIL
        services.AddScoped<IEmailService, EmailService>(); 

        var emailConfig = configuration.GetSection("EmailConfiguration").Get<EmailAppSettings>(); 

        var defaultFromEmail = emailConfig!.DefaultFromEmail;   
        var defaultFromName = emailConfig!.DefaultFromName;
        var host = emailConfig.SmtpServer; 
        var port = emailConfig.Port; 
        var userName = emailConfig.UserName; 
        var password = emailConfig.Password; 

        services.AddFluentEmail(defaultFromEmail)  
            .AddSmtpSender("localhost", 25)
            .AddRazorRenderer()
            .AddLiquidRenderer();      


        return services;
    }
}
