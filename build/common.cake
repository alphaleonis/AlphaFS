/// <summary>
/// Deletes the specified directory if it exists.
/// </summary>
/// <param name="path">The directory to delete.</param>
void DeleteDirectoryIfExists(DirectoryPath path, bool force = true)
{
    if (DirectoryExists(path))
    {
        DeleteDirectory(path, new DeleteDirectorySettings { Force = force, Recursive = true });
    }
}

/// <summary>
/// Deletes all contents of the specified directory, optionally also removing read-only files.
/// </summary>
/// <param name="path">The directory to delete all contents of.</param>
/// <param name="force">If true even read-only files will be deleted.</param>
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