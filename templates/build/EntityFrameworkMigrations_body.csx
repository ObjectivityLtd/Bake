var task = CurrentTask as EntityFrameworkMigrationsTask;

var projectPath = BuildScriptPath.GetRelativePath(task.SourceFile).FullPath;

$@"
Task(""{task.Name}"")
    .Description(""Builds migration package {task.ProjectName}"")
    .Does(() =>
    {{
        var projectPath = ""{projectPath}"";
        var projectDir = projectPath.GetDirectory();
        var outputDir = outputDir + Directory(""Migrations.{task.ProjectName}"";

        MSBuild(project, settings =>
            settings.WithProperty(""OutDir"", outputDir)
                    .SetConfiguration(configuration)
                    .SetNodeReuse(false)
        );

        CopyFileToDirectory(projectDir + ""{task.EntityFrameworkDllFilePath}"", outputDir);
        CopyFileToDirectory(projectDir + ""{task.EntityFrameworkMigrateFilePath}"", outputDir);
    }});
"