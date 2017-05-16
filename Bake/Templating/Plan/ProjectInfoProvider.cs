using Bake.API.ScriptTaskFactory;
using Cake.Common.Solution;
using Cake.Common.Solution.Project;
using System.IO;
using Bake.API.Logging;

namespace Bake.Templating.Plan
{
    public class ProjectInfoProvider
    {

        private readonly ProjectParser projectParser;

        public ProjectInfoProvider(ProjectParser projectParser)
        {
            this.projectParser = projectParser;
        }

        public ProjectInfo ParseProject(SolutionInfo solutionInfo, SolutionProject project)
        {
            Log.IncreaseIndent();
            try
            {
                ProjectParserResult projectParserResult = null;
                if (File.Exists(project.Path.FullPath))
                {
                    projectParserResult = projectParser.Parse(project.Path);
                }
                else if (!Directory.Exists(project.Path.FullPath))
                {
                    Log.Warn("Project points to a non-existing file or directory: {Path}.", project.Path.FullPath);
                }
                return new ProjectInfo(solutionInfo, project, projectParserResult);
            }
            finally
            {
                Log.DecreaseIndent();
            }
        }
    }
}
