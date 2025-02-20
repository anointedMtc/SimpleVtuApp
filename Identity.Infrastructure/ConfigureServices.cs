using DomainSharedKernel.Interfaces;
using Identity.Application.Interfaces;
using Identity.Domain.Entities;
using Identity.Infrastructure.Authorization;
using Identity.Infrastructure.Models;
using Identity.Infrastructure.Persistence;
using Identity.Infrastructure.Persistence.Repositories;
using Identity.Infrastructure.Services;
using Identity.Shared.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;

namespace Identity.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddIdentityInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(typeof(IRepository<>), typeof(IdentityRepository<>));

        services.AddScoped<ApplicationDbContextInitializer>();

        services.AddIdentity<ApplicationUser, ApplicationRole>(opt =>
        {
            opt.Password.RequireDigit = false;
            opt.Password.RequireLowercase = false;
            opt.Password.RequireNonAlphanumeric = false;
            opt.Password.RequireUppercase = false;
            opt.Password.RequiredLength = 7;
            opt.Password.RequiredUniqueChars = 0;

            opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            opt.User.RequireUniqueEmail = true;
            opt.SignIn.RequireConfirmedEmail = true;
            opt.Tokens.EmailConfirmationTokenProvider = "emailconfirmation";

            opt.Lockout.AllowedForNewUsers = true;
            opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);   
            opt.Lockout.MaxFailedAccessAttempts = 3;                              
        })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders()      
            .AddPasswordValidator<CustomPasswordValidator<ApplicationUser>>()
            .AddTokenProvider<EmailConfirmationTokenProvider<ApplicationUser>>("emailconfirmation");

        services.Configure<DataProtectionTokenProviderOptions>(opt =>
            opt.TokenLifespan = TimeSpan.FromHours(2));                   

        services.Configure<EmailConfirmationTokenProviderOptions>(opt =>
             opt.TokenLifespan = TimeSpan.FromDays(3));                    

        services.AddAuthorization(options =>
        {
            // RoleBase Auth
            options.AddPolicy(PolicyNames.AdminAndAbove,
                policy => policy.RequireRole(AppUserRoles.Admin, AppUserRoles.GodsEye));

            options.AddPolicy(PolicyNames.HasNationality,
                builder => builder.RequireClaim(AppClaimTypes.Nationality, "Nigeria", "Ghana"));     

            options.AddPolicy(PolicyNames.AtLeast20,
                builder => builder.AddRequirements(new MinimumAgeRequirement(18)));
        });

        services.AddScoped<IAuthorizationHandler, MinimumAgeRequirementHandler>();


        // registering JWT
        var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
        if (jwtSettings == null || string.IsNullOrEmpty(jwtSettings.Key))
        {
            throw new InvalidOperationException("JWT secret key is not configured.");
        }

        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
        services.AddAuthentication(o =>
        {
            o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.ValidIssuer,
                ValidAudience = jwtSettings.ValidAudience,
                IssuerSigningKey = secretKey
            };
            o.Events = new JwtBearerEvents
            {
                OnChallenge = context =>
                {
                    context.HandleResponse();
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "application/json";
                    var result = JsonSerializer.Serialize(new
                    {
                        message = "You are not authorized to access this resource. Please authenticate."
                    });
                    return context.Response.WriteAsync(result);
                },
            };
        });

        services.AddScoped<ITokenService, TokenService>();


        return services;
    }
}
