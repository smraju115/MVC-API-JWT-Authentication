using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Threading.Tasks;
using WP1.Providers;

[assembly: OwinStartup(typeof(WP1.Startup))]

namespace WP1
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            app.UseCors(CorsOptions.AllowAll);
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            OAuthAuthorizationServerOptions option
                = new OAuthAuthorizationServerOptions
                {
                    TokenEndpointPath = new PathString("/Token"),
                    Provider = new AppOAuthProvider(),
                    AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(60),
                    AllowInsecureHttp = true
                };
            app.UseOAuthAuthorizationServer(option);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

        }
    }
}
