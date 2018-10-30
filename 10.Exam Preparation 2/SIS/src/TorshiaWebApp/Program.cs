using SIS.MvcFramework;

namespace TorshiaWebApp
{
    public class Program
    {
        public static void Main()
        {
            WebHost.Start(new Startup());     
        }
    }
}
