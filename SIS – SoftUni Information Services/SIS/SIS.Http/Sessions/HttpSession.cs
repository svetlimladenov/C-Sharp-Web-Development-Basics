namespace SIS.Http.Sessions
{
    using System.Collections.Generic;
    using Contacts;

    public class HttpSession : IHttpSession
    {
        private readonly Dictionary<string, object> sessionParameters;
        public HttpSession(string id)
        {
            this.Id = id;
            this.sessionParameters = new Dictionary<string, object>();
        }

        public string Id { get; }
        public object GetParameter(string name)
        {
            return this.sessionParameters.GetValueOrDefault(name, null);
        }

        public bool ContainsParameter(string name)
        {
            return this.sessionParameters.ContainsKey(name);
        }

        public void AddParameter(string name, object parameter)
        {
            this.sessionParameters[name] = parameter;
        }

        public void ClearParameter(string name)
        {
            this.sessionParameters.Clear();
        }
    }
}
