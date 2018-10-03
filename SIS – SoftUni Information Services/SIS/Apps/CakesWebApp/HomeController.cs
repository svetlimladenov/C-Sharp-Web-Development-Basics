using System;
using System.Collections.Generic;
using System.Text;
using SIS.Http.Enums;
using SIS.Http.Responses.Contracts;
using SIS.WebServer.Results;

namespace CakesWebApp
{
    public class HomeController
    {
        public IHttpResponse Index()
        {
            var content = "<h1>Hello, world</h1></br><a href='/home'>to Home page</a>";
            return new HtmlResult(content, HttpResponseStatusCode.OK);
        }

        public IHttpResponse Home()
        {
            var content = "<h1> You are on Home Page</h1>";
            return new HtmlResult(content, HttpResponseStatusCode.OK);
        }
    }
}
