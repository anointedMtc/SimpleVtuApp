using Hangfire;
using HangfireBasicAuthenticationFilter;

namespace VtuHost.WebApi.Middlewares;

public static class HangfireMiddleware
{
    public static void MapHangfireEndpoints(this WebApplication app)
    {
        app.UseHangfireDashboard("/api/admin/hangfire", new DashboardOptions
        {
            Authorization =
            [
                new HangfireCustomBasicAuthenticationFilter
                {
                    User = app.Configuration.GetSection("HangfireSettings:Username").Value,
                    Pass = app.Configuration.GetSection("HangfireSettings:Password").Value
                }
            ],

            // If the user is not in the Admin Role, it presents only a readonly dashboard for him/her
            //IsReadOnlyFunc = (DashboardContext dashboardContext) =>
            //{
            //    var context = dashboardContext.GetHttpContext();
            //    return !context.User.IsInRole("Admin");
            //},

            //The path for the first url prefix link, eg.set "/admin", then url is "{domain}/{PrefixPath}/{hangfire}"
            //PrefixPath = "/admin",

            DashboardTitle = "RWESpecificationPattern DashBoard",

            DarkModeEnabled = false,
            DisplayStorageConnectionString = true,

            // Change `Back to site` link URL
            AppPath = "https://localhost:7287/swagger/index.html",

        });
    }
}
