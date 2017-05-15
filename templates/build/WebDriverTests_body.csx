var task = CurrentTask as WebDriverTestsTask;

var projectPath = BuildScriptPath.GetRelativePath(task.SourceFile).FullPath;

var result = $@"
Task(""{task.Name}"")
    .Description(""Builds webdriver package {task.ProjectName}"")
    .Does(() =>
    {{
        var projectPath = File(""{projectPath}"");
        var outDir = outputDir + Directory(""{task.ProjectName}"");";

if (task.RestoreNuget)
{
    result += $@"
        NuGetRestore(projectPath);";
}

result += $@"
        MSBuild(projectPath, defaultMsBuildCommonSettings.WithTarget(""Build"").WithProperty(""OutDir"", outDir));
    }});
";

return result;