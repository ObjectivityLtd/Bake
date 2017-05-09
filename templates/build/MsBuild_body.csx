var task = CurrentTask as MsBuildTask;

var solutionPath = BuildScriptPath.GetRelativePath(task.SourceFile).FullPath;

$@"
Task(""{task.Name}"")
    .Description(""Builds solution {task.SolutionName}"")
    .IsDependentOn(""Clean"")
    .Does(() =>
    {{
        var solutionPath = ""{solutionPath}"";
        var outputZip = outputDir + ""{task.SolutionName}.zip"";

        MSBuild(solutionPath, settings =>
            settings.WithProperty(""DeployTarget"", ""Package"")
                    .WithProperty(""DeployOnBuild"", ""True"")
                    .WithProperty(""AutoParameterizationWebConfigConnectionStrings"", ""false"")
                    .WithProperty(""PackageLocation"", outputZip)
                    .SetConfiguration(configuration)
                    .SetNodeReuse(false));
    }});
"