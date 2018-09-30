namespace SIS.Http.Exceptions
{
    using System;

    public class InternalServerErrorException : Exception
    {
        private const string defaultMessage = "The Server has encountered an error.";

        public InternalServerErrorException()
            :base(defaultMessage)
        {
            
        }

        public InternalServerErrorException(string message)
            :base(message)
        {
        }
    }
}
