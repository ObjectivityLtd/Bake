using Bake.API.EntryScript;
using Bake.API.MsBuild;
using Bake.API.ScriptTaskFactory;
using Cake.Common.Solution;
using System.Collections.Generic;

namespace Bake.Templating.Plan
{
    public class SolutionInfoProvider
    {
        private readonly SolutionParser solutionParser;

        private readonly ProjectInfoProvider projectScriptTasksFactory;

        public SolutionInfoProvider(SolutionParser solutionParser, ProjectInfoProvider projectScriptTasksFactory)
        {
            this.solutionParser = solutionParser;
            this.projectScriptTasksFactory = projectScriptTasksFactory;
        }

        public SolutionInfo ParseSolution(InitOptions initOptions)
        {
            var solutionParserResult = solutionParser.Parse(initOptions.SolutionFilePath);
            List<ProjectInfo> projects = new List<ProjectInfo>();
            var solutionInfo = new SolutionInfo(initOptions.SolutionFilePath, initOptions.BuildSolution, projects);
            foreach (var project in solutionParserResult.Projects)
            {
                if (project.Type == null || !MsBuildGuids.IsSupportedSlnTypeIdentifier(project.Type))
                {
                    continue;
                }
                var projectInfo = projectScriptTasksFactory.ParseProject(solutionInfo, project);
                projects.Add(projectInfo);
            }
            return solutionInfo;
        }
    }
}
