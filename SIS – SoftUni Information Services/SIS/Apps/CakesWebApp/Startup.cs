﻿using System;
using CakesWebApp.Controllers;
using CakesWebApp.Services;
using SIS.Http.Enums;
using SIS.MvcFramework;
using SIS.MvcFramework.Logger;
using SIS.MvcFramework.Services;
using SIS.WebServer.Routing;

namespace CakesWebApp
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
            collection.AddService<ILogger>((() => new FileLogger($"log_{DateTime.Now:yyyy-dd-M}.txt")));
        }
    }
}
