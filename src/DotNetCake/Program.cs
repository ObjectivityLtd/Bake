using System;

namespace DotNetCake
{
    class Program
    {
        static int Main(string[] args)
        {
            return GetCommandLineParser().Parse(args);
        }
        
        static CommandLineParser GetCommandLineParser()
        {
            var templateFileProvider = new TemplateFileProvider();
            var buildGenerator = new BuildGenerator(templateFileProvider);
            return new CommandLineParser(buildGenerator);
        }
    }
}

