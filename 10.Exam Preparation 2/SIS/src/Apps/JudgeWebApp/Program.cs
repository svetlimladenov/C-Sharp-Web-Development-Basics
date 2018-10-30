using System;
using SIS.MvcFramework;

namespace JudgeWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebHost.Start(new Startup());
        }
    }
}
