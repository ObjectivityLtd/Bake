using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.CD.MsBuild;
using Cake.CD.Templating.Steps.Build;
using Cake.Common.Solution;
using Cake.Common.Solution.Project;
using Serilog;
using Cake.Core.IO;

namespace Cake.CD.Templating.ScriptTaskFactories
{
    public class EntityFrameworkMigrationsFactory : IScriptTaskFactory
    {
        public bool IsApplicable(SolutionProject solutionProject, ProjectParserResult parserResult)
        {
            if (!IsCSharpLibraryProject(solutionProject, parserResult))
            {
                return false;
            }
            var entityFrameworkReference = FindEntityFrameworkReference(parserResult);
            if (entityFrameworkReference == null)
            {
                return false;
            }
            if (!ContainsMigrations(parserResult))
            {
                Log.Information("Project {ProjectFile} has reference to Entity Framework but it has no reference to {Dir} directory - skipping.",
                    solutionProject.Path.GetFilename(), "migrations");
                return false;
            }
            return true;

        }

        public IEnumerable<IScriptTask> Create(SolutionProject solutionProject, ProjectParserResult parserResult)
        {
            Log.Information("Project {ProjectFile} is Entity Framework project with migrations - adding migrations template.", solutionProject.Path.GetFilename());
            var sourceFile = solutionProject.Path;
            var sourceDir = sourceFile.GetDirectory();
            var projectName = solutionProject.Name;
            var entityFrameworkReference = FindEntityFrameworkReference(parserResult);
            var entityFrameworkDllFilePath = GetPathToEntityFramework(sourceDir, entityFrameworkReference);
            var entityFrameworkMigrateFilePath = GetPathToMigrate(entityFrameworkDllFilePath);
            return new List<IScriptTask>
            {
                new EntityFrameworkMigrationsTask(sourceFile, projectName, entityFrameworkDllFilePath, entityFrameworkMigrateFilePath)
            };
        }

        private bool IsCSharpLibraryProject(SolutionProject solutionProject, ProjectParserResult parserResult)
        {
            if (parserResult == null || !MsBuildGuids.IsCSharp(solutionProject.Type))
            {
                return false;
            }
            var projectPath = solutionProject.Path.FullPath;
            if (parserResult.IsWebApplication(projectPath) || parserResult.IsExecutableApplication())
            {
                return false;
            }
            return true;
        }

        private ProjectAssemblyReference FindEntityFrameworkReference(ProjectParserResult parserResult)
        {
            return parserResult.References.FirstOrDefault(r => r.Include != null && r.Include.StartsWith("EntityFramework"));
        }

        private bool ContainsMigrations(ProjectParserResult parserResult)
        {
            return parserResult.Files.Any(f => f.Compile && f.RelativePath.ToLower().Contains("migrations"));
        }

        private FilePath GetPathToEntityFramework(DirectoryPath projectDir, ProjectAssemblyReference efReference)
        {
            var hintPath = efReference.HintPath;
            if (hintPath != null)
            {
                return projectDir.GetRelativePath(hintPath);
            }
            return new FilePath(@"..\packages\EntityFramework.<TODO version>\lib\net45\EntityFramework.dll");
        }

        private FilePath GetPathToMigrate(FilePath pathToEf)
        {
            var efDir = pathToEf.GetDirectory();
            var libSegment = Array.IndexOf(efDir.Segments, "lib");
            if (libSegment == -1)
            {
                return efDir.CombineWithFilePath("<TODO migrate.exe>");
            }
            var segmentsBaseDir = efDir.Segments.Take(libSegment);
            var efBaseDir = new DirectoryPath(string.Join("/", segmentsBaseDir));
            return efBaseDir.CombineWithFilePath("tools/migrate.exe");
        }
    }
}
