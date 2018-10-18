using System.Collections.Generic;
using CakesWebApp.ViewModels.Home;
using SIS.Http.Enums;
using SIS.Http.Requests.Contracts;
using SIS.Http.Responses.Contracts;
using SIS.MvcFramework;
using SIS.WebServer.Results;

namespace CakesWebApp.Controllers
{
    public class HomeController : BaseController
    {
        [HttpGet("/")]
        public IHttpResponse Index()
        {
            return this.View("Index");
        }

        [HttpGet("/hello")]
        public IHttpResponse HelloUser()
        {
            return this.View("HelloUser", new HelloUserViewModel { Username = this.User });
        }
    }
}
