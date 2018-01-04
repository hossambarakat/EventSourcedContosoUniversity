///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(ctx =>
{
   // Executed BEFORE the first task.
   Information("Running tasks...");
});

Teardown(ctx =>
{
   // Executed AFTER the last task.
   Information("Finished running tasks.");
});

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////
var artifactDirectory = "./artifacts/";

Task("Build")
    .Does(() => 
    {
        var solution = "./EventSourcedContosoUniversity.sln";
        Information("Building {0}", solution);
        
        NuGetRestore(solution);
        MSBuild(solution, settings => 
            settings
                .SetConfiguration(configuration)
                .SetVerbosity(Verbosity.Minimal)
        );
    });

Task("RunUnitTests")
    .IsDependentOn("Build")
    .Does(() => 
    {

    });

Task("Package")
    .IsDependentOn("Build")
    .IsDependentOn("RunUnitTests")
    .Does(() => 
    {

    });

Task("Default")
    .IsDependentOn("Package")
    .Does(() => {
    
    });

RunTarget(target);
