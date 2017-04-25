using Cake.CD.Templating;
using System.Collections.Generic;

namespace Cake.CD.Command
{
    public class GenerateBuildScriptsCommand
    {

        private TemplateFileProvider templateFileProvider;

        private CommandRunner commandRunner;

        public GenerateBuildScriptsCommand(TemplateFileProvider templateFileProvider, CommandRunner commandRunner)
        {
            this.templateFileProvider = templateFileProvider;
            this.commandRunner = commandRunner;
        }

        public void Generate()
        {
            var filePaths = new List<string>() { "build\\build.ps1", "build\\build.cake" };
            templateFileProvider.WriteTemplateFiles(filePaths);

            // TODO
            commandRunner.UpdateVisualStudioSlnCommand.AddSolutionFolderToSlnFile("WebApplication1.sln", "Build", "Build", filePaths);
        }
    }
}