using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore.Internal;
using SIS.HTTP.Responses;

namespace JudgeWebApp.Controllers
{
    public class AccountsController : BaseController
    {
        public IHttpResponse Profile(int id)
        {
            var currentUser = this.Db.Users.FirstOrDefault(u => u.Id == id);
            if (currentUser == null)
            {
                return BadRequestError("Invalid user.");
            }
            if (this.User.Username == currentUser.Username)
            {
                return MyProfile();
            }


            return this.View();
        }

        public IHttpResponse MyProfile()
        {
            return this.View("Accounts/MyProfile");
        }
    }
}
