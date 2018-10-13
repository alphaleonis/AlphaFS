#l "rest-util.cake"
void DownloadAppVeyorArtifact(string apiUrl, string jobId, string artifactName, string sourceBuildVersion, FilePath targetFile)
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
    Information($"Downloading artifact {matchingArtifact.FileName} to {targetFile.FullPath}");
    DownloadFile(url, targetFile);
}

void PublishAppVeyorTestResult(FilePath trxFile, string appVeyorApiBaseUrl = "https://ci.appveyor.com/api")
{
    if (BuildSystem.IsRunningOnAppVeyor)
    {
        UploadFile($"{appVeyorApiBaseUrl}/testresults/mstest/{BuildSystem.AppVeyor.Environment.JobId}", trxFile);                
    }
}
class ArtifactInfo
{
    public string FileName { get; set; }
    public string Type { get; set; }
    public long Size{ get; set;}
    public DateTime Created { get; set; }
    public string RelativeName => System.IO.Path.GetFileName(FileName);
}