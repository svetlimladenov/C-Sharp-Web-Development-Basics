using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // race condition
            //for (int k = 0; k < 3; k++)
            //{
            //    var tasks = new List<Task>();

            //    int num = 0;
            //    var lockObj = new object();
            //    try
            //    {
            //        for (int i = 0; i < 8; i++)
            //        {
            //            var task = new Task(() =>
            //            {
            //                for (int j = 0; j < 100000; j++)
            //                {
            //                    if (j == 1000)
            //                    {
            //                        // throw new Exception();
            //                    }
            //                    lock (lockObj)
            //                    {
            //                        num++;
            //                    }                  
            //                }

            //                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} has finished.");
            //            });
            //            tasks.Add(task);
            //            task.Start();
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine("!!!!!" + e);
            //    }

            //    foreach (var task in tasks)
            //    {
            //        task.Wait();
            //    }

            //    Console.WriteLine(num);
            //    Console.WriteLine(new string('=',60));
            //}





            ////Thread thread = new Thread((() =>
            ////{
            ////    var result = NumberOfPrimesInInterval(2, 100000);
            ////    Console.WriteLine(result + "comes from thread #" + Thread.CurrentThread.ManagedThreadId + Environment.NewLine
            ////                      + new System.Diagnostics.StackTrace().ToString());
            ////}));

            ////thread.Start();


            ////var task = Task.Run((() => NumberOfPrimesInInterval(2, 100000)));
            ////task.ContinueWith((taskResult) =>
            ////    Console.WriteLine(taskResult.Result
            ////                      + "comes from thread #"
            ////                      + Thread.CurrentThread.ManagedThreadId
            ////                      + Environment.NewLine
            ////                      + new System.Diagnostics.StackTrace().ToString()));


            ////Console.WriteLine(new System.Diagnostics.StackTrace().ToString());
            ////Console.WriteLine(NumberOfPrimesInInterval(2, 3));

            //PrintCount();

            var sw = new Stopwatch();
            sw.Start();
            var result = NumberOfPrimesInInterval(2, 100000);
            Console.WriteLine(result);
            Console.WriteLine(sw.Elapsed);
            while (true)
            {
                var line = Console.ReadLine();
                if (line == "exit")
                {
                    //// Environment.Exit(0);
                    return;
                }
                else
                {
                    Console.WriteLine(line);
                }
            }
        }

        public static async void PrintCount()
        {
            var result = await NumberOfPrimesInIntervalAsync(2, 10000);
            Console.WriteLine(result);
            Console.WriteLine(DateTime.Now);
            Thread.Sleep(10000);
            Console.WriteLine(DateTime.Now);
        }

        public static Task<int> NumberOfPrimesInIntervalAsync(int min, int max)
        {
            return Task.Run(() => NumberOfPrimesInInterval(min, max));
        }

        public static int NumberOfPrimesInInterval(int min, int max)
        {
            var count = 0;
            Parallel.For(min,max + 1, i =>
            {
                {
                    bool isPrime = true;
                    for (int j = 2; j < i; j++)
                    {
                        if (i % j == 0)
                        {
                            isPrime = false;
                            break;
                        }
                    }

                    if (isPrime)
                    {
                        Interlocked.Increment(ref count);
                    }
                }
            });


            return count;
        }
    }
}
