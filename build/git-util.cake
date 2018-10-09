
/// <summary>
///  Executes a command using git.exe.
/// </summary>
/// <param name="repositoryDir">The directory of the git repository. This will be the working directory. Specify null to not change the directory (eg. for clone).</param>
/// <param name="command">The command to run.</param>
/// <param name="args">Any additional arguments to the command.</param>
void GitCommand(DirectoryPath repositoryDir, string command, params string[] args)
{
    var gitPath = Context.Tools.Resolve("git.exe");
    if (gitPath == null)
        throw new Exception($"Unable to resolve git.exe tool.");

    var argumentBuilder = new ProcessArgumentBuilder();
    argumentBuilder.Append(command);
    foreach (var arg in args)
        argumentBuilder.Append(arg);

    int exitCode = StartProcess(gitPath, new ProcessSettings {
        WorkingDirectory = repositoryDir, 
        Arguments = argumentBuilder,
        RedirectStandardOutput = true,
        RedirectStandardError = true
    }, out var stdOut, out var stdErr);

    foreach (var output in stdOut)
        Verbose(output);

    // Git apparently writes all messages to stderr... so we just write them as verbose.
     foreach (var output in stdErr)
        Verbose(output);

    if (exitCode != 0)
    {
        throw new Exception($"Git failed with exit code {exitCode}.");
    }
}