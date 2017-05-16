var task = CurrentTask as MsTestTestsTask;

var solutionDir = BuildScriptPath.GetRelativePath(SolutionDir);

$@"Task(""{task.Name}"")
    .Description(""Runs mstest tests for {task.SolutionName}"")
    .Does(() =>
    {{
        var solutionDir = ""{solutionDir.FullPath}"";
        MSTest(solutionDir + ""/**/bin/"" + configuration + ""/*.Test*.dll"", new MSTestSettings {{
        
        }});
    }});
"