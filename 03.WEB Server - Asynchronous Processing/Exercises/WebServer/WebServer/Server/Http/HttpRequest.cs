namespace WebServer.Server.Http
{    
    using Exceptions;
    using Http;
    using Common;
    using Enums;
    using Contracts;
    using System;
    using System.Net;
    using System.Linq;
    using System.Collections.Generic;

    public class HttpRequest : IHttpRequest
    {
        public HttpRequest(string requestText)
        {
            CoreValidator.ThrowIfNullOrEmpty(requestText, nameof(requestText));
            this.FormData = new Dictionary<string, string>();
            this.Headers = new HttpHeaderCollection();
            this.QueryParameters = new Dictionary<string, string>();
            this.UrlParameters = new Dictionary<string, string>();

            this.ParseRequest(requestText);
        }

        public IDictionary<string, string> FormData { get; private set; }
        public HttpHeaderCollection Headers { get; private set; }
        public string Path { get; private set; }
        public IDictionary<string, string> QueryParameters { get; private set; }
        public HttpRequestMethod Method { get; private set; }
        public string Url { get; private set; }
        public IDictionary<string, string> UrlParameters { get; private set; }
        public void AddUrlParameter(string key, string value)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));
            CoreValidator.ThrowIfNullOrEmpty(value, nameof(value));
            this.UrlParameters[key] = value;
        }

        private void ParseRequest(string requestText)
        {
            //cats/{method}/{id} - CatsController, Index();
            //cats/{edit}/{500} - method = edit; id = 500;
             //cats/{delete}/{300} - method = delete; id = 300;

            var requestLines = requestText.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            if (!requestLines.Any())
            {
                BadRequestException.ThrowFromInvalidRequest();
            }
            var requestLine = requestLines.First().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (requestLine.Length != 3 || requestLines[2].ToLower() != "http/1.1")
            {
                BadRequestException.ThrowFromInvalidRequest();
            }

            this.Url = requestLines[1];
            this.Method = this.ParseMethod(requestLines.First());
            this.Path = this.ParsePath(this.Url);
            this.ParseHeaders(requestLine);
            this.ParseParameters();
            this.ParseFormData(requestLine.Last());
        }


        private HttpRequestMethod ParseMethod(string method)
        {
            HttpRequestMethod parsedMethod;
            if (!Enum.TryParse(method, true, out parsedMethod))
            {
                BadRequestException.ThrowFromInvalidRequest();
            }

            return parsedMethod;
        }

        private string ParsePath(string url)
        {
            return url.Split(new[] { '?', '#' }, StringSplitOptions.RemoveEmptyEntries)[0];
        }

        private void ParseHeaders(string[] requestLine)
        {
            //GET ....
            //h1
            //h2
            //h3
            //...
            //
            //Request Content
            var emptyLineAfterHeadersIndex = Array.IndexOf(requestLine, string.Empty);
            for (int i = 1; i < emptyLineAfterHeadersIndex; i++)
            {
                var headerParts = requestLine[i].Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                if (headerParts.Length != 2)
                {
                    BadRequestException.ThrowFromInvalidRequest();
                }
                var headerKey = headerParts[0];
                var headerValue = headerParts[0].Trim();
                var header = new HttpHeader(headerKey, headerValue);
                this.Headers.Add(header);
            }

            if (!this.Headers.ContainsKey("Host"))
            {
                BadRequestException.ThrowFromInvalidRequest();
            }
        }

        private void ParseParameters()
        {
            if (this.Url.Contains('?'))
            {
                return;
            }
            var query = this.Url.Split(new[] { '?' }, StringSplitOptions.RemoveEmptyEntries).Last();
            //register/?name=Ivan&age=12
            this.ParseQuery(query, this.UrlParameters);
        }


        private void ParseFormData(string formDataLine)
        {
            if (this.Method == HttpRequestMethod.Get)
            {
                return;
            }
            //in form ===> username=gosho
            this.ParseQuery(formDataLine, this.FormData);
        }

        private void ParseQuery(string query, IDictionary<string, string> dictionary)
        {

            if (!query.Contains('='))
            {
                return;
            }

            var queryPairs = query.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var queryPair in queryPairs)
            {
                var queryKvp = queryPair.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                var queryKey = WebUtility.UrlDecode(queryKvp[0]);
                var queryValue = WebUtility.UrlDecode(queryKvp[1]);

                if (queryKvp.Length != 2)
                {
                    return;
                }
                dictionary.Add(queryKey, queryValue);
            }
        }

    }
}
