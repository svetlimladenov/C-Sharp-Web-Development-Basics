namespace SIS.WebServer.Results
{
    using System.Text;
    using Http.Enums;
    using Http.Headers;
    using Http.Responses;
    public class HtmlResult : HttpResponse
    {
        public HtmlResult(string content, HttpResponseStatusCode responseStatusCode)
            : base(responseStatusCode)
        {
            this.Headers.Add(new HttpHeader("Content-Type", "text/html"));
            this.Content = Encoding.UTF8.GetBytes(content);
        }
    }
}
