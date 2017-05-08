using Cake.Core.IO;
using System;
using System.Linq;

namespace Cake.CD.Command
{
    public class InitCommand
    {
        public CommandRunner commandRunner { get; set; }

        public InitCommand()
        {
        }

        public void Run(FilePath slnFilePath)
        {
            var buildResult = commandRunner.GenerateBuildScriptsCommand.Generate(slnFilePath);
            //var deployScriptPaths = commandRunner.GenerateDeployScriptsCommand.Generate();
            if (slnFilePath != null)
            { 
                var relativePaths = buildResult.GetAddedFiles().Select(path => slnFilePath.GetRelativePath(path).FullPath).ToList();
                commandRunner.UpdateVisualStudioSlnCommand.AddSolutionFolderToSlnFile(slnFilePath.FullPath, "Build", "Build", relativePaths);
                //commandRunner.UpdateVisualStudioSlnCommand.AddSolutionFolderToSlnFile(slnFilePath, "Deploy", "Deploy", deployScriptPaths);
            }

        }
    }
}
