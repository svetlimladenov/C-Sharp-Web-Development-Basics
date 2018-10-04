using System.IO;
using CakesWebApp.Data;
using SIS.Http.Enums;
using SIS.Http.Requests.Contracts;
using SIS.Http.Responses;
using SIS.Http.Responses.Contracts;
using SIS.WebServer.Results;
using CakesWebApp.GlobalConst;
using CakesWebApp.Services;

namespace CakesWebApp.Controllers
{
    public abstract class BaseController
    {

        protected BaseController()
        {
            this.Db = new CakesDbContext();
            this.UserCookieService = new UserCookieService();
        }

        protected CakesDbContext Db { get; }

        protected IUserCookieService UserCookieService { get; }
        protected string GetUsername(IHttpRequest request)
        {
            if (!request.Cookies.ContainsCookie(GlobalConstants.userCookieName))
            {
                return null;
            }
            var cookie = request.Cookies.GetCookie(GlobalConstants.userCookieName);
            var cookieContent = cookie.Value;
            var userName = this.UserCookieService.GetUserData(cookieContent);

            return userName;
        }

        protected IHttpResponse View(string viewName)
        {
            var content = File.ReadAllText("Views/" + viewName + ".html");
            return new HtmlResult(content, HttpResponseStatusCode.OK);
        }

        protected IHttpResponse BadRequestError(string errorMessage)
        {
            return new HtmlResult($"<h1>{errorMessage}</h1>", HttpResponseStatusCode.BadRequest);
        }

        protected IHttpResponse ServerError(string errorMessage)
        {
            return new HtmlResult($"<h1>{errorMessage}</h1>", HttpResponseStatusCode.InternalServerError);
        }
    }
}
