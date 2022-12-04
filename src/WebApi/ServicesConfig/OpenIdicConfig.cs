using System.Security.Principal;
using Microsoft.AspNetCore.Authentication;

namespace WebApi.ServicesConfig;

public static class OpenIdicConfig
{
    public static IApplicationBuilder UseOpenIddictValidation(this IApplicationBuilder app, string schema = "OpenIddict.Validation.AspNetCore")
    {
        return app.Use(async delegate (HttpContext ctx, Func<Task> next)
        {
            IIdentity? identity = ctx.User.Identity;
            if (identity == null || !identity!.IsAuthenticated)
            {
                AuthenticateResult authenticateResult = await ctx.AuthenticateAsync(schema).ConfigureAwait(continueOnCapturedContext: false);
                if (authenticateResult.Succeeded && authenticateResult.Principal != null)
                {
                    ctx.User = authenticateResult.Principal;
                }
            }

            await next().ConfigureAwait(continueOnCapturedContext: false);
        });
    }
}