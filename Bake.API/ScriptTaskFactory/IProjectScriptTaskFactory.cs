using System.Collections.Generic;
using Bake.API.Task;

namespace Bake.API.ScriptTaskFactory
{
    public interface IProjectScriptTaskFactory
    {
        int Order { get; }

        IEnumerable<ITask> Create(ProjectInfo projectInfo);

        bool IsApplicable(ProjectInfo projectInfo);
    }
}