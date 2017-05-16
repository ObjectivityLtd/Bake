#addin "Cake.Powershell"

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument<string>("target", "Default");
var configuration = Argument<string>("configuration", "Release");
var packageVersion = Argument<string>("packageVersion", "0.0.1");

///////////////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
///////////////////////////////////////////////////////////////////////////////

var sourceDir = Directory("..");
var outputDir = Directory("../bin");

///////////////////////////////////////////////////////////////////////////////
// TASK DEFINITIONS
///////////////////////////////////////////////////////////////////////////////

Task("Clean")
    .Description("Cleans Output folder")
    .Does(() =>
{
    CleanDirectories(outputDir);
});

Task("Build")
    .Description("Builds Bake")
    .Does(() =>
{
	var projectPath = sourceDir + File("Bake.sln");

    NuGetRestore(projectPath);
    
	MSBuild(projectPath, settings => 
		settings
			.SetConfiguration(configuration)
			.SetVerbosity(Verbosity.Minimal)
			.WithProperty("Version", packageVersion)
    );
});

Task("Package")
    .Description("Creates chocolatey package")
    .IsDependentOn("Build")
    .Does(() =>
{
	var chocolateyPackSettings = new ChocolateyPackSettings {
		Version = packageVersion
	};
	ChocolateyPack("Bake.nuspec", chocolateyPackSettings);
});

///////////////////////////////////////////////////////////////////////////////
// TARGETS
///////////////////////////////////////////////////////////////////////////////

Task("Default")
    .Description("This is the default task which will be ran if no specific target is passed in.")
    .IsDependentOn("Clean")
    .IsDependentOn("Package");
    

///////////////////////////////////////////////////////////////////////////////
// EXECUTION
///////////////////////////////////////////////////////////////////////////////

RunTarget(target);