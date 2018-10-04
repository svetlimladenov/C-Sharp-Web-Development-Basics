namespace SIS.Http.Exceptions
{
    using System;
    using Enums;
    public class BadRequestException : Exception
    {
        private const string defaultMessage = "The Request was malformed or contains unsupported elements.";

        public BadRequestException()
            : this(defaultMessage)
        {
        }

        public BadRequestException(string message)
            : base(message)
        {
        }
    }
}
