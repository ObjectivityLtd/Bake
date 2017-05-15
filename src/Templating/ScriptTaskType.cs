using System.Collections.Generic;
using System.Linq;

namespace Cake.CD.Templating
{
    public class ScriptTaskType
    {
        public enum Group
        {
            Entry,
            Build,
            UnitTest
        }

        private static readonly List<ScriptTaskType> AllScriptTaskTypes = new List<ScriptTaskType>();

        public static readonly ScriptTaskType Entry = new ScriptTaskType(Group.Entry, "", 0);
        public static readonly ScriptTaskType BuildBackend = new ScriptTaskType(Group.Build, "BuildBackend", 1);
        public static readonly ScriptTaskType BuildIntegrationTests = new ScriptTaskType(Group.Build, "BuildIntegrationTests", 2);
        public static readonly ScriptTaskType BuildUnitTests = new ScriptTaskType(Group.Build, "BuildUnitTests", 3);
        public static readonly ScriptTaskType BuildFrontend = new ScriptTaskType(Group.Build, "BuildFrontend", 4);
        public static readonly ScriptTaskType UnitTestBackend = new ScriptTaskType(Group.UnitTest, "RunUnitTestsBackend", 5);
        public static readonly ScriptTaskType UnitTestFrontend = new ScriptTaskType(Group.UnitTest, "RunUnitTestsFrontend", 6);

        public Group TaskGroup { get; }

        public string TaskName { get; }

        public int TaskOrder { get; }

        private ScriptTaskType(Group taskGroup, string taskName, int taskOrder)
        {
            this.TaskName = taskName;
            this.TaskOrder = taskOrder;
            this.TaskGroup = taskGroup;
            AllScriptTaskTypes.Add(this);
        }

        public static IEnumerable<ScriptTaskType> GetScriptTaskTypesForGroup(Group group)
        {
            return AllScriptTaskTypes.Where(scriptTaskType => scriptTaskType.TaskGroup == group && !string.IsNullOrEmpty(scriptTaskType.TaskName));
        }
    }
}
