using System;
using System.Net;


namespace _01.URL_Decode
{
    public class Program
    {
        public static void Main()
        {
            var urlInput = Console.ReadLine();
            Console.WriteLine(WebUtility.UrlDecode(urlInput));
        }
    }
}
