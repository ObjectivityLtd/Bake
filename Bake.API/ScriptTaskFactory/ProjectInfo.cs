using System.Linq;
using Bake.API.MsBuild;
using Cake.Common.Solution;
using Cake.Common.Solution.Project;
using Cake.Core.IO;

namespace Bake.API.ScriptTaskFactory
{
    public class ProjectInfo
    {
        public SolutionInfo SolutionInfo { get; }

        public DirectoryPath ProjectDirectoryPath
        {
            get
            {
                if (System.IO.File.Exists(Path.FullPath))
                {
                    return Path.GetDirectory();
                }
                if (System.IO.Directory.Exists(Path.FullPath))
                {
                    return new DirectoryPath(Path.FullPath);
                }
                return null;
            }
        }

        public FilePath Path { get;  }

        public string Name => SolutionProject != null ? SolutionProject.Name : Path.GetFilenameWithoutExtension().FullPath;

        public SolutionProject SolutionProject { get; }

        public ProjectParserResult ParserResult { get; }

        public ProjectInfo(SolutionInfo solutionInfo, FilePath path, SolutionProject solutionProject, ProjectParserResult parserResult)
        {
            this.SolutionInfo = solutionInfo;
            this.Path = path;
            this.SolutionProject = solutionProject;
            this.ParserResult = parserResult;
        }

        public bool IsExecutableApplication()
        {
            return ParserResult != null && ParserResult.IsExecutableApplication();
        }

        public bool IsWebApplication()
        {
            return ParserResult != null && ParserResult.IsWebApplication(Path.FullPath);
        }

        public bool IsWebsite()
        {
            return SolutionProject != null && MsBuildGuids.IsWebSite(SolutionProject.Type);
        }

        public bool IsLibraryProject()
        {
            return ParserResult != null
                    && !ParserResult.IsWebApplication(Path.FullPath)
                    && !ParserResult.IsExecutableApplication();
        }

        public bool IsUnitTestProject()
        {
            return ParserResult != null
                && IsLibraryProject()
                   && (ParserResult.IsTestProjectType(Path.FullPath)
                    || ParserResult.IsMsTestProject(Path.FullPath)
                    || FindReference("nunit") != null 
                    || FindReference("xunit") != null);
        }

        public bool IsWebDriverProject()
        {
            return FindReference("WebDriver") != null;
        }

        public ProjectAssemblyReference FindReference(string namePrefix)
        {
            return ParserResult?.References.FirstOrDefault(r => r.Include != null && r.Include.StartsWith(namePrefix));
        }

    }
}
