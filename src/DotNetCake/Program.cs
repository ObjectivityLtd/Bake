using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace DotNetCake
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Cake.CD");
            new CommandLineParser().Parse(args);
        }        
    }
}

