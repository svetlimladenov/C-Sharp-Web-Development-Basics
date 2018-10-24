using System;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using MishMashWebApp.Models;
using MishMashWebApp.ViewModels;
using MishMashWebApp.ViewModels.User;
using SIS.HTTP.Cookies;
using SIS.HTTP.Responses;
using SIS.MvcFramework;
using SIS.MvcFramework.Services;

namespace MishMashWebApp.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IHashService hashService;

        public UsersController(IHashService hashService)
        {
            this.hashService = hashService;
        }

        [HttpGet("/Users/Login")]
        public IHttpResponse Login()
        {
            if (this.User != null)
            {
                return this.BadRequestError("First you need to log out.");
            }
            return this.View("Users/Login");
        }

        [HttpPost("/Users/Login")]
        public IHttpResponse DoLogin(DoLoginViewModel model)
        {
            var hashedPassword = this.hashService.Hash(model.Password);
            var user = this.Db.Users
                .FirstOrDefault(x => x.Username == model.Username && x.Password == hashedPassword);
            if (user == null)
            {
                return BadRequestError("Invalid username or password.");
            }

            var cookieContent = this.UserCookieService.GetUserCookie(user.Username);
            var cookie = new HttpCookie(".auth-MishMash", cookieContent, 7) { HttpOnly = true };
            this.Response.Cookies.Add(cookie);
            return this.Redirect("/");
        }

        [HttpGet("/Users/Logout")]
        public IHttpResponse Logout()
        {
            if (!this.Request.Cookies.ContainsCookie(".auth-MishMash"))
            {
                return this.Redirect("/");
            }

            var cookie = this.Request.Cookies.GetCookie(".auth-MishMash");
            cookie.Delete();
            this.Response.Cookies.Add(cookie);
            return this.Redirect("/");
        }

        [HttpGet("/Users/Register")]
        public IHttpResponse Register()
        {
            if (this.User != null)
            {
                return this.BadRequestError("First you need to log out.");
            }
            return this.View("Users/Register");
        }

        [HttpPost("/Users/Register")]
        public IHttpResponse DoRegister(DoRegisterViewModel model)
        {
            if (this.Db.Users.Any(x => x.Username == this.User))
            {
                return this.BadRequestError("User with the same name already exist");
            }

            if (model.Username.Trim().Length < 4 || string.IsNullOrWhiteSpace(model.Username))
            {
                return this.BadRequestError("Please provide valid username with length of 4 or more characters.");
            }

            if (model.Password != model.ConfirmPassword)
            {
                return this.BadRequestError("Passwords do not match.");
            }

            if (model.Password.Trim().Length < 6 || string.IsNullOrWhiteSpace(model.Password))
            {
                return this.BadRequestError("Please provide valid password with length of 6 or more characters.");
            }

            if (string.IsNullOrWhiteSpace(model.Email) || model.Email.Trim().Length < 4)
            {
                return this.BadRequestError("Please provide valid email with length of 4 or more characters.");
            }

            var hashedPassword = this.hashService.Hash(model.Password);
            var role = Role.User;
            if (!this.Db.Users.Any())
            {
                role = Role.Admin;
            }

            var user = new User()
            {
                Username = model.Username,
                Password = hashedPassword,
                Email = model.Email,
                Role = role
            };

            this.Db.Users.Add(user);

            try
            {
                this.Db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return this.Redirect("/Users/Login");
        }
    }
}
