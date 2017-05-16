using Bake.API.ScriptTaskFactory;
using Bake.API.Task;
using Cake.Core.IO;
using System.Collections.Generic;
using Bake.Cake.Build.Task;

namespace Bake.Cake.Build.TaskFactory.Solution
{
    public class BuildSolutionFactory : AbstractSolutionScriptTaskFactory
    {
        public override int Order => 1;

        public override bool IsApplicable(SolutionInfo solutionInfo)
        {
            return true;
        }

        public override IEnumerable<ITask> Create(SolutionInfo solutionInfo)
        {
            var solutionName = solutionInfo.SolutionFilePath.GetFilenameWithoutExtension().FullPath;
            var result = new List<ITask>();
            if (solutionInfo.BuildSolution)
            {
                result.Add(new MsBuildTask(
                    taskType: MsBuildTask.MsBuildTaskType.Solution,
                    sourceFiles: new List<FilePath> { solutionInfo.SolutionFilePath },
                    projectName: solutionName
                ));
            }
            else
            {
                result.Add(new RestoreNugetTask(solutionName));
            }
            return result;
        }
    }
}
