using System.Linq;
using JudgeWebApp.ViewModels;
using JudgeWebApp.ViewModels.Account;
using Microsoft.EntityFrameworkCore;
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
            var user = this.Db.Users.Include(u => u.Submissions).ThenInclude(s => s.Contest).FirstOrDefault(u => u.Username == this.User.Username);
            var submissions = user.Submissions.ToArray();
            var firstS = submissions[0].Contest;
            var viewModel = new MyProfileViewModel()
            {
                Username = user.Username,
                Email = user.Email.Replace("@", "&#64;"),
                Age = user.Age,
                ProfilePicture = user.ProfilePictureUrl,
                Submissions = submissions
            };
            return this.View("Accounts/MyProfile", viewModel);
        }
    }
}
