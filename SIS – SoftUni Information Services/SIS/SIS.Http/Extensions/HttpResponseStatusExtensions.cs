namespace SIS.Http.Extensions
{
    using Enums;


    public static class HttpResponseStatusExtensions
    {
        public static string GetResponseLine(this HttpResponseStatusCode statusCode)
        {
            var statusCodeNumber = (int)statusCode;
            var statusCodeMessage = statusCode.ToString();

            return $"{statusCodeNumber} {statusCodeMessage}";
        }
    }
}
