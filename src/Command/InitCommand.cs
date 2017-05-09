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

        public void Run(InitOptions initOptions)
        {
            var solutionFilePath = initOptions.SolutionFilePath;
            var buildResult = commandRunner.GenerateBuildScriptsCommand.Generate(initOptions);
            //var deployScriptPaths = commandRunner.GenerateDeployScriptsCommand.Generate();
            if (solutionFilePath != null && buildResult.GetAddedFiles().Any())
            { 
                var relativePaths = buildResult.GetAddedFiles()
                    .Select(path => solutionFilePath.GetRelativePath(path).FullPath.Replace('/', '\\'))
                    .ToList();
                commandRunner.UpdateVisualStudioSlnCommand.AddSolutionFolderToSlnFile(solutionFilePath.FullPath, "Build", "Build", relativePaths);
                //commandRunner.UpdateVisualStudioSlnCommand.AddSolutionFolderToSlnFile(slnFilePath, "Deploy", "Deploy", deployScriptPaths);
            }

        }
    }
}
