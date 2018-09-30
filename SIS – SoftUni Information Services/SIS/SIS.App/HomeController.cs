namespace SIS.App
{
    using Http.Enums;
    using Http.Responses.Contracts;
    using WebServer.Results;

    public class HomeController
    {
        public IHttpResponse Index()
        {
            var content = "<h1>Hello, world</h1>";
            return new HtmlResult(content, HttpResponseStatusCode.OK);
        }
    }
}
