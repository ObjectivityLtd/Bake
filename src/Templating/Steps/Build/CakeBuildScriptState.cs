using Cake.Core.IO;
using System;
using System.Collections.Generic;

namespace Cake.CD.Templating.Steps.Build
{
    public class CakeBuildScriptState : BuildScriptState
    {
        private readonly ISet<string> cakeAddins = new HashSet<string>();

        private readonly ISet<string> cakeNugetTools = new HashSet<string>();

        public CakeBuildTasksProvider CakeBuildTasksProvider { get; }

        public CakeBuildScriptState(ScriptTaskEvaluator scriptTaskEvaluator, FilePath solutionFilePath, DirectoryPath buildScriptPath, DirectoryPath outputPath)
            : base(scriptTaskEvaluator, solutionFilePath, buildScriptPath, outputPath)
        {
            this.CakeBuildTasksProvider = new CakeBuildTasksProvider();
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

    }
}
