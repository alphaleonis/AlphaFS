// TODO: Set build version 
#addin nuget:?package=Cake.DocFx
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using RestSharp;
#addin nuget:?package=Cake.Git
#addin nuget:?package=Cake.Incubator
#addin nuget:?package=RestSharp
#tool "docfx.console"

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var ToolsDirectory = Directory("./tools/");
var ArtifactsDirectory = Directory("./artifacts");
var SolutionFile = "./AlphaFs.sln";
var DocFxArtifactsDirectory = ArtifactsDirectory.Path.Combine("docs");
var WorkDirectory = ToolsDirectory.Path.Combine("_work");
var GitHubProject = BuildSystem.AppVeyor.IsRunningOnAppVeyor ? BuildSystem.AppVeyor.Environment.Repository.Name : "alphaleonis/AlphaFS";
var DocFxBranchName = "gh-pages";
var DocFxArtifactName = "artifacts/docs.zip";
var GitHubCommitName = "AppVeyor";
var GitHubCommitEMail = "alphaleonis@users.noreply.github.com";

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
        Information($"AppVeyor Build JobId: {BuildSystem.AppVeyor.Environment.JobId}");
    }
});

Teardown(ctx =>
{    
});

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////

Task("SetBuildNumber")
    .WithCriteria(AppVeyor.IsRunningOnAppVeyor)
    .Does(() => 
    {
        Warning("Setting a build number");
    });

Task("Clean")
    .IsDependentOn("SetBuildNumber")
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
        var projects = GetFiles("./tests/**/*.csproj");
        foreach (var project in projects)
        {
            DotNetCoreTest(project.FullPath, settings);
        }        
    });

Task("PublishTestResults")
    .IsDependentOn("Test")
    .WithCriteria(() => BuildSystem.IsRunningOnAppVeyor)
    .Does(() => 
    {
        foreach (var trx in GetFiles("./**/*.trx"))
        {
            UploadFile($"https://ci.appveyor.com/api/testresults/mstest/{BuildSystem.AppVeyor.Environment.JobId}", trx);                
        }
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

        var projects = GetFiles("./src/**/*.csproj");
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
        DocFxBuild("./docs/docfx.json");
    });
    
var DocPublishTask = Task("DocPublish")
    .Does(() => 
    {        
        // 'ALPHALEONIS_DEPLOY_BUILD_ID'
        // 'ALPHALEONIS_DEPLOY_SOURCE_PROJECT_ID'
        // 'ALPHALEONIS_DEPLOY_BUILD_NUMBER'
        // 'ALPHALEONIS_DEPLOY_BUILD_VERSION'
        // 'ALPHALEONIS_DEPLOY_JOB_ID'
        // 'ALPHALEONIS_DEPLOY_PROJECT_SLUG'
         
        var docFxArtifactZip = WorkDirectory.CombineWithFilePath("docfx-source.zip");
        var gitCloneDir = MakeAbsolute(WorkDirectory.Combine(Directory("repo")));
        var gitCloneUrl = $"https://github.com/{GitHubProject}.git";
        var accessToken = EnvironmentVariable("access_token");
        var gitCommitMessage = "Updated documentation";
            var sourceBuildVersion = EnvironmentVariable("ALPHALEONIS_DEPLOY_BUILD_VERSION", "0.0.0");

        EnsureDirectoryExists(WorkDirectory);

        if (!BuildSystem.IsLocalBuild) 
        {
            if (String.IsNullOrEmpty(accessToken)) {
                BuildSystem.AppVeyor.AddErrorMessage("No access token for git access was specified. Expecting environment variable named 'access_token'.");                
                throw new Exception("No access token for git access was specified. Expecting environment variable named 'access_token'.");
            }
            var apiUrl = "https://ci.appveyor.com/api"; 
            var jobId = EnvironmentVariable<string>("ALPHALEONIS_DEPLOY_JOB_ID", null);

            if (String.IsNullOrEmpty(jobId))
                throw new ArgumentException($"Missing environment variable ALPHALEONIS_DEPLOY_JOB_ID");
            
            // Download the artifact from the build server if this is not a local build.
            DownloadArtifact(apiUrl, jobId, DocFxArtifactName, sourceBuildVersion, docFxArtifactZip);
        }
        else
        {
            if (!DirectoryExists(DocFxArtifactsDirectory) || !GetFiles(DocFxArtifactsDirectory.FullPath + "/**/*").Any())
                throw new DirectoryNotFoundException($"{DocFxArtifactsDirectory} does not exist or is empty.");
        }

        DeleteDirectoryIfExists(gitCloneDir);

        Information($"Cloning {gitCloneUrl} to {gitCloneDir}");

        GitClone(gitCloneUrl, gitCloneDir, new GitCloneSettings { BranchName = DocFxBranchName, Checkout = true });
        
        var allFilesInGitRepo = GetFiles(gitCloneDir.FullPath + "/**/*").Where(f => !MakeAbsolute(gitCloneDir).GetRelativePath(f).FullPath.StartsWith(".git/")).ToArray();
        
        if (allFilesInGitRepo.Length > 0)
        {
            Verbose($"Removing all files from GIT repository in {gitCloneDir}");
            GitRemove(gitCloneDir, true, allFilesInGitRepo);      
        }

        if (!BuildSystem.IsLocalBuild)
        {
            Unzip(docFxArtifactZip, gitCloneDir);
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
            GitPush(gitCloneDir, accessToken, "x-oauth-basic", DocFxBranchName);
            AppVeyor.AddInformationalMessage($"Pushed changes from build {sourceBuildVersion}.");
        }
        else
        {
            Information("No changes made to documentation files, nothing to publish.");
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

void DeleteDirectoryIfExists(DirectoryPath path)
{
    if (DirectoryExists(path))
    {
        DeleteDirectory(path, new DeleteDirectorySettings { Force = true, Recursive = true });
    }
}

void CleanDirectory(DirectoryPath path, bool force)
{
    if (force && DirectoryExists(path))
    {
        Verbose("Removing ReadOnly attributes on all files in directory {0}", path);                
        foreach (var file in System.IO.Directory.GetFiles(MakeAbsolute(path).FullPath, "*", SearchOption.AllDirectories))
        {
            System.IO.File.SetAttributes(file, System.IO.File.GetAttributes(file) & ~FileAttributes.ReadOnly);            
        }
    }

    CleanDirectory(path);
}

void DownloadArtifact(string apiUrl, string jobId, string artifactName, string sourceBuildVersion, FilePath targetFile)
{            
    var artifacts = RestInvoke<List<ArtifactInfo>>(apiUrl,  $"buildjobs/{jobId}/artifacts", Method.GET);

    ArtifactInfo matchingArtifact = null;
    foreach (var artifact in artifacts)
    {
        Verbose($"- Found artifact: {artifact.FileName} of type {artifact.Type}");
        if (artifact.FileName.Equals(artifactName, StringComparison.OrdinalIgnoreCase)) {
            if (matchingArtifact != null)
                throw new Exception($"Multiple artifacts matching the name \"{artifactName}\" were found.");
            else
                matchingArtifact = artifact;
        }
    }

    if (matchingArtifact == null)
        throw new FileNotFoundException($"Unable to find artifact named \"{artifactName}\" in build {sourceBuildVersion}.");
    
    Information($"Found matching artifact: {matchingArtifact.FileName}, type={matchingArtifact.Type}, size={matchingArtifact.Size}b");

    var url = apiUrl + $"/buildjobs/{jobId}/artifacts/{WebUtility.UrlEncode(matchingArtifact.FileName)}";      
    EnsureDirectoryExists("_download");
    Information($"Downloading artifact {matchingArtifact.FileName} to {targetFile.FullPath}");
    DownloadFile(url, targetFile);
}

T RestInvoke<T>(string baseUrl, string resource, Method method) where T : new()
{
    var client = new RestClient(baseUrl);
    var request = new RestRequest(resource, method);
    Verbose($"{method} {baseUrl}/{resource}");
    var result = client.Execute<T>(request);
    if (result.StatusCode != HttpStatusCode.OK || !result.IsSuccessful)
    {
        throw new Exception($"REST call to {baseUrl}/{resource} failed with status {result.StatusCode}. Error message: {result.ErrorMessage}");
    }
    return result.Data;    
}

class ArtifactInfo
{
    public string FileName { get; set; }
    public string Type { get; set; }
    public long Size{ get; set;}
    public DateTime Created { get; set; }
    public string RelativeName => System.IO.Path.GetFileName(FileName);
}
