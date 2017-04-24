using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.PlatformAbstractions;
using System.Reflection;

namespace DotNetCake
{
    public class CommandLineParser
    {
        private BuildGenerator buildGenerator;

        private CommandLineApplication cmd;

        public CommandLineParser(BuildGenerator buildGenerator)
        {
            this.buildGenerator = buildGenerator;
            PrepareCommandLineApplication();
        }

        public int Parse(string[] args)
        {
            return cmd.Execute(args);
        }

        private void PrepareCommandLineApplication()
        {
            cmd = new CommandLineApplication();
            cmd.Name = isRunningInCoreCli() ? "cake" : "dotnet-cake";
            cmd.FullName = "Cake.CD";
            cmd.HelpOption("-?|-h|--help");        
            cmd.VersionOption("--version", () => GetVersion());
            cmd.OnExecute(() =>
            {
                cmd.ShowHelp();
                return 2;
            });
            
            PrepareAddCommands();
        }

        private bool isRunningInCoreCli() {
            return PlatformServices.Default.Application.RuntimeFramework.Identifier.StartsWith(".NETCore");
        }
        private string GetVersion()
        {
            return typeof(CommandLineParser)
            .GetTypeInfo()
            .Assembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            .InformationalVersion;
        }

        private void PrepareAddCommands()
        {
            var buildCommand = cmd.Command("add", config =>
            {
                config.Description = "adds build or deploy template files";
                config.OnExecute(() =>
                {
                    config.ShowHelp();
                    return 2;
                });
                config.HelpOption("-?|-h|--help");
            });
            buildCommand.Command("build", config =>
            {
                config.Description = "adds build template";
                config.OnExecute(() =>
                {
                    buildGenerator.Generate();
                    return 0;
                });

            });
        }

    }
}