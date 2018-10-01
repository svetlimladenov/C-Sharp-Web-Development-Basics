using System.Net.Mime;
using System.Threading;
using System.Drawing;

namespace CookiesLab
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHttpServer server = new HttpServer();

            server.Start();
        }
    }
}
