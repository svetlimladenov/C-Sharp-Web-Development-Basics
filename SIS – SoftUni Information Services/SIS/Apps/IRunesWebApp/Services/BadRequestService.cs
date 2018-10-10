using System.IO;
using System.Text;
using SIS.Http.Enums;
using SIS.Http.Responses.Contracts;
using SIS.WebServer.Results;

namespace IRunesWebApp.Services
{
    public class BadRequestService
    {
        public IHttpResponse BadRequestError(string errorMessage)
        {
            var response = this.ViewLoggedOut("BadRequest");
            var view = Encoding.UTF8.GetString(response.Content);
            view = view.Replace("{errorMessage}", errorMessage);

            return new HtmlResult(view, HttpResponseStatusCode.BadRequest);
        }

        public IHttpResponse ServerError(string errorMessage)
        {
            return new HtmlResult($"<h1>{errorMessage}</h1>", HttpResponseStatusCode.InternalServerError);
        }

        public IHttpResponse ViewLoggedOut(string viewName)
        {
            var allContent = this.GetViewContentLoggedOut(viewName);
            return new HtmlResult(allContent, HttpResponseStatusCode.OK);
        }

        private string GetViewContentLoggedOut(string viewName)
        {
            var layoutContent = File.ReadAllText("Views/_Layout_LoggedOut.html");
            var content = File.ReadAllText("Views/" + viewName + ".html");
            var allContent = layoutContent.Replace("@RenderBody()", content);
            return allContent;
        }
    }
}
