using Cake.CD.Command;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.PlatformAbstractions;
using System.Reflection;

namespace Cake.CD.CommandLine
{
    public class CommandLineParser
    {
        private CommandRunner commandRunner;

        private CommandLineApplication cmd;

        public CommandLineParser(CommandRunner commandRunner)
        {
            this.commandRunner = commandRunner;
            PrepareCommandLineApplication();
        }

        public int Parse(string[] args)
        {
            return cmd.Execute(args);
        }

        private void PrepareCommandLineApplication()
        {
            cmd = new CommandLineApplication()
            {
                Name = IsRunningInCoreCli() ? "cake" : "dotnet-cake",
                FullName = "Cake.CD"
            };
            cmd.HelpOption("-?|-h|--help");        
            cmd.VersionOption("--version", () => GetVersion());
            cmd.OnExecute(() =>
            {
                cmd.ShowHelp();
                return 2;
            });
            PrepareInitCommand();
            // PrepareAddCommands();
        }

        private bool IsRunningInCoreCli() {
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

        private void PrepareInitCommand()
        {
            var scaffoldCommand = cmd.Command("init", config =>
            {
                config.FullName = "Cake.CD init build and deploy";
                config.Description = "Initialize build and deploy template files in current directory";
                var argSlnFilePath = config.Argument("[slnFile]", "Path to sln file, if empty sln will not be modified");
                config.OnExecute(() =>
                {
                    commandRunner.InitCommand.Run(argSlnFilePath.Value);
                    return 0;
                });
                config.HelpOption("-?|-h|--help");
            });
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
                    this.commandRunner.GenerateBuildScriptsCommand.Generate();
                    return 0;
                });
            });
            buildCommand.Command("deploy", config =>
            {
                config.Description = "adds deploy template";
                config.OnExecute(() =>
                {
                    this.commandRunner.GenerateDeployScriptsCommand.Generate();
                    return 0;
                });

            });
        }
    }
}