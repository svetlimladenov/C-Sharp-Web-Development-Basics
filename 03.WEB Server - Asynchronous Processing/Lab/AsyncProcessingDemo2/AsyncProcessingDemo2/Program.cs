using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncProcessingDemo2
{
    using System;
    using System.IO;
    using System.Drawing;

    public class Program
    {
        //private static string result;
        public static void Main(string[] args)
        {

            var text = "hello, world";
          
            var bytes = Encoding.UTF8.GetBytes(text);

            Console.WriteLine();




            //var currentDirectory = Directory.GetCurrentDirectory();
            //var directoryInfo = new DirectoryInfo(currentDirectory + "\\Images");

            //var files = directoryInfo.GetFiles();

            //const string resultDir = "Result";

            //if (Directory.Exists(resultDir))
            //{
            //    Directory.Delete(resultDir, true);

            //}

            //var tasks = new List<Task>();

            //Directory.CreateDirectory(resultDir);
            //foreach (var file in files)
            //{
            //    var task = Task.Run((() =>
            //    {
            //        var image = Image.FromFile(file.FullName + "1");
            //        image.RotateFlip(RotateFlipType.RotateNoneFlipY);
            //        image.Save($"{resultDir}\\fliped-{file.Name}");
            //        Console.WriteLine($"{file.Name} processed...");
            //    }));

            //    tasks.Add(task);
            //}

            //try
            //{
            //    Task.WaitAll(tasks.ToArray());
            //}
            //catch (AggregateException ex)
            //{
            //    foreach (var exception in ex.InnerExceptions)
            //    {
            //        Console.WriteLine(exception.Message);
            //    }
            //}

            //Console.WriteLine("Calculating...");
            //Task.Run((() => CalculateSlowly()));
            //Console.WriteLine("Enter command:");
            //while (true)
            //{
            //    var line = Console.ReadLine();
            //    if (line == "show")
            //    {
            //        if (result == null)
            //        {
            //            Console.WriteLine("Still Calculating... Pls wait");
            //        }
            //        else
            //        {
            //            Console.WriteLine($"Result is {result}");
            //        }
            //        //Print result or print loading...
            //    }

            //    if (line == "exit")
            //    {
            //        break;
            //    }
            //}

            //    Task
            //        .Run(async () =>
            //        {
            //            await DownloadFileAsync();
            //        })
            //        .GetAwaiter()
            //        .GetResult();

            Task.Run(async () =>
            {
                await GetHeaders("https://softuni.bg");
            })
                .GetAwaiter()
                .GetResult();
        }

        public static async Task GetHeaders(string url)
        {
            using (var httpClient = new HttpClient())
            {
                var contentToSend = new StringContent("Hello!");
                var response = await httpClient.PostAsync(url, contentToSend);

                if (response.IsSuccessStatusCode)
                {

                    var headers = response.Headers;

                    foreach (var header in headers)
                    {
                        Console.WriteLine(header.Key + ":" + string.Join(", ", header.Value));
                    }

                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(content);
                }
                else
                {
                    Console.WriteLine(response.StatusCode);
                }
            }
        }


        //public static async Task DownloadFileAsync()
        //{
        //    Console.WriteLine("Downloadign....");
        //    var webClinet = new WebClient();
        //    await webClinet.DownloadFileTaskAsync("https://www.youtube.com/watch?v=fX62PJ_wv20&feature=youtu.be", "index.html");
        //    Console.WriteLine("Finished!");
        //}


        //private static void CalculateSlowly()
        //{
        //    Thread.Sleep(10000);
        //    result = "54";
        //}


        //private static async void DoWork()
        //{
        //    var tasks = new List<Task>();
        //    var results = new List<bool>();
        //    for (int i = 0; i < 10; i++)
        //    {
        //        tasks.Add(Task.Run(async () =>
        //        {
        //            var result = await SlowMethod();
        //            results.Add(result);
        //        }));

        //    }

        //    await Task.WhenAll(tasks.ToArray());
        //}

        //private static async Task<bool> SlowMethod()
        //{
        //    Thread.Sleep(1000);
        //    Console.WriteLine("Result!");

        //    return true;
        //}
    }
}
