using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using CakesWebApp.Services;
using IRunesWebApp.Data;
using SIS.Http.Enums;
using SIS.Http.Responses.Contracts;
using SIS.WebServer.Results;
using SIS.MvcFramework;

namespace IRunesWebApp.Controller
{
    public class BaseController : SIS.MvcFramework.Controller
    {
        protected readonly string userCookieAuth = ".auth-IRunes";
        public BaseController()
        {
            this.Db = new IRunesDbContext();
            this.UserCookieService = new UserCookieService(); 
        }

        public IRunesDbContext Db { get; }

        public IUserCookieService UserCookieService { get; }

        public IHttpResponse View(string viewName)
        {
            var allContent = this.GetViewContent(viewName);
            return new HtmlResult(allContent, HttpResponseStatusCode.OK);
        }

        public IHttpResponse ViewLoggedOut(string viewName)
        {
            var allContent = this.GetViewContentLoggedOut(viewName);
            return new HtmlResult(allContent, HttpResponseStatusCode.OK);
        }

        protected IHttpResponse BadRequestError(string errorMessage)
        {
            var response = this.View("BadRequest");
            var view = Encoding.UTF8.GetString(response.Content);
            view = view.Replace("{errorMessage}", errorMessage);

            return new HtmlResult(view, HttpResponseStatusCode.BadRequest);
        }

        protected IHttpResponse ServerError(string errorMessage)
        {
            return new HtmlResult($"<h1>{errorMessage}</h1>", HttpResponseStatusCode.InternalServerError);
        }

        private string GetViewContent(string viewName)
        {
            var layoutContent = File.ReadAllText("Views/_Layout.html");
            var content = File.ReadAllText("Views/" + viewName + ".html");
            var allContent = layoutContent.Replace("@RenderBody()", content);
            return allContent;
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
