var task = CurrentTask as MsBuildTask;

var solutionPath = CakeScriptPath.GetRelativePath(task.SourceFile).FullPath;

$@"
Task(""{task.Name}"")
    .Does(() =>
    {{
        NuGetRestore(""{solutionPath}"");
    }});
"