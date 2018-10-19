using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using IRunesWebApp.Data;
using SIS.Http.Enums;
using SIS.Http.Responses.Contracts;
using SIS.WebServer.Results;
using SIS.MvcFramework;

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
