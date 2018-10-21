using System.Linq;
using CakesWebApp.ViewModels.User;
using SIS.Http.Responses.Contracts;
using SIS.MvcFramework;

namespace CakesWebApp.Controllers
{
    public class UserController : BaseController
    {
        [HttpGet("user/profile")]
        public IHttpResponse Profile()
        {
            var viewModel = this.Db.Users.Where(x => x.Username == this.User)
                .Select(x => new ProfileViewModel
                {
                    Username = x.Username,
                    RegisteredOn = x.DateOfRegistration,
                    OrdersCount = x.Orders.Count,
                }).FirstOrDefault();

            if (viewModel == null)
            {
                this.BadRequestError("Profile page not accessible for this user.");
            }

            //TODO Create view model
            return this.View("Profile", viewModel);
        }
    }
}
