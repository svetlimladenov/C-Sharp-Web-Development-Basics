using System.Net.Security;
using SIS.Http.Cookies;

namespace SIS.Http.Requests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using Cookies.Contracts;
    using Common;
    using Enums;
    using Exceptions;
    using Extensions;
    using Headers;
    using Headers.Contracts;
    using Contracts;

    public class HttpRequest : IHttpRequest
    {
        public HttpRequest(string requestString)
        {
            CoreValidator.ThrowIfNullOrEmpty(requestString, nameof(requestString));
            this.FormData = new Dictionary<string, object>();
            this.QueryData = new Dictionary<string, object>();
            this.Headers = new HttpHeaderCollection();
            this.Cookies = new HttpCookieCollection();

            this.ParseRequest(requestString);
            //TODO: Parse request data..
        }
        public string Path { get; private set; }

        public string Url { get; private set; }

        public Dictionary<string, object> FormData { get; }

        public Dictionary<string, object> QueryData { get; }

        public IHttpHeaderCollection Headers { get; }

        public IHttpCookieCollection Cookies { get; }

        public HttpRequestMethod RequestMethod { get; private set; }

        private bool IsValidRequestLine(string[] requestLine)
        {
            if (requestLine.Length == 3 && requestLine[2] == "HTTP/1.1")
            {
                return true;
            }

            return false;
        }

        private bool IsValidRequestQueryString(string queryString, string[] queryParameters)
        {
            if (!string.IsNullOrEmpty(queryString) || queryParameters.Any())
            {
                return true;
            }

            return false;
        }

        private void ParseRequestMethod(string[] requestLine)
        {
            var method = requestLine[0].Capitalize();
            //this.RequestMethod = Enum.Parse<HttpRequestMethod>(method);
            HttpRequestMethod parsedMethod;
            if (!Enum.TryParse(method, true, out parsedMethod))
            {
                throw new BadRequestException("Request Method does not present in our Http Request Methods");
            }

            this.RequestMethod = parsedMethod;
        }

        private void ParseRequestUrl(string[] requestLine)
        {
            this.Url = requestLine[1];
        }

        private void ParseRequestPath()
        {
            this.Path = this.Url.Split(new[] { '?', '#' }, StringSplitOptions.RemoveEmptyEntries)[0];
        }

        private void ParseHeaders(string[] requestContent)
        {
            var emptyLineAfterHeadersIndex = Array.IndexOf(requestContent, "\r");//requestContent.Length - 1;
            for (int i = 0; i < emptyLineAfterHeadersIndex; i++)
            {
                var headerParts = requestContent[i].Split(new[] { ": " }, StringSplitOptions.RemoveEmptyEntries);
                if (headerParts.Length != 2)
                {
                    throw new BadRequestException("Wrong headers request.");
                }
                var headerKey = headerParts[0];
                var headerValue = headerParts[1].Trim();
                var header = new HttpHeader(headerKey, headerValue);
                this.Headers.Add(header);
            }

            if (!this.Headers.ContainsHeader("Host"))
            {
                throw new BadRequestException("HeadersCollection must contain a Host header");
            }
        }

        private void ParseQueryParameters()
        {
            if (!this.Url.Contains('?'))
            {
                return;
            }
            var query = this.Url.Split(new[] { '?' }, StringSplitOptions.RemoveEmptyEntries).Last();
            //register/?name=Ivan&age=12
            this.ParseQuery(query, this.QueryData);
        }

        private void ParseFormDataParameters(string formData)
        {
            if (this.RequestMethod == HttpRequestMethod.Get)
            {
                return;
            }

            this.ParseQuery(formData, this.FormData);
        }

        private void ParseQuery(string query, Dictionary<string, object> dictionary)
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

        private void ParseCookies()
        {
            if (!this.Headers.ContainsHeader("Cookie"))
            {
                return;
            }
            //Cookie: lang=en; to=GH;
            var cookiesHeaderSplit = this.Headers.GetHeader("Cookie").Value;


            //lang = en; to = GH;
            var cookies = cookiesHeaderSplit.Split("; ", StringSplitOptions.RemoveEmptyEntries);
            foreach (var cookie in cookies)
            {
                var cookieParts = cookie.Split('=', 2, StringSplitOptions.RemoveEmptyEntries);
                if (cookieParts.Length == 2)
                {
                    var cookiesName = cookieParts[0];
                    var cookieValue = cookieParts[1];
                    if (!this.Cookies.ContainsCookie(cookiesName))
                    {
                        this.Cookies.Add(new HttpCookie(cookiesName, cookieValue, false));
                    }
                }
                else
                {
                    var cookiesName = cookieParts[0];
                    if (!this.Cookies.ContainsCookie(cookiesName))
                    {
                        this.Cookies.Add(new HttpCookie(cookiesName,string.Empty, false));
                    }
                }
            }

        }

        private void ParseRequest(string requestString)
        {
            var splitRequestContent = requestString
                .Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            var requestLine = splitRequestContent[0].Trim()
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (!this.IsValidRequestLine(requestLine))
            {
                throw new BadRequestException();
            }

            this.ParseRequestMethod(requestLine);
            this.ParseRequestUrl(requestLine);
            this.ParseRequestPath();

            this.ParseHeaders(splitRequestContent.Skip(1).ToArray());
            this.ParseCookies();
            this.ParseQueryParameters();
            this.ParseFormDataParameters(splitRequestContent.Last());
        }


    }


}
