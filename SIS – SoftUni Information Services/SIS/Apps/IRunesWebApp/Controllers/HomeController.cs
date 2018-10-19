using System.Collections.Generic;
using System.Text;
using SIS.Http.Enums;
using SIS.Http.Requests.Contracts;
using SIS.Http.Responses;
using SIS.Http.Responses.Contracts;
using SIS.WebServer.Results;
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
                return this.ViewLoggedOut("Index");
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
