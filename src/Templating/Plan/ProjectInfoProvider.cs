using Cake.CD.Logging;
using Cake.CD.Templating.ScriptTaskFactories;
using Cake.Common.Solution;
using Cake.Common.Solution.Project;
using Cake.Core.IO;
using Serilog;
using System.IO;

namespace Cake.CD.Templating.Plan
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
            LogHelper.IncreaseIndent();
            try
            {
                ProjectParserResult projectParserResult = null;
                if (File.Exists(project.Path.FullPath))
                {
                    projectParserResult = projectParser.Parse(project.Path);
                }
                else if (!Directory.Exists(project.Path.FullPath))
                {
                    Log.Warning("Project points to a non-existing file or directory: {Path}.", project.Path.FullPath);
                }
                return new ProjectInfo(solutionInfo, project, projectParserResult);
            }
            finally
            {
                LogHelper.DecreaseIndent();
            }
        }
    }
}
