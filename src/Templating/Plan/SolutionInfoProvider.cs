using Cake.CD.Logging;
using Cake.CD.MsBuild;
using Cake.CD.Templating.ScriptTaskFactories;
using Cake.Common.Solution;
using Cake.Core.IO;
using System.Collections.Generic;
using System.IO;

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

        public SolutionInfo ParseSolution(FilePath slnFilePath)
        {
            var solutionParserResult = solutionParser.Parse(slnFilePath);
            List<ProjectInfo> projects = new List<ProjectInfo>();
            var solutionInfo = new SolutionInfo(slnFilePath, projects);
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
