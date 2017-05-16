using Bake.Command;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.PlatformAbstractions;
using System.Reflection;
using Bake.API.EntryScript;
using Cake.Core.IO;
using System.IO;

namespace Bake.CommandLine
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
                Name = "bake",
                FullName = "Bake"
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
            cmd.Command("init", config =>
            {
                config.FullName = "Bake init build and deploy";
                config.Description = "Initialize build and deploy template files in current directory";
                var argSlnFilePath = config.Argument("[slnFile]", "Path to sln file, if empty sln will not be modified");
                var overwrite = config.Option("-o|--overwrite", "Overwrite .cake files if they already exist", CommandOptionType.NoValue);
                var buildSolution = config.Option("-bs|--buildSolution", "Build solution file, if not specified every project will be built separately", CommandOptionType.NoValue);
                config.OnExecute(() =>
                {
                    var solutionFilePath = argSlnFilePath.Value == null ? null : new DirectoryPath(Directory.GetCurrentDirectory()).CombineWithFilePath(argSlnFilePath.Value);
                    var initOptions = new InitOptions()
                    {
                        SolutionFilePath = solutionFilePath,
                        Overwrite = overwrite.HasValue(),
                        BuildSolution = buildSolution.HasValue()
                    };
                    
                    commandRunner.InitCommand.Run(initOptions);
                    return 0;
                });
                config.HelpOption("-?|-h|--help");
            });
        }

        /*private void PrepareAddCommands()
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
        }*/
    }
}