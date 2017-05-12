var task = CurrentTask as NUnitTestsTask;

var solutionDir = BuildScriptPath.GetRelativePath(SolutionFilePath.GetDirectory());
var solutionName = SolutionFilePath.GetFilenameWithoutExtension();

$@"
Task(""{task.Name}"")
    .Description(""Runs nunit tests for {solutionName}"")
    .IsDependentOn(""BuildBackend"")
    .Does(() =>
{{
    NUnit3($""{solutionDir.FullPath}/**/bin/{{configuration}}/*.Tests.dll"", new NUnit3Settings {{
        NoResults = true
        }});
}});
"