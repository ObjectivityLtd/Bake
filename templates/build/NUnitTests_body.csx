var task = CurrentTask as NUnitTestsTask;

var solutionDir = BuildScriptPath.GetRelativePath(SolutionFilePath.GetDirectory()).FullPath;

$@"Task(""{task.Name}"")
    .IsDependentOn(""Build"")
    .Does(() =>
{{
    NUnit3($""{solutionDir}/**/bin/{{configuration}}/*.Tests.dll"", new NUnit3Settings {{
        NoResults = true
        }});
}});
"