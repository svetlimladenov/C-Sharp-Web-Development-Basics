using System;
using System.Collections.Generic;

namespace _03.Request_Parser
{
    public class Program
    {
        public static void Main()
        {   
            //key = Status Code value = Path
            var pathStorage = new Dictionary<string,List<string>>();
            while (true)
            {
                var input = Console.ReadLine();
                if (input.ToLower() == "end")
                {
                    break;
                }

                var inputTokens = input.Split('/');
                var inputPath = inputTokens[1];
                var inputStatusCode = inputTokens[2];
                if (!pathStorage.ContainsKey(inputStatusCode))
                {
                    pathStorage[inputStatusCode] = new List<string>();
                }
                pathStorage[inputStatusCode].Add(inputPath);
            }

            var request = Console.ReadLine();
            var requestTokens = request.Split(new[] {' ', '/'},StringSplitOptions.RemoveEmptyEntries);
            var requestStatusCode = requestTokens[0].ToLower();
            var requestPath = requestTokens[1];


            if (pathStorage.ContainsKey(requestStatusCode))
            {
                if (pathStorage[requestStatusCode].Contains(requestPath))
                {
                    var responseStatusCode = 200;
                    var responseStatusMsg = "OK";
                    PrintResponse(responseStatusCode, responseStatusMsg);
                    return;
                }
            }

            var notFoundStatusCode = 404;
            var notFoundStatusMsg = "NotFound";
            PrintResponse(notFoundStatusCode, notFoundStatusMsg);

        }

        private static void PrintResponse(int responseStatusCode, string responseStatusMsg)
        {
            Console.WriteLine($"HTTP/1.1 {responseStatusCode} {responseStatusMsg}");
            Console.WriteLine($"Content-Length: {responseStatusMsg.Length}");
            Console.WriteLine("Content-Type: text/plain");
            Console.WriteLine();
            Console.WriteLine(responseStatusMsg);
        }
    }
}
