
using System;

namespace Alphaleonis.Win32.Filesystem
{
    /// <summary>
    /// Callback delegate used by some of the Directory methods to obtain a <see cref="EnumerationExceptionDecision"/> of what to do in case of enumeration traversal failure.
    /// </summary>
    /// <param name="path">The path of failed directory of file</param>
    /// <param name="e">The exception that occured during operation</param>
    /// <returns><see cref="EnumerationExceptionDecision"/></returns>
    public delegate EnumerationExceptionDecision DirectoryEnumerationExceptionHandler(string path, Exception e);
}
