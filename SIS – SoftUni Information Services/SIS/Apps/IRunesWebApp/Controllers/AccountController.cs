using System;
using System.Linq;
using IRunesWebApp.GlobalConst;
using IRunesWebApp.Models;
using IRunesWebApp.ViewModels.Account;
using SIS.Http.Cookies;
using SIS.Http.Requests.Contracts;
using SIS.Http.Responses.Contracts;
using SIS.MvcFramework;
using SIS.WebServer.Results;
using SIS.MvcFramework.Services;

namespace IRunesWebApp.Controller
{
    public class AccountController : BaseController
    {
        private readonly IHashService hashService;

        public AccountController(IHashService hashService)
        {
            this.hashService = hashService;
        }

        [HttpGet("/Users/Login")]
        public IHttpResponse Login()
        {
            if (this.Request.Cookies.GetCookie(GlobalConstants.userCookieAuthentication) != null)
            {
                return this.BadRequestError("You are already logged in.");
            }
            return this.View("Login", "_Layout_LoggedOut");
        }

        [HttpPost("/Users/Login")]
        public IHttpResponse DoLogin(DoLoginInputModel model)
        {

            var hashedPassword = this.hashService.Hash(model.Password);

            var user = this.Db.Users.FirstOrDefault(x => x.Username == model.Username);
            if (user == null)
            {
                return this.BadRequestError("Invalid username or password.");
            }

            if (user.Password != hashedPassword)
            {
                return this.BadRequestError("Invalid username or password.");
            }

            var cookieContent = this.UserCookieService.GetUserCookie(user.Username);
            //TODO  : Cookie.HTTP ONLY !!!
            var cookie = new HttpCookie(GlobalConstants.userCookieAuthentication, cookieContent, 7) { HttpOnly = true };
            this.Response.Cookies.Add(cookie);
            return Redirect("/");
        }

        [HttpGet("/Users/Logout")]
        public IHttpResponse Logout()
        {
            if (!this.Request.Cookies.ContainsCookie(GlobalConstants.userCookieAuthentication))
            {
                return this.Redirect("/");
            }

            var cookie = this.Request.Cookies.GetCookie(GlobalConstants.userCookieAuthentication);
            cookie.Delete();
            this.Response.Cookies.Add(cookie);
            return this.Redirect("/");
        }

        [HttpGet("/Users/Register")]
        public IHttpResponse Register()
        {
            if (this.Request.Cookies.GetCookie(GlobalConstants.userCookieAuthentication) != null)
            {
                return this.BadRequestError("You must log out, if you want to make a new registration.");
            }
            return this.View("Register", "_Layout_LoggedOut");
        }

        [HttpPost("/Users/Register")]
        public IHttpResponse DoRegister(DoRegisterInputModel model)
        {
            var username = model.Username.Trim();
            var password = model.Password;
            var confirmPassword = model.ConfirmPassword;
            var email = model.Email.Trim();

            if (username.Length < 3)
            {
                return this.BadRequestError("Please provide valid username with length of 4 or more characters.");
            }

            if (this.Db.Users.Any(x => x.Username == username))
            {
                return this.BadRequestError($"This username - {username}, already exists.");
            }

            if (password.Length < 6)
            {
                return this.BadRequestError("Please provide valid password with length of 6 or more characters.");
            }

            if (password != confirmPassword)
            {
                return this.BadRequestError("Password do not match confirtm password.");
            }

            var hashedPassword = this.hashService.Hash(password);

            var user = new User
            {
                Username = username,
                Password = hashedPassword,
                Email = email
            };

            this.Db.Users.Add(user);

            try
            {
                this.Db.SaveChanges();
            }
            catch (Exception e)
            {
                return this.BadRequestError(e.Message);             
            }

            return Redirect("/");
        }
    }
}
