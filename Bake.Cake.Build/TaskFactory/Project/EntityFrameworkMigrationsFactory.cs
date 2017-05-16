using Bake.API.ScriptTaskFactory;
using Bake.API.Task;
using Cake.Common.Solution.Project;
using Cake.Core.IO;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using Bake.Cake.Build.Task;

namespace Bake.Cake.Build.TaskFactory.Project
{
    public class EntityFrameworkMigrationsFactory : AbstractProjectScriptTaskFactory
    {
        public override int Order => 20;

        public override bool IsApplicable(ProjectInfo projectInfo)
        {
            if (!projectInfo.IsLibraryProject())
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
                    projectInfo.Path.GetFilename(), "migrations");
                return false;
            }
            return true;

        }

        public override IEnumerable<ITask> Create(ProjectInfo projectInfo)
        {
            var sourceFile = projectInfo.Path;
            var sourceDir = sourceFile.GetDirectory();
            var entityFrameworkReference = projectInfo.FindReference("EntityFramework");
            var entityFrameworkDllFilePath = GetPathToEntityFramework(sourceDir, entityFrameworkReference);
            var entityFrameworkMigrateFilePath = GetPathToMigrate(entityFrameworkDllFilePath);
            return new List<ITask>
            {
                new EntityFrameworkMigrationsTask(sourceFile, projectInfo.Name, entityFrameworkDllFilePath, entityFrameworkMigrateFilePath)
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
