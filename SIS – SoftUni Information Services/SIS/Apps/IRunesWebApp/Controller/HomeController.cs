using System.Text;
using IRunesWebApp.Services;
using SIS.Http.Enums;
using SIS.Http.Requests.Contracts;
using SIS.Http.Responses;
using SIS.Http.Responses.Contracts;
using SIS.WebServer.Results;

namespace IRunesWebApp.Controller
{
    public class HomeController : BaseController
    {
        private readonly UserService userService;

        public HomeController()
        {
            this.userService = new UserService();
        }

        public IHttpResponse Index(IHttpRequest request)
        {
            if (request.Cookies.GetCookie(this.userCookieAuth) == null)
            {
                return this.View("Index");
            }
            else
            {
                var username = userService.GetUsername(request);
                var response = this.View("IndexLogged");
                var view = Encoding.UTF8.GetString(response.Content);
                view = view.Replace("{username}", username);
                return new HtmlResult(view,HttpResponseStatusCode.OK);
            }
            
        }
    }
}
