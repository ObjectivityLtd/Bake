using Cake.CD.Scripting;
using System.Collections.Generic;

namespace Cake.CD.Templating.Build
{
    public class BuildScriptState : IScriptState
    {
        public List<IScriptTask> TaskScripts { get; private set; }

        // TODO: just for test, remove me
        public string BuildDir { get; private set; }

        public BuildScriptState()
        {
            this.TaskScripts = new List<IScriptTask>();
            this.BuildDir = "testBuildDir";
        }

        public void AddTaskScript(IScriptTask taskScript)
        {
            this.TaskScripts.Add(taskScript);
        }
    }
}
