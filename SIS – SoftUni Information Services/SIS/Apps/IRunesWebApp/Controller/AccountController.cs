using System;
using System.Linq;
using CakesWebApp.GlobalConst;
using CakesWebApp.Services;
using IRunesWebApp.Models;
using SIS.Http.Cookies;
using SIS.Http.Requests.Contracts;
using SIS.Http.Responses.Contracts;
using SIS.WebServer.Results;
using IRunesWebApp.Services;
namespace IRunesWebApp.Controller
{
    public class AccountController : BaseController
    {
        private IHashService hashService;
        private readonly IUserService userService;
        private readonly BadRequestService badRequestService;

        public AccountController()
        {
            this.hashService = new HashService();
            this.userService = new UserService();
            this.badRequestService = new BadRequestService();; 
        }

        public IHttpResponse Login(IHttpRequest request)
        {
            if (request.Cookies.GetCookie(this.userCookieAuth) != null)
            {
                return this.badRequestService.BadRequestError("You are already logged in.");
            }
            return this.View("Login");
        }

        public IHttpResponse DoLogin(IHttpRequest request)
        {
            var response = this.userService.Login(request);
            return response;
        }

        public IHttpResponse Logout(IHttpRequest request)
        {
            if (!request.Cookies.ContainsCookie(this.userCookieAuth))
            {
                return new RedirectResult("/");
            }
            var cookie = request.Cookies.GetCookie(this.userCookieAuth);
            cookie.Delete();
            var response = new RedirectResult("/");
            response.Cookies.Add(cookie);
            return response;
        }

        public IHttpResponse Register(IHttpRequest request)
        {
            if (request.Cookies.GetCookie(this.userCookieAuth) != null)
            {
                return this.badRequestService.BadRequestError("You must log out, if you want to make a new registration.");
            }

            return this.View("Register");
        }

        public IHttpResponse DoRegister(IHttpRequest request)
        {
            var response = this.userService.Register(request);
            return response;
        }
    }
}
