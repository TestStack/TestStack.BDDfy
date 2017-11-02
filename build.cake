#tool "nuget:?package=GitReleaseNotes"
#tool "nuget:?package=GitVersion.CommandLine"

var target = Argument("target", "Default");
var bddfyProj = "./src/TestStack.BDDfy/TestStack.BDDfy.csproj";
var outputDir = "./artifacts/";

Task("Clean")
    .Does(() => {
        if (DirectoryExists(outputDir))
        {
            DeleteDirectory(outputDir, recursive:true);
        }
    });

Task("Restore")
    .Does(() => {
        DotNetCoreRestore("src");
    });

GitVersion versionInfo = null;
Task("Version")
    .Does(() => {
		GitVersion(new GitVersionSettings{
            UpdateAssemblyInfo = true,
            OutputType = GitVersionOutput.BuildServer
        });
        versionInfo = GitVersion(new GitVersionSettings{ OutputType = GitVersionOutput.Json });        
    });

Task("Build")
    .IsDependentOn("Clean")
    .IsDependentOn("Version")
    .IsDependentOn("Restore")
    .Does(() => {
        MSBuild("./src/TestStack.BDDfy.sln");
    });

Task("Test")
    .IsDependentOn("Build")
    .Does(() => {
        DotNetCoreTest("./src/TestStack.BDDfy.Tests");
        DotNetCoreTest("./src/Samples/TestStack.BDDfy.Samples");
    });

Task("Package")
    .IsDependentOn("Test")
    .Does(() => {
        var settings = new DotNetCorePackSettings
        {
			ArgumentCustomization = args=> args.Append(" --include-symbols /p:PackageVersion=" + versionInfo.NuGetVersion),
            OutputDirectory = outputDir,
            NoBuild = true
        };
		
        DotNetCorePack(bddfyProj, settings);
		
        var releaseNotesExitCode = StartProcess(
            @"tools\GitReleaseNotes\tools\gitreleasenotes.exe", 
            new ProcessSettings { Arguments = ". /o artifacts/releasenotes.md" });

        if (string.IsNullOrEmpty(System.IO.File.ReadAllText("./artifacts/releasenotes.md")))
            System.IO.File.WriteAllText("./artifacts/releasenotes.md", "No issues closed since last release");

        if (releaseNotesExitCode != 0) throw new Exception("Failed to generate release notes");

        System.IO.File.WriteAllLines(outputDir + "artifacts", new[]{
            "nuget:TestStack.BDDfy." + versionInfo.NuGetVersion + ".nupkg",
            "nugetSymbols:TestStack.BDDfy." + versionInfo.NuGetVersion + ".symbols.nupkg",
            "releaseNotes:releasenotes.md"
        });

        if (AppVeyor.IsRunningOnAppVeyor)
        {
            foreach (var file in GetFiles(outputDir + "**/*"))
                AppVeyor.UploadArtifact(file.FullPath);
        }
    });

Task("Default")
    .IsDependentOn("Package");

RunTarget(target);