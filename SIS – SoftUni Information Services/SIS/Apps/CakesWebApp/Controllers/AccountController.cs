﻿using System;
using CakesWebApp.Data;
using Microsoft.EntityFrameworkCore.Internal;
using SIS.Http.Enums;
using SIS.Http.Requests;
using SIS.Http.Responses;
using SIS.WebServer.Results;
using System.Linq;
using CakesWebApp.Models;
using CakesWebApp.Services;
using SIS.Http.Cookies;
using SIS.Http.Requests.Contracts;
using SIS.Http.Responses.Contracts;
using SIS.MvcFramework;
using SIS.MvcFramework.Services;

namespace CakesWebApp.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IHashService hashService;

        public AccountController()
        {
            this.hashService = new HashService();
        }

        [HttpGet("/register")]
        public IHttpResponse Register()
        {
            return this.View("Register");
        }

        [HttpPost("/register")]
        public IHttpResponse DoRegister()
        {
            var userName = this.Request.FormData["username"].ToString().Trim();
            var password = this.Request.FormData["password"].ToString();
            var confirmPassword = this.Request.FormData["confirmPassword"].ToString();

            // Validate
            if (string.IsNullOrWhiteSpace(userName) || userName.Length < 4)
            {
                return this.BadRequestError("Please provide valid username with length of 4 or more characters.");
            }

            if (this.Db.Users.Any(x => x.Username == userName))
            {
                return this.BadRequestError("User with the same name already exists.");
            }

            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            {
                return this.BadRequestError("Please provide password of length 6 or more.");
            }

            if (password != confirmPassword)
            {
                return this.BadRequestError("Passwords do not match.");
            }

            // Hash password
            var hashedPassword = this.hashService.Hash(password);

            // Create user
            var user = new User
            {
                Name = userName,
                Username = userName,
                Password = hashedPassword,
            };
            this.Db.Users.Add(user);

            try
            {
                this.Db.SaveChanges();
            }
            catch (Exception e)
            {
                // TODO: Log error
                return this.ServerError(e.Message);
            }

            // TODO: Login

            // Redirect
            return this.Redirect("/");
        }

        [HttpGet("/login")]
        public IHttpResponse Login()
        {
            return this.View("Login");
        }

        [HttpPost("/login")]
        public IHttpResponse DoLogin()
        {
            var userName = this.Request.FormData["username"].ToString().Trim();
            var password = this.Request.FormData["password"].ToString();

            var hashedPassword = this.hashService.Hash(password);

            var user = this.Db.Users.FirstOrDefault(x => 
                x.Username == userName &&
                x.Password == hashedPassword);

            if (user == null)
            {
                return this.BadRequestError("Invalid username or password.");
            }

            var cookieContent = this.UserCookieService.GetUserCookie(user.Username);

            var cookie = new HttpCookie(".auth-cakes", cookieContent, 7) { HttpOnly = true };
            this.Response.Cookies.Add(cookie);
            return this.Redirect("/");
        }

        [HttpGet("/logout")]
        public IHttpResponse Logout()
        {
            if (!this.Request.Cookies.ContainsCookie(".auth-cakes"))
            {
                return this.Redirect("/");
            }

            var cookie = this.Request.Cookies.GetCookie(".auth-cakes");
            cookie.Delete();
            this.Response.Cookies.Add(cookie);
            return this.Redirect("/");
        }
    }
}
