using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WP1.ViewModels;

namespace WP1.Controllers
{
    public class AccountController : ApiController
    {
        [HttpPost]
        public async Task<IHttpActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new IdentityUser() { UserName = model.UserName };
            var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>());// HttpContext.Current.GetOwinContext().GetUserManager <UserManager<IdentityUser>> ();
            IdentityResult result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(ModelState);
            }

            return Ok();
        }
    }
}
