using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Globalization;
using System.Threading.Tasks;
using BuildertrendMVC.Models;

namespace BuildertrendMVC.Middleware
{
    public class UserCultureMiddleware
    {
        private readonly RequestDelegate _next;

        public UserCultureMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, UserManager<ApplicationUser> userManager)
        {
            if (context.User.Identity != null && context.User.Identity.IsAuthenticated)
            {
                var user = await userManager.GetUserAsync(context.User);
                if (user != null && !string.IsNullOrEmpty(user.Language))
                {
                    var culture = user.Language == "en" ? "en" : "es";
                    var cultureInfo = new CultureInfo(culture);
                    CultureInfo.CurrentCulture = cultureInfo;
                    CultureInfo.CurrentUICulture = cultureInfo;
                }
            }
            await _next(context);
        }
    }
}
