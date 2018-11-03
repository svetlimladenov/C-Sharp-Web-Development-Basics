using SIS.MvcFramework;

namespace ChushkaWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebHost.Start(new Startup());        
        }
    }
}
