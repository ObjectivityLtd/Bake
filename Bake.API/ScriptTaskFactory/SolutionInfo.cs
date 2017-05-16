using System.Collections.Generic;
using Cake.Core.IO;

namespace Bake.API.ScriptTaskFactory
{
    public class SolutionInfo
    {
        public Path SolutionPath { get; }

        public string Name => HasSlnFile
            ? ((FilePath) SolutionPath).GetFilenameWithoutExtension().FullPath
            : ((DirectoryPath) SolutionPath).GetDirectoryName();

        public bool HasSlnFile => SolutionPath is FilePath;

        public bool BuildSolution { get; }

        public List<ProjectInfo> Projects { get; }

        public SolutionInfo(Path solutionPath, bool buildSolution)
        {
            this.SolutionPath = solutionPath;
            this.BuildSolution = buildSolution;
            this.Projects = new List<ProjectInfo>();
        }

        public void AddProject(ProjectInfo project)
        {
            Projects.Add(project);
        }

    }
}
