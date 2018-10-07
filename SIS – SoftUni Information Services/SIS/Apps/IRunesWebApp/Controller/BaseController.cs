using System.IO;
using System.Runtime.CompilerServices;
using CakesWebApp.Services;
using IRunesWebApp.Data;
using SIS.Http.Enums;
using SIS.Http.Responses.Contracts;
using SIS.WebServer.Results;

namespace IRunesWebApp.Controller
{
    public class BaseController
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
            var content = File.ReadAllText("Views/" + viewName + ".html");
            return new HtmlResult(content, HttpResponseStatusCode.OK);
        }

        protected IHttpResponse BadRequestError(string errorMessage)
        {
            return new HtmlResult($"<h1>{errorMessage}</h1></br><a href=\"/\">Go home</a>", HttpResponseStatusCode.BadRequest);
        }

        protected IHttpResponse ServerError(string errorMessage)
        {
            return new HtmlResult($"<h1>{errorMessage}</h1>", HttpResponseStatusCode.InternalServerError);
        }
    }
}
