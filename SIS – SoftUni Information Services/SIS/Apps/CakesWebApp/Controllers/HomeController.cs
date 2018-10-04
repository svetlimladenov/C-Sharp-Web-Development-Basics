
namespace CakesWebApp.Controllers
{
    using SIS.Http.Requests.Contracts;
    using SIS.Http.Enums;
    using SIS.Http.Responses.Contracts;
    using SIS.WebServer.Results;

    public class HomeController : BaseController
    {
        public IHttpResponse Index(IHttpRequest request)
        {
            return this.View("Index");
        }

    }
}
