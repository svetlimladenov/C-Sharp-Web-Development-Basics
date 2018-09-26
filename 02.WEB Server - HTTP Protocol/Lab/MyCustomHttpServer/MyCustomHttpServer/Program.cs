using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using HttpMachine;
namespace MyCustomHttpServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var port = 1337;
            var ipAddress = IPAddress.Parse("127.0.0.1");

            var tcpListender = new TcpListener(ipAddress, port);
            tcpListender.Start();

            Task.Run(async () =>
            {
                await Connect(tcpListender);
            })
                .GetAwaiter()
                .GetResult();
        }

        public static async Task Connect(TcpListener listener)
        {
            while (true)
            {
                var client = await listener.AcceptTcpClientAsync();
                var buffer = new byte[1024];
                await client.GetStream().ReadAsync(buffer, 0, buffer.Length);
                var clientMessege = Encoding.UTF8.GetString(buffer);
                Console.WriteLine(clientMessege);

                var parsed = clientMessege.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var path = parsed[1];

                var responseText = "Page Not Found";
                var statusCode = 404;
                if (path == "/index" || path == "/index.html")
                {
                    responseText = File.ReadAllText("BeerStore\\index.html");
                    statusCode = 200;
                }
                else if (path == "/all-beers.html")
                {
                    responseText = File.ReadAllText("BeerStore\\all-beers.html");
                    statusCode = 200;
                }
                else if (path == "/style/style.css")
                {
                    responseText = File.ReadAllText("BeerStore\\style\\style.css");
                    statusCode = 200;
                }

                var response = $"HTTP/1.1 {statusCode} OK" + Environment.NewLine
                                        + "Content-Length: " + responseText.Length + Environment.NewLine
                                        + "Content-Type: text/html" + Environment.NewLine + Environment.NewLine
                                        + responseText;
                var responseBytes = Encoding.UTF8.GetBytes(response);
                await client.GetStream().WriteAsync(responseBytes, 0, responseBytes.Length);
                client.Dispose();
            }
        }

        public interface IHttpParserDelegate
        {
            void OnMessageBegin(HttpParser parser);
            void OnHeaderName(HttpParser parser, string name);
            void OnHeaderValue(HttpParser parser, string value);
            void OnHeadersEnd(HttpParser parser);
            void OnBody(HttpParser parser, ArraySegment<byte> data);
            void OnMessageEnd(HttpParser parser);
        }

        public interface IHttpRequestParserDelegate : IHttpParserDelegate
        {
            void OnMethod(HttpParser parser, string method);
            void OnRequestUri(HttpParser parser, string requestUri);
            void OnPath(HttpParser parser, string path);
            void OnFragment(HttpParser parser, string fragment);
            void OnQueryString(HttpParser parser, string queryString);
        }

        public interface IHttpResponseParserDelegate : IHttpParserDelegate
        {
            void OnResponseCode(HttpParser parser, int statusCode, string statusReason);
        }
    }
}


