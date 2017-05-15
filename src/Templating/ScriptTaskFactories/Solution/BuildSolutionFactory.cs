using System.Collections.Generic;
using Cake.CD.Templating.Steps.Build;

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
            /*return new List<IScriptTask>
            {
                new MsBuildTask(
                    taskType: MsBuildTask.MsBuildTaskType.Solution,
                    sourceFile: projectInfo.SolutionFilePath,
                    projectName: solutionName,
                    createPackage: false,
                    restoreNuget: true,
                    autoParameterizeWebConfig: false)
            };*/
            return new List<IScriptTask>
            {
                new RestoreNugetTask(solutionName)
            };
        }
    }
}
