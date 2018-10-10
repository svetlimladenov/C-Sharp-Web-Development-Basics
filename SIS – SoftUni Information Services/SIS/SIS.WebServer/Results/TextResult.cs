namespace SIS.WebServer.Results
{
    using System.Text;
    using Http.Enums;
    using Http.Headers;
    using Http.Responses;

    public class TextResult : HttpResponse
    {
        public TextResult(string content, HttpResponseStatusCode responseStatusCode)
            : base(responseStatusCode)
        {
            this.Headers.Add(new HttpHeader("Content-Type", "text/plain; charset=utf-8"));
            this.Content = Encoding.UTF8.GetBytes(content);

        }
    }
}
