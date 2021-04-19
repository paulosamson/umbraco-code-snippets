using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Umbraco.Core.Models.Identity;
using Umbraco.Core.Models.Membership;
using Umbraco.Web;
using Umbraco.Web.WebApi;

namespace Website.Core.Controllers.ApiControllers
{
  public class TheApiController : UmbracoApiController
  {
    [HttpGet]
    public async Task<IHttpActionResult> CreateUser()
    {
        var owinContext = TryGetOwinContext().Result;
        var userManager = owinContext.GetBackOfficeUserManager();

        var email = "paulosamson521@gmail.com";
        var user = BackOfficeIdentityUser.CreateNew(email, email, "en-US");
        user.Name = "Paulo Samson";

        await userManager.CreateAsync(user);
        var password = userManager.GeneratePassword();
        await userManager.AddPasswordAsync(user.Id, password);

        //Save group
        var u = Services.UserService.GetByUsername(email);
        u.IsApproved = true;
        var group = Services.UserService.GetUserGroupByAlias("admin") as IReadOnlyUserGroup;
        u.AddGroup(group);
        Services.UserService.Save(u);

        return Ok(password);
    }
  }
}

