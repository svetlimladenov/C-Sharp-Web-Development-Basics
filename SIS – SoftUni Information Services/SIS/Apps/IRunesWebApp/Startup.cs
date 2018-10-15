﻿using System;
using System.Collections.Generic;
using System.Text;
using CakesWebApp.Services;
using IRunesWebApp.Controller;
using SIS.Http.Enums;
using SIS.MvcFramework;
using SIS.MvcFramework.Logger;
using SIS.MvcFramework.Services;
using SIS.WebServer.Routing;

namespace IRunesWebApp
{
    public class Startup : IMvcApplication
    {
        public void Configure()
        {

        }

        public void ConfigureServices(IServiceCollection collection)
        {
            collection.AddService<IHashService, HashService>();
            collection.AddService<IUserCookieService, UserCookieService>();
            collection.AddService<ILogger, FileLogger>();
        }
    }
}
