using Cake.CD.Logging;
using Cake.CD.MsBuild;
using Cake.CD.Templating.ScriptTaskFactories;
using Cake.Common.Solution;
using Cake.Core.IO;
using System.Collections.Generic;
using System.IO;
using Cake.CD.Command;

namespace Cake.CD.Templating.Plan
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
