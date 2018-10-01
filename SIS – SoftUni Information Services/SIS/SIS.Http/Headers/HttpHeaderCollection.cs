namespace SIS.Http.Headers
{
    using System;
    using System.Collections.Generic;
    using Contracts;
    using Common;


    public class HttpHeaderCollection : IHttpHeaderCollection
    {
        private readonly IDictionary<string, HttpHeader> headers;

        public HttpHeaderCollection()
        {
            this.headers = new Dictionary<string, HttpHeader>();
        }

        public void Add(HttpHeader header)
        {
            CoreValidator.ThrowIfNull(header, nameof(header));
            this.headers[header.Key] = header;
        }

        public bool ContainsHeader(string key)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));
            return this.headers.ContainsKey(key);
        }

        public HttpHeader GetHeader(string key)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));
            if (!this.ContainsHeader(key))
            {
                return null;
            }
            return this.headers[key];
        }

        public override string ToString() => string.Join(Environment.NewLine, this.headers.Values);
    }
}
