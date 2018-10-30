using JudgeWebApp.Data;
using SIS.MvcFramework;

namespace JudgeWebApp.Controllers
{
    public abstract class BaseController : Controller
    {
        public BaseController()
        {
            this.Db = new ApplicationDbContext();
        }
        public ApplicationDbContext Db { get; }
    }
}
