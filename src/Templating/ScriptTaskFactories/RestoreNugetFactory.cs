using System.Collections.Generic;
using Cake.CD.Templating.Steps.Build;

namespace Cake.CD.Templating.ScriptTaskFactories
{
    public class RestoreNugetFactory : AbstractScriptTaskFactory
    {
        public override int ParsingOrder => 1;

        public override bool IsSolutionLevel => true;

        public override bool IsApplicable(ProjectInfo projectInfo)
        {
            return projectInfo.SolutionFilePath != null;
        }

        public override IEnumerable<IScriptTask> Create(ProjectInfo projectInfo)
        {
            return new List<IScriptTask>
            {
                new RestoreNugetTask(projectInfo.SolutionFilePath.GetFilenameWithoutExtension().FullPath)
            };
        }
    }
}
