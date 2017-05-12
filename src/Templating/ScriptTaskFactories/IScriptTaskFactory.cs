using System.Collections.Generic;

namespace Cake.CD.Templating.ScriptTaskFactories
{
    public interface IScriptTaskFactory
    {
        int Order { get; }

        IEnumerable<IScriptTask> Create(ProjectInfo projectInfo);

        bool IsApplicable(ProjectInfo projectInfo);
    }
}