using SIS.Http.Responses.Contracts;
using IRunesWebApp.GlobalConst;
using IRunesWebApp.ViewModels.Account;
using SIS.MvcFramework;

namespace IRunesWebApp.Controller
{
    public class HomeController : BaseController
    {
        [HttpGet("/")]
        public IHttpResponse Index()
        {
            if (this.Request.Cookies.GetCookie(GlobalConstants.userCookieAuthentication) == null)
            {
                return this.View("Index", "_Layout_LoggedOut");
            }

            var username = this.User;
            var viewModel = new IndexViewModel()
            {
                Username = username
            };
            return this.View("IndexLogged", viewModel);
        }
    }
}
