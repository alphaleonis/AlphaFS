using System.Xml.Linq;
#addin nuget:?package=Cake.DocFx
#addin nuget:?package=Cake.Git
#addin nuget:?package=Cake.Incubator
#l "build/appveyor-util.cake"
#l "build/common.cake"
#l "build/git-util.cake"
#tool "docfx.console"

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var ToolsDirectory = Directory("./tools/");
var ArtifactsDirectory = Directory("./artifacts");
var SolutionFile = "./AlphaFs.sln";
var DocFxFile = "./docs/docfx.json";
var DocFxArtifactsDirectory = ArtifactsDirectory.Path.Combine("docs");
var WorkDirectory = ToolsDirectory.Path.Combine("_work");
var GitHubProject = BuildSystem.AppVeyor.IsRunningOnAppVeyor ? BuildSystem.AppVeyor.Environment.Repository.Name : "alphaleonis/AlphaFS";
var DocFxBranchName = "gh-pages-lab";
var DocFxArtifactName = "artifacts/docs.zip";
var GitHubCommitName = "AppVeyor";
var GitHubCommitEMail = "alphaleonis-build@users.noreply.github.com";
var AppVeyorApiBaseUrl = "https://ci.appveyor.com/api";
var TestProjectsPattern = "./tests/**/*.csproj";
var PackProjectsPattern = "./src/**/*.csproj";

var msBuildSettings = new DotNetCoreMSBuildSettings
{
    //MaxCpuCount = 1
};

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(ctx =>
{
    if (BuildSystem.AppVeyor.IsRunningOnAppVeyor) 
    {        
        Information("Updating build number");
        string targetVersion;
        if (AppVeyor.Environment.Repository.Branch == DocFxBranchName)
        {
            foreach (var variable in EnvironmentVariables())
            {
                if (variable.Key.StartsWithIgnoreCase("ALPHALEONIS"))
                {
                    Verbose($"{variable.Key}={variable.Value}");
                }
            }
            targetVersion = EnvironmentVariable<string>("ALPHALEONIS_DEPLOY_BUILD_VERSION", null);
        }
        else
        {
            // Update BuildNumber.
            var propsFile = File("build/common.props");
            string major = XmlPeek(propsFile, "//PropertyGroup/Major");
            string minor = XmlPeek(propsFile, "//PropertyGroup/Minor");
            string revision = XmlPeek(propsFile, "//PropertyGroup/Revision");

            if (String.IsNullOrEmpty(major) || 
                String.IsNullOrEmpty(minor) ||
                String.IsNullOrEmpty(revision))
            {
                throw new Exception($"Unable to find Major, Minor and/or Build version in {propsFile}.");
            }
            targetVersion = $"{major}.{minor}.{revision}";
        }

        if (targetVersion != null)
            BuildSystem.AppVeyor.UpdateBuildVersion($"{targetVersion}.{AppVeyor.Environment.Build.Number}");        
    }
});

Teardown(ctx =>
{    
});

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////
Task("Clean")
    .Does(() =>
    {        
        CleanDirectory(ArtifactsDirectory);
    });

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() => 
    {
        DotNetCoreRestore(SolutionFile);
    });
    
Task("Build")
    .IsDependentOn("Restore")
    .Does(() =>
    {
        var path = MakeAbsolute(new DirectoryPath(SolutionFile));
        DotNetCoreBuild(path.FullPath, new DotNetCoreBuildSettings
        {
            Configuration = configuration,
            NoRestore = true,
            DiagnosticOutput = true,
            MSBuildSettings = msBuildSettings,
            Verbosity = DotNetCoreVerbosity.Minimal
        });
    });

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
    {
        DotNetCoreTestSettings settings = new DotNetCoreTestSettings()
        {
            NoBuild = true,
            NoRestore = true,
            Logger = "trx"            
        };
        var projects = GetFiles(TestProjectsPattern);
        foreach (var project in projects)
        {
            DotNetCoreTest(project.FullPath, settings);
        }        
    })
    .OnError(error => 
    {
        UploadTestResults();        
    });

void UploadTestResults()
{
    if (BuildSystem.IsRunningOnAppVeyor)
    {
        foreach (var trx in GetFiles("./**/*.trx"))
        {
            PublishAppVeyorTestResult(trx, AppVeyorApiBaseUrl);            
        }
    }
}

Task("PublishTestResults")
    .IsDependentOn("Test")
    .WithCriteria(() => BuildSystem.IsRunningOnAppVeyor)
    .Does(() => 
    {
        UploadTestResults();
    });

Task("Pack")
    .IsDependentOn("PublishTestResults")    
    .Does(() => 
    {
        var settings = new DotNetCorePackSettings
        {
            Configuration = configuration,
            OutputDirectory = ArtifactsDirectory,
            MSBuildSettings = msBuildSettings, 
            NoBuild = true
        };

        var projects = GetFiles(PackProjectsPattern);
        foreach(var project in projects)
        {
            DotNetCorePack(project.FullPath, settings);
        }
    });

Task("DocClean")
    .Does(() => 
    {        
        CleanDirectory(DocFxArtifactsDirectory, true);
    });

Task("DocBuild")
    .IsDependentOn("DocClean")
    .Does(() => 
    {        
        DocFxMetadata(DocFxFile);
        DocFxBuild(DocFxFile);
    });

var DocPublishTask = Task("DocPublish")
    .Does(() => 
    {             
        var artifactTempZipPath = WorkDirectory.CombineWithFilePath("docfx-artifact.zip");
        var gitCloneDir = MakeAbsolute(WorkDirectory.Combine(Directory("repo")));
        var gitCloneUrl = $"git@github.com:{GitHubProject}.git";
        var gitCommitMessage = "Updated documentation";
        var sourceBuildVersion = EnvironmentVariable("ALPHALEONIS_DEPLOY_BUILD_VERSION", "0.0.0");

        EnsureDirectoryExists(WorkDirectory);

        if (!BuildSystem.IsLocalBuild) 
        {
            var jobId = EnvironmentVariable<string>("ALPHALEONIS_DEPLOY_JOB_ID", null);

            if (String.IsNullOrEmpty(jobId))
                throw new ArgumentException($"Missing environment variable ALPHALEONIS_DEPLOY_JOB_ID");
            
            // Download the artifact from the build server if this is not a local build.
            DownloadAppVeyorArtifact(AppVeyorApiBaseUrl, jobId, DocFxArtifactName, sourceBuildVersion, artifactTempZipPath);
        }
        else
        {
            if (!DirectoryExists(DocFxArtifactsDirectory) || !GetFiles(DocFxArtifactsDirectory.FullPath + "/**/*").Any())
                throw new DirectoryNotFoundException($"{DocFxArtifactsDirectory} does not exist or is empty.");
        }

        DeleteDirectoryIfExists(gitCloneDir);

        Information($"Cloning {gitCloneUrl} to {gitCloneDir}");

        GitCommand(null, "clone", "-b", DocFxBranchName, "--single-branch", gitCloneUrl, gitCloneDir.FullPath);
        
        var allFilesInGitRepo = GetFiles(gitCloneDir.FullPath + "/**/*").Where(f => !MakeAbsolute(gitCloneDir).GetRelativePath(f).FullPath.StartsWith(".git/")).ToArray();
        
        if (allFilesInGitRepo.Length > 0)
        {
            Verbose($"Removing all files from GIT repository in {gitCloneDir}");
            GitRemove(gitCloneDir, true, allFilesInGitRepo);      
        }

        if (!BuildSystem.IsLocalBuild)
        {
            Unzip(artifactTempZipPath, gitCloneDir);
        }
        else
        {
            Verbose("Copy all files from local build to the local temporary git repository.");
            CopyDirectory(DocFxArtifactsDirectory, gitCloneDir);
        }

        Verbose($"Staging all files in {gitCloneDir}");
        GitAddAll(gitCloneDir);

        if (GitHasUncommitedChanges(gitCloneDir))
        {            
            Verbose("Changes detected, committing.");        
            var commit = GitCommit(gitCloneDir, GitHubCommitName, GitHubCommitEMail, gitCommitMessage);
            Information("Pushing changes to remote.");            
            GitCommand(gitCloneDir, "push");
            if (BuildSystem.AppVeyor.IsRunningOnAppVeyor)
                AppVeyor.AddInformationalMessage($"Pushed changes from build {sourceBuildVersion}.");
        }
        else
        {
            Information("No changes made to documentation files, nothing to publish.");
            if (BuildSystem.AppVeyor.IsRunningOnAppVeyor)
                AppVeyor.AddInformationalMessage("No changes made to documentation files, nothing to publish.");
        }

        Information($"Cleaning up {gitCloneDir}");
        CleanDirectory(gitCloneDir, true);
    });

Task("Default")
    .IsDependentOn("Pack")
    .IsDependentOn("DocBuild");

// Sort of an ugly hack due to conditional dependencies not being supported.
if (BuildSystem.IsLocalBuild)
{
    DocPublishTask.IsDependentOn("DocBuild");
}

RunTarget(target);




