var task = CurrentTask as WebDriverTestsTask;

var projectPath = BuildScriptPath.GetRelativePath(task.SourceFile).FullPath;

$@"
Task(""{task.Name}"")
    .Description(""Builds webdriver package {task.ProjectName}"")
    .Does(() =>
    {{
        var projectPath = ""{projectPath}"";
        var dstDir = outputDir + ""{task.ProjectName}"";

        NuGetRestore(projectPath);

        MSBuild(projectPath, settings =>
            settings.WithProperty(""OutDir"", dstDir)
                .WithTarget(""Build"")
                .SetConfiguration(configuration)
                .SetNodeReuse(false));
    }});
"