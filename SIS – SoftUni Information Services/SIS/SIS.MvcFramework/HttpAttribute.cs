using System;
using SIS.Http.Enums;

namespace SIS.MvcFramework
{
    public abstract class HttpAttribute : Attribute
    {
        protected HttpAttribute(string path)
        {
            this.Path = path;
        }

        public string Path { get; }

        public abstract HttpRequestMethod  Method { get; }
    }
}