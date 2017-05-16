using System.Collections.Generic;
using System.Linq;

namespace Bake.API.Task
{
    public class TaskType
    {
        public enum Group
        {
            Entry,
            Build,
            UnitTest
        }

        private static readonly List<TaskType> AllTaskTypes = new List<TaskType>();

        public static readonly TaskType Entry = new TaskType(Group.Entry, "", 0);
        public static readonly TaskType BuildBackend = new TaskType(Group.Build, "BuildBackend", 1);
        public static readonly TaskType BuildIntegrationTests = new TaskType(Group.Build, "BuildIntegrationTests", 2);
        public static readonly TaskType BuildUnitTests = new TaskType(Group.Build, "BuildUnitTests", 3);
        public static readonly TaskType BuildFrontend = new TaskType(Group.Build, "BuildFrontend", 4);
        public static readonly TaskType UnitTestBackend = new TaskType(Group.UnitTest, "RunUnitTestsBackend", 5);
        public static readonly TaskType UnitTestFrontend = new TaskType(Group.UnitTest, "RunUnitTestsFrontend", 6);

        public Group TaskGroup { get; }

        public string TaskName { get; }

        public int TaskOrder { get; }

        private TaskType(Group taskGroup, string taskName, int taskOrder)
        {
            this.TaskName = taskName;
            this.TaskOrder = taskOrder;
            this.TaskGroup = taskGroup;
            AllTaskTypes.Add(this);
        }

        public static IEnumerable<TaskType> GetTaskTypesForGroup(Group group)
        {
            return AllTaskTypes.Where(TaskType => TaskType.TaskGroup == group && !string.IsNullOrEmpty(TaskType.TaskName));
        }
    }
}
