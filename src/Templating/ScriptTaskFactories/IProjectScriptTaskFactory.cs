using System.Collections.Generic;

namespace Cake.CD.Templating.ScriptTaskFactories
{
    public interface IProjectScriptTaskFactory
    {
        int ParsingOrder { get; }

        IEnumerable<IScriptTask> Create(ProjectInfo projectInfo);

        bool IsApplicable(ProjectInfo projectInfo);
    }
}