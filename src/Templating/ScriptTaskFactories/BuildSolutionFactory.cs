using System.Collections.Generic;
using Cake.CD.Templating.Steps.Build;

namespace Cake.CD.Templating.ScriptTaskFactories
{
    public class BuildSolutionFactory : AbstractScriptTaskFactory
    {
        public override int ParsingOrder => 1;

        public override bool IsSolutionLevel => true;

        public override bool IsApplicable(ProjectInfo projectInfo)
        {
            return projectInfo.SolutionFilePath != null;
        }

        public override IEnumerable<IScriptTask> Create(ProjectInfo projectInfo)
        {
            var solutionName = projectInfo.SolutionFilePath.GetFilenameWithoutExtension().FullPath;
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
