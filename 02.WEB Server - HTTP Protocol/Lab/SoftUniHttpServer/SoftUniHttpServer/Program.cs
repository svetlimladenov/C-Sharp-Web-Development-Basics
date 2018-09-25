using System;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Drawing;
using System.Threading.Tasks;

namespace SoftUniHttpServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHttpServer server = new HttpServer();

            server.Start();
        }
    }

    public class HttpServer : IHttpServer
    {
        private bool isWorking;

        private TcpListener tcpListener;

        public HttpServer()
        {
            this.tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 80);
        }

        public void Start()
        {
            this.isWorking = true;
            this.tcpListener.Start();
            Console.WriteLine("Started. 80");
            while (isWorking)
            {
                var client = this.tcpListener.AcceptTcpClient();
                Task.Run((() => ProcessClientAsync(client)));

                //stream.Write(Encoding.UTF8.GetBytes("<h1>@</h1>"));
            }
        }

        private static async void ProcessClientAsync(TcpClient client)
        {
            var buffer = new byte[10240];
            var stream = client.GetStream();
            var readLength = await stream.ReadAsync(buffer, 0, buffer.Length);
            var requestText = Encoding.UTF8.GetString(buffer, 0, readLength);
            Console.WriteLine(new string('=', 50));
            Console.WriteLine(requestText);
            //custom slow-down 
            await Task.Run((() => Thread.Sleep(10000)));
            var responseText = DateTime.Now.ToString(); //File.ReadAllText("index.html");

            var responseBytes = Encoding.UTF8.GetBytes(
                "HTTP/1.1 200 OK"
                + Environment.NewLine
                ////+ "Location: https://softuni.bg" + Replace 200 OK with Moved-Permanently 301
                //// + "Content-Type: text/html"
                ////+ Environment.NewLine
                ////+ "Content-Disposition: attachment; filename=index.html"
                ////+ Environment.NewLine
                + "Content-Length: " + responseText.Length
                + Environment.NewLine
                + Environment.NewLine
                + responseText);

            await stream.WriteAsync(responseBytes);
        }

        public void Stop()
        {
            isWorking = false;
        }
    }

    public interface IHttpServer
    {
        void Start();

        void Stop();
    }
}
