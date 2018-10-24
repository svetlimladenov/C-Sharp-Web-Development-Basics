using SIS.HTTP.Enums;
using SIS.HTTP.Responses;
using SIS.MvcFramework;
using SIS.WebServer.Results;

namespace MishMashWebApp.Controllers
{
    public class HomeController : BaseController
    {
        [HttpGet("/Home/Index")]
        public IHttpResponse Index()
        {
            if (this.User == null)
            {
                return this.View("Home/Index");
            }

            return this.View("Home/LoggedInIndex");
        }


        [HttpGet("/")]
        public IHttpResponse RootIndex()
        {
            return this.Index();
        }
    }
}
