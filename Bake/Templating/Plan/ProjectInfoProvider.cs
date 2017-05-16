using System.Collections.Generic;
using Bake.API.ScriptTaskFactory;
using Cake.Common.Solution;
using Cake.Common.Solution.Project;
using System.IO;
using Bake.API.Logging;
using Bake.API.MsBuild;
using Cake.Core.IO;

namespace Bake.Templating.Plan
{
    public class ProjectInfoProvider
    {
        private readonly SolutionParser solutionParser;

        private readonly ProjectParser projectParser;

        private readonly DirectoryExplorer directoryExplorer;

        public ProjectInfoProvider(SolutionParser solutionParser, ProjectParser projectParser, DirectoryExplorer directoryExplorer)
        {
            this.solutionParser = solutionParser;
            this.projectParser = projectParser;
            this.directoryExplorer = directoryExplorer;
        }

        public IEnumerable<ProjectInfo> ParseSolutionFile(SolutionInfo solutionInfo)
        {
            var solutionParserResult = solutionParser.Parse((FilePath)solutionInfo.SolutionPath);
            foreach (var project in solutionParserResult.Projects)
            {
                ProjectParserResult parserResult = null;
                if (File.Exists(project.Path.FullPath))
                {
                    parserResult = projectParser.Parse(project.Path);
                }
                yield return new ProjectInfo(solutionInfo, project.Path, project, parserResult);
            }
        }

        public IEnumerable<ProjectInfo> ExploreDirectory(SolutionInfo solutionInfo)
        {
            var parsableItems = directoryExplorer.Explore(solutionInfo.SolutionPath.FullPath);
            foreach (var parsableItem in parsableItems)
            {
                ProjectParserResult parserResult = null;
                if (parsableItem.Type == ParsableItem.ItemType.MsBuildProject)
                {
                    parserResult = projectParser.Parse(parsableItem.Path);
                }
                yield return new ProjectInfo(solutionInfo, parsableItem.Path, null, parserResult);
            }
        }

   }
}
