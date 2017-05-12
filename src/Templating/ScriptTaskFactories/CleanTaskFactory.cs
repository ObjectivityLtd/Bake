using Cake.CD.Templating.Steps.Build;
using System.Collections.Generic;

namespace Cake.CD.Templating.ScriptTaskFactories
{
    public class CleanTaskFactory : AbstractScriptTaskFactory
    {
        public override int ParsingOrder => 0;

        public override bool IsSolutionLevel => true;

        public override bool IsApplicable(ProjectInfo projectInfo)
        {
            return true;
        }

        public override IEnumerable<IScriptTask> Create(ProjectInfo projectInfo)
        {
            return new List<IScriptTask>
            {
                new CleanTask()
            };
        }

        
    }
}
