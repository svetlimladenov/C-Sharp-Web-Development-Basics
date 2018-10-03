namespace WebServer.Server.Http
{
    using Common;
    using System.Collections.Generic;
    using Contracts;
    using Http;
    using System;


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
            headers[header.Key] = header;
        }

        public bool ContainsKey(string key)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));

            return this.headers.ContainsKey(key);
        }

        public HttpHeader Get(string key)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));
            if (!this.ContainsKey(key))
            {
                throw new InvalidOperationException($"The given key {key}, is not present in headers collection.");
            }
            return this.headers[key];
        }

        public override string ToString() => string.Join(Environment.NewLine, this.headers.Values);
    }
}
