
using System;
using System.Text;

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
            var view = this.View("Index");
            var viewText = Encoding.UTF8.GetString(view.Content);
            viewText = viewText.Replace("{userName}", GetUsername(request));
            return new HtmlResult(viewText,HttpResponseStatusCode.OK);
        }


        public IHttpResponse HelloUser(IHttpRequest request)
        {
            return new HtmlResult($"<h1>Hello, {this.GetUsername(request)} </h1>", HttpResponseStatusCode.OK);
        }
    }
}
