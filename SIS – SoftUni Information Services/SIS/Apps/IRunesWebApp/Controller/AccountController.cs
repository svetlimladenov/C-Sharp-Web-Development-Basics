using System;
using System.Linq;
using CakesWebApp.GlobalConst;
using CakesWebApp.Services;
using IRunesWebApp.Models;
using SIS.Http.Cookies;
using SIS.Http.Requests.Contracts;
using SIS.Http.Responses.Contracts;
using SIS.WebServer.Results;

namespace IRunesWebApp.Controller
{
    public class AccountController : BaseController
    {
        private IHashService hashService;

        public AccountController()
        {
            this.hashService = new HashService();
        }
        public IHttpResponse Login(IHttpRequest request)
        {
            return this.View("Login");
        }

        public IHttpResponse DoLogin(IHttpRequest request)
        {
            var username = request.FormData["username"].ToString().Trim();
            var password = request.FormData["password"].ToString();

            var hashedPassword = this.hashService.Hash(password);

            var user = this.Db.Users.FirstOrDefault(x => x.Username == username);
            if (user == null)
            {
                return BadRequestError("Invalid username or password.");
            }

            var cookieContent = this.UserCookieService.GetUserCookie(user.Username);

            var response = new RedirectResult("/");
            //TODO  : Cookie.HTTP ONLY !!!
            var cookie = new HttpCookie(GlobalConstants.userCookieName, cookieContent, 7) { HttpOnly = true };
            response.Cookies.Add(cookie);
            return response;
        }

        public IHttpResponse Logout(IHttpRequest request)
        {
            return this.View("Index");
            //TODO : Finish logout
        }

        public IHttpResponse Register(IHttpRequest request)
        {
            return this.View("Register");
        }

        public IHttpResponse DoRegister(IHttpRequest request)
        {
            var username = request.FormData["username"].ToString().Trim();
            var password = request.FormData["password"].ToString();
            var confirmPassword = request.FormData["confirmpassword"].ToString();
            var email = request.FormData["email"].ToString().Trim();

            if (username.Length < 3 || username.Length > 20)
            {
                return BadRequestError("Username must be more than 3 symbols and less than 20 symbols.");
            }

            if (this.Db.Users.Any(x => x.Username == username))
            {
                return BadRequestError("Username already taken.");
            }

            if (password.Length < 6)
            {
                return BadRequestError("Please provide valid password with length 6 or more symbols.");
            }

            if (password != confirmPassword)
            {
                return BadRequestError("Password does not match confirm password.");
            }

            var hashedPassword = hashService.Hash(password);

            var user = new User
            {
                Username = username,
                HashedPassword = hashedPassword,
                Email = email
            };

            this.Db.Users.Add(user);

            try
            {
                this.Db.SaveChanges();
            }
            catch (Exception e)
            {
                return this.ServerError(e.Message);
            }

            return new RedirectResult("/");
        }
    }
}
