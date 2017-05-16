using Bake.API.ScriptTaskFactory;
using Bake.API.Task;
using System.Collections.Generic;

namespace Bake.Cake.Build.TaskFactory.Project
{
    public abstract class AbstractProjectScriptTaskFactory : IProjectScriptTaskFactory
    {
        public virtual int Order => 10;

        public abstract bool IsApplicable(ProjectInfo projectInfo);

        public abstract IEnumerable<ITask> Create(ProjectInfo projectInfo);
    }
}
