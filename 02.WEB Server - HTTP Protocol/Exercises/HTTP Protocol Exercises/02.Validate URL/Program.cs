using System;
using System.Net;
using System.Runtime.InteropServices.ComTypes;

namespace _02.Validate_URL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var urlInput = Console.ReadLine();

            if (!CheckURLValid(urlInput))
            {
                Console.WriteLine("Invalid URL");
                return;
            }

            Uri uri = new Uri(WebUtility.UrlDecode(urlInput));

            string requested = uri.Scheme + Uri.SchemeDelimiter + uri.Host + ":" + uri.Port;

            var protocol = uri.Scheme;
            var host = uri.Host;
            var port = uri.Port;
            var path = uri.AbsolutePath;
            var query = uri.Query.TrimStart('?');
            var fragment = uri.Fragment.TrimStart('#');

            Console.WriteLine($"Protocol: {protocol}");
            Console.WriteLine($"Host: {host}");
            Console.WriteLine($"Port: {port}");
            Console.WriteLine($"Path: {path}");
            if (query != "")
            {
                Console.WriteLine($"Query: {query}"); 
            }

            if (fragment != "")
            {
                Console.WriteLine($"Fragment: {fragment}");
            }
            

            
            

        }
        public static bool CheckURLValid(string strURL)
        {
            Uri uri;
            try
            {
                uri = new Uri(strURL);
            }
            catch (Exception ex)
            {
                return false;
            }
            if (uri.Scheme == "http" && uri.Port != 80)
            {
                return false;
            }
            else if (uri.Scheme == "https" && uri.Port != 443)
            {
                return false;
            }

            if (string.IsNullOrEmpty(uri.Scheme) || string.IsNullOrEmpty(uri.Host) || string.IsNullOrEmpty(uri.AbsolutePath) || uri.Port == -1)
            {
                return false;
            }

            //if (uri.Scheme == "" || uri.Host == "" || uri.Port == -1 || uri.AbsolutePath == "")
            //{
            //    return false;
            //}
            return Uri.TryCreate(strURL, UriKind.RelativeOrAbsolute, out uri);
        }
    }
}
