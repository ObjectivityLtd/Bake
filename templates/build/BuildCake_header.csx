var task = CurrentTask as BuildCake;

string result = GenerateParts(ScriptTaskPart.Header, ScriptTaskType.Group.Build);
result += GenerateParts(ScriptTaskPart.Header, ScriptTaskType.Group.UnitTest);

result += $@"
//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument(""target"", ""Default"");
var configuration = Argument(""configuration"", ""Release"");

///////////////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
/////////////////////////////////////////////////////////////////////////////// 

var outputDir = Directory(MakeAbsolute(Directory(""{BuildScriptPath.GetRelativePath(OutputPath).FullPath}"")).FullPath);
var isRunningOnCIServer = TeamCity.IsRunningOnTeamCity || AppVeyor.IsRunningOnAppVeyor || Jenkins.IsRunningOnJenkins;
";

if (task.HasMsBuildSteps)
{
    result += $@"
var defaultMsBuildCommonSettings = new MSBuildSettings {{
        Configuration = configuration,
        Verbosity = Verbosity.Minimal,
        //ToolVersion = MSBuildToolVersion.VS2017,
        PlatformTarget = PlatformTarget.MSIL,
        NodeReuse = !isRunningOnCIServer,
        MaxCpuCount = 0
    }};

var defaultMsBuildPackageSettings = defaultMsBuildCommonSettings
        .WithProperty(""DeployTarget"", ""Package"")
        .WithProperty(""DeployOnBuild"", ""True"")
        .WithProperty(""AutoParameterizationWebConfigConnectionStrings"", ""{task.AutoParameterizeWebConfig}"");
";
}

result += $@"
///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(taskSetupContext => 
{{
    // Executed BEFORE the first task.
    Information(""Running tasks..."");
}});

Teardown(taskTeardownContext =>
{{
    // Executed AFTER the last task.
    Information(""Finished running tasks."");
}});

";

result
