using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MishMashWebApp.Data;
using SIS.MvcFramework;

namespace MishMashWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebHost.Start(new Startup());
        }
    }
}
