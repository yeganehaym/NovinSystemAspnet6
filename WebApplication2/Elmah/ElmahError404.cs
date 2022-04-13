using ElmahCore;

namespace WebApplication2.Elmah;

public class ElmahError404:IErrorFilter
{
    public void OnErrorModuleFiltering(object sender, ExceptionFilterEventArgs args)
    {
        if (args.Context is HttpContext)
        {
            var context = (HttpContext) args.Context;
            if (context.Response.StatusCode == 404)
            {
                args.Dismiss();
            }
        }
    }
}