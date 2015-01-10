using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>
   /// The exception that is thrown when an attempt to create a directory or file that already exists was made.
   /// </summary>
   [Serializable]
   public class AlreadyExistsException : System.IO.IOException
   {
      private static readonly int s_errorCode = Win32Errors.GetHrFromWin32Error(Win32Errors.ERROR_ALREADY_EXISTS);

      /// <summary>
      /// Initializes a new instance of the <see cref="AlreadyExistsException"/> class.
      /// </summary>
      public AlreadyExistsException()
         : base(Alphaleonis.Win32.Resources.EFileOrDirectoryAlreadyExists, s_errorCode)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="AlreadyExistsException"/> class.
      /// </summary>
      /// <param name="message">The message.</param>
      public AlreadyExistsException(string message)
         : base(message, s_errorCode)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="AlreadyExistsException"/> class.
      /// </summary>
      /// <param name="message">The message.</param>
      /// <param name="innerException">The inner exception.</param>
      public AlreadyExistsException(string message, Exception innerException)
         : base(message, innerException)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="AlreadyExistsException"/> class.
      /// </summary>
      /// <param name="info">The data for serializing or deserializing the object.</param>
      /// <param name="context">The source and destination for the object.</param>
      protected AlreadyExistsException(SerializationInfo info, StreamingContext context)
         : base(info, context)
      {
      }
   }
}
