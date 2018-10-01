using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CookiesLab
{
    public class HttpServer : IHttpServer
    {
        private bool isWorking;

        private readonly TcpListener tcpListener;

        private readonly RequestProcesser requestProcesser;

        public HttpServer()
        {
            this.tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 80);
            this.requestProcesser = new RequestProcesser();
        }

        public void Start()
        {
            this.isWorking = true;
            this.tcpListener.Start();
            Console.WriteLine("Started. 80");
            while (isWorking)
            {
                var client = this.tcpListener.AcceptTcpClient();

#pragma warning disable 4014
                this.requestProcesser.ProcessClientAsync(client);
#pragma warning restore 4014

                //stream.Write(Encoding.UTF8.GetBytes("<h1>@</h1>"));
            }
        }



        public void Stop()
        {
            isWorking = false;
        }

        public class SessionManager
        {
            private readonly IDictionary<string, int>
                sessionData = new ConcurrentDictionary<string, int>();

            public void CreateSession(string sessionId)
            {
                this.sessionData.Add(sessionId, 0);
            }

            public int GetSession(string sessionId)
            {
                this.sessionData.TryGetValue(sessionId, out int value);
                return value;
            }

            public void SetSessionData(string sessionId, int newValue)
            {
                this.sessionData[sessionId] = newValue;
            }

            public bool Exists(string sessionId)
            {
                return this.sessionData.TryGetValue(sessionId, out _);
            }
        }

        public class RequestProcesser
        {
            private SessionManager sessionManager = new SessionManager();
            object lockObj = new object();

            public async Task ProcessClientAsync(TcpClient client)
            {
                var buffer = new byte[10240];
                var stream = client.GetStream();
                var readLength = await stream.ReadAsync(buffer, 0, buffer.Length);
                var requestText = Encoding.UTF8.GetString(buffer, 0, readLength);

                var sessionId = ParseSessionId(requestText);

                string sessionSetCookie = null;
                Console.WriteLine(new string('=', 50));
                Console.WriteLine(requestText);

                if (!sessionManager.Exists(sessionId))
                {
                    var newSessionId = Guid.NewGuid().ToString();
                    sessionManager.GetSession(newSessionId);
                    sessionSetCookie = "Set-Cookie: SessionId=" + newSessionId + "; Max-Age=5000000" + ";" + Environment.NewLine;
                }
                else
                {
                    lock (lockObj)
                    {
                        var data = sessionManager.GetSession(sessionId);
                        data++;
                        sessionManager.SetSessionData(sessionId, data);
                    }

                }

                var sessionData = sessionManager.GetSession(sessionId);

                var responseText =  "Hello, " + sessionId + "   " + sessionData;

                var responseBytes = Encoding.UTF8.GetBytes(
                    "HTTP/1.1 200 OK"
                    + Environment.NewLine
                    + "Content-Length: " + responseText.Length
                    + Environment.NewLine
                    + sessionSetCookie
                    ////+ DateTime.UtcNow.AddMinutes(1).ToString("R")
                    + "Content-Type: text/html"
                    + Environment.NewLine
                    + Environment.NewLine
                    + responseText);

                await stream.WriteAsync(responseBytes);
            }

            private string ParseSessionId(string requestText)
            {
                StringReader sr = new StringReader(requestText);
                var line = string.Empty;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.StartsWith("Cookie: "))
                    {
                        var lineParts = line.Split(": ", StringSplitOptions.RemoveEmptyEntries);
                        if (lineParts.Length == 2)
                        {
                            var cookies = lineParts[1].Split("; ", StringSplitOptions.RemoveEmptyEntries);
                            foreach (var cookie in cookies)
                            {
                                var cookieParts = cookie.Split('=', 2, StringSplitOptions.RemoveEmptyEntries);
                                if (cookieParts.Length == 2)
                                {
                                    var cookiesName = cookieParts[0];
                                    var cookieValue = cookieParts[1];
                                    if (cookiesName == "SessionId")
                                    {
                                        return cookieValue;
                                    }
                                }
                            }
                        }
                    }
                }

                return null;
            }

        }
    }
}