using Cake.CD.Command;
using Cake.CD.Templating;

namespace Cake.CD.CommandLine
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
            var commandRunner = new CommandRunner(templateFileProvider);
            return new CommandLineParser(commandRunner);
        }
    }
}

