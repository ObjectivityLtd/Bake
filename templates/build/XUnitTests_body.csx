var task = CurrentTask as XUnitTestsTask;

var solutionDir = BuildScriptPath.GetRelativePath(SolutionFilePath.GetDirectory());
var solutionName = SolutionFilePath.GetFilenameWithoutExtension();

$@"
Task(""{task.Name}"")
    .Description(""Runs xunit tests for {solutionName}"")
    .Does(() =>
    {{
        var solutionDir = ""{solutionDir.FullPath}"";
        XUnit2(solutionDir + ""/**/bin/"" + configuration + ""/*.Tests.dll"", new XUnit2Settings {{
        
        }});
    }});
"