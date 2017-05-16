var task = CurrentTask as XUnitTestsTask;

var solutionDir = BuildScriptPath.GetRelativePath(SolutionDir);

$@"
Task(""{task.Name}"")
    .Description(""Runs xunit tests for {task.SolutionName}"")
    .Does(() =>
    {{
        var solutionDir = ""{solutionDir.FullPath}"";
        XUnit2(solutionDir + ""/**/bin/"" + configuration + ""/*.Test*.dll"", new XUnit2Settings {{
        
        }});
    }});
"