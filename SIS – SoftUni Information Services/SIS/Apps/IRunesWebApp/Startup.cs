using System;
using SIS.Http.Enums;
using SIS.MvcFramework;
using SIS.MvcFramework.Logger;
using SIS.MvcFramework.Services;

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
            collection.AddService<ILogger>((() => new FileLogger($"log_IRunes_{DateTime.Now:yyyy-dd-M}.txt")));
        }
    }
}
