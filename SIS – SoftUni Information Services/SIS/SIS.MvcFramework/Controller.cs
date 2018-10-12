using System.Collections.Generic;
using System.Text;
using CakesWebApp.Services;
using SIS.Http.Enums;
using SIS.Http.Headers;
using SIS.Http.Requests.Contracts;
using SIS.Http.Responses;
using SIS.Http.Responses.Contracts;

namespace SIS.MvcFramework
{
    public abstract class Controller
    {
        protected Controller()
        {
            this.UserCookieService = new UserCookieService();
            this.Response = new HttpResponse { StatusCode = HttpResponseStatusCode.OK };
        }

        public IHttpRequest Request { get; set; }

        public IHttpResponse Response { get; set; }

        protected string User
        {
            get
            {
                if (!this.Request.Cookies.ContainsCookie(".auth-IRunes"))
                {
                    return null;
                }
                //".auth-IRunes"
                //.auth-cakes
                var cookie = this.Request.Cookies.GetCookie(".auth-IRunes");
                var cookieContent = cookie.Value;
                var userName = this.UserCookieService.GetUserData(cookieContent);
                return userName;
            }
        }

        protected IUserCookieService UserCookieService { get; }

        protected IHttpResponse View(string viewName, IDictionary<string, string> viewBag = null)
        {
            if (viewBag == null)
            {
                viewBag = new Dictionary<string, string>();
            }

            var allContent = this.GetViewContent(viewName, viewBag);
            this.PrepareHtmlResult(allContent);
            return this.Response;
        }

        protected IHttpResponse ViewLoggedOut(string viewName, IDictionary<string, string> viewBag = null)
        {
            if (viewBag == null)
            {
                viewBag = new Dictionary<string, string>();
            }
            var allContent = this.GetViewContentLoggedOut(viewName);
            this.PrepareHtmlResult(allContent);
            return this.Response;
        }


        protected IHttpResponse File(byte[] content)
        {
            this.Response.Headers.Add(new HttpHeader(HttpHeader.ContentLength, content.Length.ToString()));
            this.Response.Headers.Add(new HttpHeader(HttpHeader.ContentDisposition, "inline"));
            this.Response.Content = content;
            return this.Response;
        }

        protected IHttpResponse Redirect(string location)
        {
            this.Response.Headers.Add(new HttpHeader(HttpHeader.Location, location));
            this.Response.StatusCode = HttpResponseStatusCode.SeeOther; // TODO: Found better?
            return this.Response;
        }

        protected IHttpResponse Text(string content)
        {
            this.Response.Headers.Add(new HttpHeader(HttpHeader.ContentType, "text/plain; charset=utf-8"));
            this.Response.Content = Encoding.UTF8.GetBytes(content);
            return this.Response;
        }

        protected IHttpResponse BadRequestError(string errorMessage)
        {
            var viewBag = new Dictionary<string, string>();
            viewBag.Add("Error", errorMessage);
            var allContent = this.GetViewContent("Error", viewBag);
            this.PrepareHtmlResult(allContent);
            this.Response.StatusCode = HttpResponseStatusCode.BadRequest;
            return this.Response;
        }

        protected IHttpResponse ServerError(string errorMessage)
        {
            var viewBag = new Dictionary<string, string>();
            viewBag.Add("Error", errorMessage);
            var allContent = this.GetViewContent("Error", viewBag);
            this.PrepareHtmlResult(allContent);
            this.Response.StatusCode = HttpResponseStatusCode.InternalServerError;
            return this.Response;
        }

        private string GetViewContent(string viewName,
            IDictionary<string, string> viewBag)
        {
            var layoutContent = System.IO.File.ReadAllText("Views/_Layout.html");
            var content = System.IO.File.ReadAllText("Views/" + viewName + ".html");
            foreach (var item in viewBag)
            {
                content = content.Replace("@Model." + item.Key, item.Value);
            }

            var allContent = layoutContent.Replace("@RenderBody()", content);
            return allContent;
        }

        private string GetViewContentLoggedOut(string viewName)
        {
            var layoutContent = System.IO.File.ReadAllText("Views/_Layout_LoggedOut.html");
            var content = System.IO.File.ReadAllText("Views/" + viewName + ".html");
            var allContent = layoutContent.Replace("@RenderBody()", content);
            return allContent;
        }


        private void PrepareHtmlResult(string content)
        {
            this.Response.Headers.Add(new HttpHeader(HttpHeader.ContentType, "text/html; charset=utf-8"));
            this.Response.Content = Encoding.UTF8.GetBytes(content);
        }
    }

}
