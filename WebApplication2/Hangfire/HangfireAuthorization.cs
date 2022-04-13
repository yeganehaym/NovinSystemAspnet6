using Hangfire.Dashboard;

namespace WebApplication2.Hangfire;

public class HangfireAuthorization:IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();
        var result= httpContext.User.Identity.IsAuthenticated && httpContext.User.IsInRole("Admin");
        return false;
    }
}