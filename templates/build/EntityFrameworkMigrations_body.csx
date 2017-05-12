var task = CurrentTask as EntityFrameworkMigrationsTask;

var projectPath = BuildScriptPath.GetRelativePath(task.SourceFile).FullPath;

$@"
Task(""{task.Name}"")
    .Description(""Builds migration package {task.ProjectName}"")
    .Does(() =>
    {{
        var projectPath = File(""{projectPath}"");
        var projectDir = projectPath.Path.GetDirectory();
        var outDir = outputDir + Directory(""Migrations.{task.ProjectName}"");

        MSBuild(projectPath, settings =>
            settings.WithProperty(""OutDir"", outDir)
                    .SetConfiguration(configuration)
                    .SetNodeReuse(false)
        );

        CopyFileToDirectory(projectDir + ""{task.EntityFrameworkDllFilePath}"", outputDir);
        CopyFileToDirectory(projectDir + ""{task.EntityFrameworkMigrateFilePath}"", outputDir);
    }});
"