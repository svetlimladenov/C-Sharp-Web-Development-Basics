﻿using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SIS.Http.Enums;
using SIS.Http.Requests;
using SIS.Http.Requests.Contracts;
using SIS.Http.Responses;
using SIS.Http.Responses.Contracts;
using SIS.WebServer.Routing;

namespace SIS.WebServer
{
    public class ConnectionHandler
    {
        private readonly Socket client;

        private readonly ServerRoutingTable serverRoutingTable;

        public ConnectionHandler(Socket client, ServerRoutingTable serverRoutingTable)
        {
            this.client = client;
            this.serverRoutingTable = serverRoutingTable;
        }    

        private async Task<IHttpRequest> ReadRequest()
        {
            var result = new StringBuilder();
            var data = new ArraySegment<byte>(new byte[1024]);
            while (true)
            {
                int numberOfBytesRead = await this.client.ReceiveAsync(data.Array, SocketFlags.None);

                if (numberOfBytesRead == 0)
                {
                    break;
                }

                var bytesAsString = Encoding.UTF8.GetString(data.Array, 0, numberOfBytesRead);
                result.Append(bytesAsString);

                if (numberOfBytesRead < 1023)
                {
                    break;
                }
            }

            if (result.Length == 0)
            {
                return null;
            }

            return new HttpRequest(result.ToString());
        }

        private IHttpResponse HandleRequest(IHttpRequest httpRequest)
        {
            if (!this.serverRoutingTable.Reoutes.ContainsKey(httpRequest.RequestMethod) ||
                !this.serverRoutingTable.Reoutes[httpRequest.RequestMethod].ContainsKey(httpRequest.Path))
            {
                return new HttpResponse(HttpResponseStatusCode.NotFound);
            }

            return this.serverRoutingTable.Reoutes[httpRequest.RequestMethod][httpRequest.Path].Invoke(httpRequest);
        }

        private async Task PrepareResponse(IHttpResponse httpResponse)
        {
            byte[] byteSegments = httpResponse.GetBytes();

            await this.client.SendAsync(byteSegments, SocketFlags.None);
        }

        public async Task ProcessRequestAsync()
        {
            var httpRequest = await this.ReadRequest();

            if (httpRequest != null)
            {
                var httpResponse = this.HandleRequest(httpRequest);

                await this.PrepareResponse(httpResponse);
            }

            this.client.Shutdown(SocketShutdown.Both);
        }
    }
}