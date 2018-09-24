using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MyCustomHttpServer
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
            this.tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"),443);
        }

        public void Start()
        {
            this.isWorking = true;
            tcpListener.Start();
            Console.WriteLine("Started. 443");
            while (true)
            {
                var client = this.tcpListener.AcceptTcpClient();
                var buffer = new byte[10240];
                var stream = client.GetStream();
                var readLenght = stream.Read(buffer, 0, buffer.Length);
                var requestText = Encoding.UTF8.GetString(buffer,0 , readLenght);
                Console.WriteLine(new string('=',50));
                Console.WriteLine(requestText);

                var responseText = File.ReadAllText("form.html");

                var responseBytes = Encoding.UTF8.GetBytes(
                    "HTTP/1.0 200 OK" + Environment.NewLine
                    + "Content-Type: text/html" + Environment.NewLine
                    + "Content-Length: " + responseText.Length + Environment.NewLine
                    + Environment.NewLine
                    + responseText
                    );
                stream.Write(responseBytes);
            }
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
