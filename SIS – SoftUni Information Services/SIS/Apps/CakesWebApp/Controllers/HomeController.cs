
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


        public IHttpResponse HelloUser(IHttpRequest request)
        {
            return new HtmlResult($"<h1>Hello, {this.GetUsername(request)} </h1>",HttpResponseStatusCode.OK);
        }
    }
}
