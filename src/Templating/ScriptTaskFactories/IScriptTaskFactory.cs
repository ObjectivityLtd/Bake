using System.Collections.Generic;

namespace Cake.CD.Templating.ScriptTaskFactories
{
    public interface IScriptTaskFactory
    {
        int ParsingOrder { get; }

        // if true, other factories will not be run if project is applicable for this factory
        bool IsTerminating { get; }

        // if true, will not be executed more than once
        bool IsSolutionLevel { get;  }

        IEnumerable<IScriptTask> Create(ProjectInfo projectInfo);

        bool IsApplicable(ProjectInfo projectInfo);
    }
}