namespace SIS.Http.Responses.Contracts
{
    using Cookies;
    using Enums;
    using Headers;
    using Headers.Contracts;
    using Cookies.Contracts;
    public interface IHttpResponse
    {
        HttpResponseStatusCode StatusCode { get; }

        IHttpHeaderCollection Headers { get; }

        IHttpCookieCollection Cookies { get; }

        byte[] Content { get; set; }

        void AddHeader(HttpHeader header);

        void AddCookie(HttpCookie cookie);

        byte[] GetBytes();
    }
}
