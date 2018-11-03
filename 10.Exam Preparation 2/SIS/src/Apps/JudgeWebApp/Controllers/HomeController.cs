using SIS.HTTP.Responses;

namespace JudgeWebApp.Controllers
{
    public class HomeController : BaseController
    {
        public IHttpResponse Index()
        {
            if (!this.User.IsLoggedIn)
            {
               return this.View("Home/IndexLoggedOut"); 
            }

            return this.View();
        }
    }
}
