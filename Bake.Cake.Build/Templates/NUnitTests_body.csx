var task = CurrentTask as NUnitTestsTask;

var solutionDir = BuildScriptPath.GetRelativePath(SolutionDir);

$@"
Task(""{task.Name}"")
    .Description(""Runs nunit tests for {task.SolutionName}"")
    .Does(() =>
    {{
        var solutionDir = ""{solutionDir.FullPath}"";
        NUnit3(solutionDir + ""/**/bin/"" + configuration + ""/*.Test*.dll"", new NUnit3Settings {{
            NoResults = true
        }});
    }});
"