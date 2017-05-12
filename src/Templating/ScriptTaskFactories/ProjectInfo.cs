using System.Linq;
using Cake.CD.MsBuild;
using Cake.Common.Solution;
using Cake.Common.Solution.Project;
using Cake.Core.IO;

namespace Cake.CD.Templating.ScriptTaskFactories
{
    public class ProjectInfo
    {
        public FilePath SolutionFilePath { get; }

        public DirectoryPath ProjectDirectoryPath
        {
            get
            {
                if (Project == null)
                {
                    return null;
                }
                if (System.IO.File.Exists(Project.Path.FullPath))
                {
                    return Project.Path.GetDirectory();
                }
                if (System.IO.Directory.Exists(Project.Path.FullPath))
                {
                    return new DirectoryPath(Project.Path.FullPath);
                }
                return null;
            }
        }

        public SolutionProject Project { get; }

        public ProjectParserResult ParserResult { get; }

        public ProjectInfo(FilePath solutionFilePath, SolutionProject project, ProjectParserResult parserResult)
        {
            this.SolutionFilePath = solutionFilePath;
            this.Project = project;
            this.ParserResult = parserResult;
        }

        public bool IsExecutableApplication()
        {
            return ParserResult != null && ParserResult.IsExecutableApplication();
        }

        public bool IsWebApplication()
        {
            return ParserResult != null && ParserResult.IsWebApplication(Project.Path.FullPath);
        }

        public bool IsWebsite()
        {
            return MsBuildGuids.IsWebSite(Project.Type);
        }

        public bool IsCSharpLibraryProject()
        {
            return ParserResult != null
                    && MsBuildGuids.IsCSharp(Project.Type)
                    && !ParserResult.IsWebApplication(Project.Path.FullPath)
                    && !ParserResult.IsExecutableApplication();
        }

        public bool IsUnitTestProject()
        {
            return ParserResult != null
                && IsCSharpLibraryProject()
                   && (ParserResult.IsTestProjectType(Project.Path.FullPath)
                    || ParserResult.IsMsTestProject(Project.Path.FullPath)
                    || FindReference("nunit") != null 
                    || FindReference("xunit") != null);
        }

        public ProjectAssemblyReference FindReference(string namePrefix)
        {
            return ParserResult?.References.FirstOrDefault(r => r.Include != null && r.Include.StartsWith(namePrefix));
        }

    }
}
