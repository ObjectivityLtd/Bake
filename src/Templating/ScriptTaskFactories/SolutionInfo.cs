using System.Collections.Generic;
using Cake.Core.IO;

namespace Cake.CD.Templating.ScriptTaskFactories
{
    public class SolutionInfo
    {
        public FilePath SolutionFilePath { get; }

        public IEnumerable<ProjectInfo> Projects { get; }

        public SolutionInfo(FilePath solutionFilePath, IEnumerable<ProjectInfo> projects)
        {
            this.SolutionFilePath = solutionFilePath;
            this.Projects = projects;
        }

    }
}
