using Bake.API.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bake.Cake.Build.EntryScript
{
    public class CakeBuildTasksProvider
    {
        public string AddAgregateTasks(IEnumerable<ITask> scriptTasks, TaskType.Group taskTypeGroup, string aggregateTaskName)
        {
            if (string.IsNullOrEmpty(aggregateTaskName))
            {
                return "";
            }
            var sb = new StringBuilder();
            var addedTasks = AppendBuildTasks(sb, scriptTasks, taskTypeGroup);
            AppendAggregateTask(sb, aggregateTaskName, addedTasks); 
            return sb.ToString();
        }

        public string AddDefaultTask(params string[] dependentTasks)
        {
            var sb = new StringBuilder();
            var dependentOnList = GetDependentOnList(dependentTasks);
            AppendTask(sb, "Default", dependentOnList);
            return sb.ToString();
        }

        private List<TaskType> AppendBuildTasks(StringBuilder sb, IEnumerable<ITask> scriptTasks, TaskType.Group taskGroup)
        {
            var tasksAdded = new List<TaskType>();
            var TaskTypes = TaskType.GetTaskTypesForGroup(taskGroup);
            foreach (var taskType in TaskTypes)
            {
                var dependencies = GetDependenciesByType(scriptTasks, taskType);
                if (dependencies.Any())
                {
                    var dependentOnList = GetDependentOnList(dependencies);
                    AppendTask(sb, taskType.TaskName, dependentOnList);
                    tasksAdded.Add(taskType);
                }
            }
            return tasksAdded;
        }

        private void AppendAggregateTask(StringBuilder sb, string taskName, IEnumerable<TaskType> addedTasks)
        {
            var addedTaskNames = addedTasks.Select(task => task.TaskName);
            var dependentOnList = GetDependentOnList(addedTaskNames);
            AppendTask(sb, taskName, dependentOnList);
        }

        private void AppendTask(StringBuilder sb, string taskName, string dependentOnList)
        {
            sb.AppendLine($"Task(\"{taskName}\")");
            sb.Append(dependentOnList);
            sb.Append(Environment.NewLine);
        }

        private IEnumerable<string> GetDependenciesByType(IEnumerable<ITask> scriptTasks, TaskType TaskType)
        {
            return scriptTasks.Where(task => task.Type == TaskType).Select(task => task.Name);
        }

        private string GetDependentOnList(IEnumerable<string> dependentTasks)
        {
            var sb = new StringBuilder();
            int i = 0;
            var dependenciesCount = dependentTasks.Count();
            foreach (var dependency in dependentTasks)
            {
                sb.Append($"    .IsDependentOn(\"{dependency}\")");
                if (++i == dependenciesCount)
                {
                    sb.Append(";");
                }
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }
    }
}
