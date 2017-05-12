using Cake.CD.MsBuild;
using Cake.CD.Templating.Steps.Build;
using Cake.Common.Solution.Project;
using Cake.Core.IO;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cake.CD.Templating.ScriptTaskFactories
{
    public class EntityFrameworkMigrationsFactory : AbstractScriptTaskFactory
    {
        public override bool IsApplicable(ProjectInfo projectInfo)
        {
            if (!projectInfo.IsCSharpLibraryProject())
            {
                return false;
            }
            var entityFrameworkReference = projectInfo.FindReference("EntityFramework");
            if (entityFrameworkReference == null)
            {
                return false;
            }
            if (!ContainsMigrations(projectInfo.ParserResult))
            {
                Log.Debug("Project {ProjectFile} has reference to Entity Framework but it has no reference to {Dir} directory - skipping.",
                    projectInfo.Project.Path.GetFilename(), "migrations");
                return false;
            }
            return true;

        }

        public override IEnumerable<IScriptTask> Create(ProjectInfo projectInfo)
        {
            var sourceFile = projectInfo.Project.Path;
            var sourceDir = sourceFile.GetDirectory();
            var projectName = projectInfo.Project.Name;
            var entityFrameworkReference = projectInfo.FindReference("EntityFramework");
            var entityFrameworkDllFilePath = GetPathToEntityFramework(sourceDir, entityFrameworkReference);
            var entityFrameworkMigrateFilePath = GetPathToMigrate(entityFrameworkDllFilePath);
            return new List<IScriptTask>
            {
                new EntityFrameworkMigrationsTask(sourceFile, projectName, entityFrameworkDllFilePath, entityFrameworkMigrateFilePath)
            };
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
