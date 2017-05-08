var task = CurrentTask as MsBuildTask;

var solutionPath = CakeScriptPath.GetRelativePath(task.SourceFile).FullPath;

$@"

Task(""Restore Nuget - {task.Name}"")
    .IsDependentOn(""Clean"")
    .Does(() =>
    {{
        NuGetRestore(""{solutionPath}"");
    }});

Task(""Build - {task.Name}"")
    .Description(""Builds solution {task.Name}"")
    .IsDependentOn(""Clean"")
    .Does(() =>
    {{
        var solutionPath = ""{solutionPath}"";
        var outputZip = outputDir + ""{task.SolutionName}"";

        MSBuild(solutionPath, settings =>
            settings.WithProperty(""VisualStudioVersion"", ""14.0"")
                    .WithProperty(""DeployTarget"", ""Package"")
                    .WithProperty(""DeployOnBuild"", ""True"")
                    .WithProperty(""AutoParameterizationWebConfigConnectionStrings"", ""false"")
                    .WithProperty(""PackageLocation"", outputZip)
                    .SetConfiguration(configuration)
                    .SetNodeReuse(false));
    }});
"