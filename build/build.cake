#addin "Cake.Powershell"

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument<string>("target", "Default");
var configuration = Argument<string>("configuration", "Release");
var packageVersion = Argument<string>("packageVersion", "0.0.1-dev");

///////////////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
///////////////////////////////////////////////////////////////////////////////

var sourceDir = "../src";
var outputDir = "../src/bin";

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
    .Description("Builds Cake.CD")
    .Does(() =>
{
    DotNetCoreRestore(sourceDir + "/Cake.CD.csproj");
    
    var settings = new DotNetCoreBuildSettings
    {
        Configuration = "Release",
        ArgumentCustomization = args => args.Append("/p:Version=" + packageVersion)
    };
    DotNetCoreBuild(sourceDir + "/Cake.CD.csproj", settings);
});

Task("Package")
    .Description("Creates nuget package")
    .IsDependentOn("Build")
    .Does(() =>
{
    var nugetPackSettings = new NuGetPackSettings {
        Version = packageVersion
    };
    NuGetPack("Cake.CD.nuspec", nugetPackSettings);
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