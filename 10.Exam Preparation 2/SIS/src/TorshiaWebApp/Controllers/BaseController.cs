using SIS.MvcFramework;
using TorshiaWebApp.Data;

namespace TorshiaWebApp.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {
            this.Db = new ApplicationDbContext();
        }
        protected ApplicationDbContext Db { get; }
    }
}
