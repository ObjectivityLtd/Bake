using System;

namespace Cake.CD.Command
{
    public class InitCommand
    {
        public CommandRunner commandRunner { get; set; }

        public InitCommand()
        {
        }

        public void Run(string slnFilePath)
        {
            var buildScriptPaths = commandRunner.GenerateBuildScriptsCommand.Generate();
            var deployScriptPaths = commandRunner.GenerateDeployScriptsCommand.Generate();
            if (!String.IsNullOrWhiteSpace(slnFilePath))
            {
                commandRunner.UpdateVisualStudioSlnCommand.AddSolutionFolderToSlnFile(slnFilePath, "Build", "Build", buildScriptPaths);
                commandRunner.UpdateVisualStudioSlnCommand.AddSolutionFolderToSlnFile(slnFilePath, "Deploy", "Deploy", deployScriptPaths);
            }

        }
    }
}
