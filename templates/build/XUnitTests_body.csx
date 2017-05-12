var task = CurrentTask as XUnitTestsTask;

var solutionDir = BuildScriptPath.GetRelativePath(SolutionFilePath.GetDirectory());
var solutionName = SolutionFilePath.GetFilenameWithoutExtension();

$@"
Task(""{task.Name}"")
    .Description(""Runs xunit tests for {solutionName}"")
    .IsDependentOn(""BuildBackend"")
    .Does(() =>
    {{
        XUnit2($""{solutionDir.FullPath}/**/bin/{{configuration}}/*.Tests.dll"", new XUnit2Settings {{
        
        }});
    }});
"