namespace CakesWebApp.Controllers
{
    using System;
    using Models;
    using Services;
    using SIS.Http.Cookies;
    using System.Linq;
    using SIS.Http.Requests.Contracts;
    using SIS.WebServer.Results;
    using SIS.Http.Responses.Contracts;
    using GlobalConst;
    public class AccountController : BaseController
    {
        private IHashService hashService;

        public AccountController()
        {
            this.hashService = new HashService();
        }

        public IHttpResponse Register(IHttpRequest request)
        {
            return this.View("Register");
        }

        public IHttpResponse DoRegister(IHttpRequest request)
        {
            var username = request.FormData["username"].ToString().Trim();
            var password = request.FormData["password"].ToString();
            var confirmPassword = request.FormData["confirmPassword"].ToString();
            //Validate
            if (string.IsNullOrWhiteSpace(username) || username.Length < 4)
            {
                return this.BadRequestError("Please provide valid username with length 4 or more letters.");
            }

            if (this.Db.Users.Any(x => x.Username == username))
            {
                return this.BadRequestError("User with the same name already exists.");
            }

            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            {
                return this.BadRequestError("Please provide valid password with length 6 or more symbols.");
            }

            if (password != confirmPassword)
            {
                return this.BadRequestError("Passwords do not match.");
            }

            //hash pass
            var hashedPass = this.hashService.Hash(password);

            //create user
            var user = new User
            {
                Name = username,
                Username = username,
                Password = hashedPass,
            };

            this.Db.Add(user);
            try
            {
                this.Db.SaveChanges();
            }
            catch (Exception e)
            {
                //TODO : Log error 
                return this.ServerError(e.Message);
            }

            //TODO : Login

            //Redirect...
            return new RedirectResult("/");
        }

        public IHttpResponse Login(IHttpRequest request)
        {
            return this.View("Login");
        }

        public IHttpResponse DoLogin(IHttpRequest request)
        {
            //Validate user
            //Save cookie/session with user
            //redirect
            var username = request.FormData["username"].ToString().Trim();
            var password = request.FormData["password"].ToString();

            var hashedPassword = this.hashService.Hash(password);

            var user = this.Db.Users.FirstOrDefault(x => x.Username == username && x.Password == hashedPassword);
            if (user == null)
            {
                return this.BadRequestError("Invalid username or password.");
            }

            var cookieContent = this.UserCookieService.GetUserCookie(user.Username);

            var response = new RedirectResult("/");
            response.Cookies.Add(new HttpCookie(GlobalConstants.userCookieName,cookieContent,7));
            return response;
        }

        public IHttpResponse Logout(IHttpRequest request)
        {
            if (!request.Cookies.ContainsCookie(GlobalConstants.userCookieName))
            {
                return new RedirectResult("/");
            }
            var cookie = request.Cookies.GetCookie(GlobalConstants.userCookieName);
            cookie.Delete();
            var response = new RedirectResult("/");
            response.Cookies.Add(cookie);
            return response;
        }
    }
}
