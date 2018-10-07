using System;
using System.Linq;
using CakesWebApp.Services;
using IRunesWebApp.Data;
using SIS.Http.Requests.Contracts;
using IRunesWebApp.Models;
using SIS.Http.Responses.Contracts;
using SIS.WebServer.Results;
using SIS.Http.Cookies;
using IRunesWebApp.GlobalConst;

namespace IRunesWebApp.Services
{
    public class UserService : IUserService
    {
        public UserService()
        {
            this.Context = new IRunesDbContext();
            this.HashService = new HashService();
            this.badRequestService = new BadRequestService();
            this.UserCookiesService = new UserCookieService();
        }
        protected IRunesDbContext Context { get; set; }
        protected IHashService HashService { get; set; }
        protected BadRequestService badRequestService { get; set; }
        protected IUserCookieService UserCookiesService { get; set; }

        public IHttpResponse Login(IHttpRequest request)
        {
            var username = request.FormData["username"].ToString().Trim();
            var password = request.FormData["password"].ToString();

            var hashedPassword = this.HashService.Hash(password);

            var user = this.Context.Users.FirstOrDefault(x => x.Username == username);
            if (user == null)
            {
                return badRequestService.BadRequestError("Invalid username or password.");
            }

            var cookieContent = this.UserCookiesService.GetUserCookie(user.Username);

            var response = new RedirectResult("/");
            //TODO  : Cookie.HTTP ONLY !!!
            var cookie = new HttpCookie(GlobalConstants.userCookieAuthentication, cookieContent, 7) { HttpOnly = true };
            response.Cookies.Add(cookie);
            return response;
        }

        public IHttpResponse Register(IHttpRequest request)
        {
            var username = request.FormData["username"].ToString().Trim();
            var password = request.FormData["password"].ToString();
            var confirmPassword = request.FormData["confirmpassword"].ToString();
            var email = request.FormData["email"].ToString().Trim();

            if (username.Length < 3)
            {
                return this.badRequestService.BadRequestError("Username too short.");
            }

            if (this.Context.Users.Any(x => x.Username == username))
            {
                return this.badRequestService.BadRequestError($"This username - {username}, already exists.");
            }

            if (password.Length < 6)
            {
                return this.badRequestService.BadRequestError("Password too short.");
            }

            if (password != confirmPassword)
            {
                return this.badRequestService.BadRequestError("Password do not match confirtm password.");
            }

            var hashedPassword = HashService.Hash(password);

            var user = new User
            {
                Username = username,
                Password = hashedPassword,
                Email = email
            };

            this.Context.Users.Add(user);

            try
            {
                this.Context.SaveChanges();
            }
            catch (Exception e)
            {
                return this.badRequestService.ServerError(e.Message);
            }

            return new RedirectResult("/");
        }

        public string GetUsername(IHttpRequest request)
        {
            if (!request.Cookies.ContainsCookie(GlobalConstants.userCookieAuthentication))
            {
                return null;
            }
            var usernameCookie = request.Cookies.GetCookie(GlobalConstants.userCookieAuthentication).Value;
            var username = UserCookiesService.GetUserData(usernameCookie);

            return username;
        }
    }
}
