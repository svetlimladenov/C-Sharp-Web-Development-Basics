using System.Linq;
using JudgeWebApp.ViewModels.Contests;
using SIS.HTTP.Responses;

namespace JudgeWebApp.Controllers
{
    public class HomeController : BaseController
    {
        public IHttpResponse Index()
        {
            return this.View();
        }
    }
}
