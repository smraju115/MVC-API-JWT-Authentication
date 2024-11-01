using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Security.Claims;

namespace WP1.Providers
{
    public class AppOAuthProvider: OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            await Task.Run(() =>
            {
                context.Validated();
            });

        }
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var um = new UserManager<IdentityUser>(new UserStore<IdentityUser>());

            var u = await um.FindAsync(context.UserName, context.Password);
            if (u == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            await Task.Run(() =>
            {
                context.Validated(new ClaimsIdentity(context.Options.AuthenticationType));
            });

        }
    }
}