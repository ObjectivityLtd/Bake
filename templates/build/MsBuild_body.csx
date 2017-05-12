var task = CurrentTask as MsBuildTask;

var projectPath = BuildScriptPath.GetRelativePath(task.SourceFile).FullPath;

$@"
Task(""{task.Name}"")
    .Description(""Builds {task.TaskType} package {task.ProjectName}"")
    .Does(() =>
    {{
        var projectPath = ""{projectPath}"";
        var outputZip = outputDir + ""{task.ProjectName}.zip"";

        NuGetRestore(projectPath);

        MSBuild(projectPath, settings =>
            settings.WithProperty(""DeployTarget"", ""Package"")
                    .WithProperty(""DeployOnBuild"", ""True"")
                    .WithProperty(""AutoParameterizationWebConfigConnectionStrings"", ""false"")
                    .WithProperty(""PackageLocation"", outputZip)
                    .SetConfiguration(configuration)
                    .SetNodeReuse(false));
    }});
"