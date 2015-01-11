using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>
   /// The operation could not be completed because the directory was not empty.
   /// </summary>
   [Serializable]
   public class DirectoryNotEmptyException : System.IO.IOException
   {
      private static readonly int s_errorCode = Win32Errors.GetHrFromWin32Error(Win32Errors.ERROR_DIR_NOT_EMPTY);

      /// <summary>
      /// Initializes a new instance of the <see cref="DirectoryNotEmptyException"/> class.
      /// </summary>
      public DirectoryNotEmptyException()
         : base("The directory is not empty.", s_errorCode)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="DirectoryNotEmptyException"/> class.
      /// </summary>
      /// <param name="message">The message.</param>
      public DirectoryNotEmptyException(string message)
         : base(message, s_errorCode)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="DirectoryNotEmptyException"/> class.
      /// </summary>
      /// <param name="message">The message.</param>
      /// <param name="innerException">The inner exception.</param>
      public DirectoryNotEmptyException(string message, Exception innerException)
         : base(message, innerException)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="DirectoryNotEmptyException"/> class.
      /// </summary>
      /// <param name="info">The data for serializing or deserializing the object.</param>
      /// <param name="context">The source and destination for the object.</param>
      protected DirectoryNotEmptyException(SerializationInfo info, StreamingContext context)
         : base(info, context)
      {
      }
   }
}
