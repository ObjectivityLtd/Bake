var task = CurrentTask as NUnitTestsTask;

var solutionDir = BuildScriptPath.GetRelativePath(SolutionFilePath.GetDirectory());
var solutionName = SolutionFilePath.GetFilenameWithoutExtension();

$@"
Task(""{task.Name}"")
    .Description(""Runs nunit tests for {solutionName}"")
    .Does(() =>
    {{
        var solutionDir = ""{solutionDir.FullPath}"";
        NUnit3(solutionDir + ""/**/bin/"" + configuration + ""/*.Test*.dll"", new NUnit3Settings {{
            NoResults = true
        }});
    }});
"