using SIS.MvcFramework;
using SIS.MvcFramework.Logger;
using SIS.MvcFramework.Services;

namespace MishMashWebApp
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
            collection.AddService<ILogger, ConsoleLogger>();
        }
    }
}
