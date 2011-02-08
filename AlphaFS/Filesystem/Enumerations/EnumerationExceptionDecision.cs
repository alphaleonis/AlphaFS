
namespace Alphaleonis.Win32.Filesystem
{
    /// <summary>
    /// The collection of values that <see cref="Alphaleonis.Win32.Filesystem.DirectoryEnumerationExceptionHandler"/> should return in case of traversal failure.
    /// </summary>
    public enum EnumerationExceptionDecision
    {
        /// <summary>
        /// 
        /// </summary>
        Skip,
        /// <summary>
        /// 
        /// </summary>
        Retry,
        /// <summary>
        /// 
        /// </summary>
        Abort,
        /// <summary>
        /// 
        /// </summary>
        Suppress
    }
}
