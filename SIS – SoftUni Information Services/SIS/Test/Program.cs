using System;
using SIS.Http.Enums;
using SIS.Http.Exceptions;
using SIS.Http.Extensions;
using SIS.Http.Requests;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var httpRequestString = @"POST /google.com/search?source=hp&ei=2K2uW5W1JYacsgHj27TwDA&q=softuni&oq=softuni&gs_l=psy-ab.3..35i39k1l2j0i131k1j0l5j0i10k1j0.546.2977.0.3209.11.8.1.0.0.0.174.753.0j6.7.0....0...1c.1.64.psy-ab..3.8.858.6..0i67k1.101.qTRvFqMCXwM HTTP/1.1
User-Agent: Mozilla/4.0
Host: www.tutorialspoint.com
Content-Type: application/x-www-form-urlencoded
Content-Length: length
Accept-Language: en-us
Accept-Encoding: gzip
Connection: Keep-Alive

licenseID=string&content=string&/paramsXML=string";


            var httpRequest = new HttpRequest(httpRequestString);

            foreach (var formData in httpRequest.FormData)
            {
                Console.WriteLine($"{formData.Key} {formData.Value}");
            }
        }
    }
}
