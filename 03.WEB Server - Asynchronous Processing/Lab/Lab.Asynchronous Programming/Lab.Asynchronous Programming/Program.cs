using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab.Asynchronous_Programming
{
    public class Program
    {
        public static void Main()
        {
            var input = Console.ReadLine()
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();

            Thread evens = new Thread((() => PrintEvenNumbers(input[0], input[1])));
            evens.Start();
            evens.Join();

            Console.WriteLine("Thread finished work.");
        }

        public static void PrintEvenNumbers(int min, int max)
        {
            for (int i = min; i < max; i++)
            {
                if (i % 2 == 0)
                {
                    Console.WriteLine(i);
                }
            }

        }
    }
}
