using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>
   /// The requested operation could not be completed because the device was not ready.
   /// </summary>
   [Serializable]
   public class DeviceNotReadyException : System.IO.IOException
   {
      private static readonly int s_errorCode = Win32Errors.GetHrFromWin32Error(Win32Errors.ERROR_NOT_READY);

      /// <summary>
      /// Initializes a new instance of the <see cref="DeviceNotReadyException"/> class.
      /// </summary>
      public DeviceNotReadyException()
         : base(Alphaleonis.Win32.Resources.EDeviceNotReady, s_errorCode)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="DeviceNotReadyException"/> class.
      /// </summary>
      /// <param name="message">The message.</param>
      public DeviceNotReadyException(string message)
         : base(message, s_errorCode)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="DeviceNotReadyException"/> class.
      /// </summary>
      /// <param name="message">The message.</param>
      /// <param name="innerException">The inner exception.</param>
      public DeviceNotReadyException(string message, Exception innerException)
         : base(message, innerException)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="DeviceNotReadyException"/> class.
      /// </summary>
      /// <param name="info">The data for serializing or deserializing the object.</param>
      /// <param name="context">The source and destination for the object.</param>
      protected DeviceNotReadyException(SerializationInfo info, StreamingContext context)
         : base(info, context)
      {
      }

   }
}
