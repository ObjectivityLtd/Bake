using System.Collections.Generic;
using Cake.CD.Templating.Steps.Build;
using Cake.Core.IO;

namespace Cake.CD.Templating.ScriptTaskFactories.Solution
{
    public class BuildSolutionFactory : AbstractSolutionScriptTaskFactory
    {
        public override int ParsingOrder => 1;

        public override bool IsApplicable(SolutionInfo solutionInfo)
        {
            return true;
        }

        public override IEnumerable<IScriptTask> Create(SolutionInfo solutionInfo)
        {
            var solutionName = solutionInfo.SolutionFilePath.GetFilenameWithoutExtension().FullPath;
            var result = new List<IScriptTask>();
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
