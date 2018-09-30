namespace WebServer.Server.Http.Response
{
    using System.Text;
    using Enums;
    public abstract class HttpResponse
    {
        private string statusCodemessage => this.StatusCode.ToString();

        protected HttpResponse()
        {
            this.Headers = new HttpHeaderCollection();
        }

        public HttpHeaderCollection Headers { get; }

        public HttpStatusCode StatusCode { get; protected set; }


        public override string ToString()
        {
            var response = new StringBuilder();
            var statusCodeNumber = (int)this.StatusCode;
            response.Append($"HTTP/1.1 {statusCodeNumber} {statusCodemessage}");

            response.AppendLine(this.Headers.ToString());
            response.AppendLine();

            return response.ToString();
        }
    }
}
