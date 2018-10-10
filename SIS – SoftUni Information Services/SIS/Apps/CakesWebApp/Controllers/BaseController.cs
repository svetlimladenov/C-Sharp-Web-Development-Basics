using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CakesWebApp.Data;
using CakesWebApp.Services;
using SIS.Http.Enums;
using SIS.Http.Requests;
using SIS.Http.Requests.Contracts;
using SIS.Http.Responses;
using SIS.Http.Responses.Contracts;
using SIS.WebServer.Results;
using SIS.MvcFramework;

namespace CakesWebApp.Controllers
{
    public abstract class BaseController : Controller
    {
        protected BaseController()
        {
            this.Db = new CakesDbContext();         
        }

        protected CakesDbContext Db { get; }

    }
}
