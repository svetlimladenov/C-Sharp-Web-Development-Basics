namespace WebServer.Server.Http
{
    using Common;

    public class HttpHeader
    {
        public HttpHeader(string key, string value)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));
            this.Key = key;
            this.Value = value;
        }
        public string Key { get; private set; }

        public string Value { get; private set; }

        public override string ToString() => $"{this.Key}: {this.Value}";

    }
}
