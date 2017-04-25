using Cake.CD.Command;
using Cake.CD.Templating;
using System;

namespace Cake.CD.CommandLine
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                return GetCommandLineParser().Parse(args);
            } catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: " + e.Message);
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Stack trace: " + e.StackTrace);
                Console.ResetColor();
                return -1;
            }
        }
        
        static CommandLineParser GetCommandLineParser()
        {
            var templateFileProvider = new TemplateFileProvider();
            var commandRunner = new CommandRunner(templateFileProvider);
            return new CommandLineParser(commandRunner);
        }
    }
}

