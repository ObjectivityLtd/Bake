using System.Collections.Generic;
using System.Linq;

namespace Cake.CD.Templating
{
    public class ScriptTaskType
    {
        public enum Group
        {
            ENTRY,
            BUILD,
            UNIT_TEST
        }

        private static readonly List<ScriptTaskType> AllScriptTaskTypes = new List<ScriptTaskType>();

        public static ScriptTaskType ENTRY = new ScriptTaskType(Group.ENTRY, "<None>", 0);
        public static ScriptTaskType BUILD_BACKEND = new ScriptTaskType(Group.BUILD, "BuildBackend", 1);
        public static ScriptTaskType BUILD_FRONTEND = new ScriptTaskType(Group.BUILD, "BuildFrontend", 2);
        public static ScriptTaskType BUILD_INTEGRATION_TESTS = new ScriptTaskType(Group.BUILD, "BuildIntegrationTests", 3);
        public static ScriptTaskType UNIT_TEST_BACKEND = new ScriptTaskType(Group.UNIT_TEST, "RunUnitTestsBackend", 4);
        public static ScriptTaskType UNIT_TEST_FRONTEND = new ScriptTaskType(Group.UNIT_TEST, "RunUnitTestsFrontend", 5);

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
            return AllScriptTaskTypes.Where(scriptTaskType => scriptTaskType.TaskGroup == group);
        }
    }
}
