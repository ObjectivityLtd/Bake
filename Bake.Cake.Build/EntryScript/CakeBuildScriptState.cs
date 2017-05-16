using Cake.Core.IO;
using System;
using System.Collections.Generic;
using Bake.API.EntryScript;
using Bake.API.Task;

namespace Bake.Cake.Build.EntryScript
{
    public class CakeBuildScriptState : BuildScriptState
    {
        private readonly ISet<string> cakeAddins = new HashSet<string>();

        private readonly ISet<string> cakeNugetTools = new HashSet<string>();

        private readonly CakeBuildTasksProvider cakeBuildTasksProvider;

        public CakeBuildScriptState(IScriptTaskEvaluator scriptTaskEvaluator, FilePath solutionFilePath, DirectoryPath buildScriptPath, DirectoryPath outputPath)
            : base(scriptTaskEvaluator, solutionFilePath, buildScriptPath, outputPath)
        {
            this.cakeBuildTasksProvider = new CakeBuildTasksProvider();
        }

        public string AddCakeAddin(string addinName)
        {
            if (cakeAddins.Contains(addinName))
            {
                return "";
            }
            cakeAddins.Add(addinName);
            return $"#addin \"{addinName}\"{Environment.NewLine}";
        }

        public string AddCakeNugetTool(string packageName)
        {
            if (cakeNugetTools.Contains(packageName))
            {
                return "";
            }
            cakeAddins.Add(packageName);
            return $"#tool \"nuget:?package={packageName}\"{Environment.NewLine}";
        }

        public string AddAgregateTasks(string aggregateTaskName, TaskType.Group taskTypeGroup)
        {
            return cakeBuildTasksProvider.AddAgregateTasks(ScriptTasks, taskTypeGroup, aggregateTaskName);
        }

        public string AddDefaultTask(params string[] dependentTasks)
        {
            return cakeBuildTasksProvider.AddDefaultTask(dependentTasks);
        }

    }
}
