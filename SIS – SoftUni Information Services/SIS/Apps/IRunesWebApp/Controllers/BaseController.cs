using IRunesWebApp.Data;

namespace IRunesWebApp.Controller
{
    public class BaseController : SIS.MvcFramework.Controller
    {       
        public BaseController()
        {
            this.Db = new IRunesDbContext();
        }

        protected IRunesDbContext Db { get; }

    }
}
