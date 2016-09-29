using System;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>Represents a wrapper class for a handle used by the FindFirstFile/FindNextFile Win32 API functions.</summary>
   [SecurityCritical]
   internal sealed class SafeEncryptedFileRawHandle : SafeHandleZeroOrMinusOneIsInvalid
   {
      /// <summary>Constructor that prevents a default instance of this class from being created.</summary>
      private SafeEncryptedFileRawHandle()
         : base(true)
      {
      }

      /// <summary>Constructor that prevents a default instance of this class from being created.</summary>
      /// <param name="handle">The handle.</param>
      /// <param name="ownsHandle">true to reliably release the handle during the finalization phase; false to prevent
      /// reliable release (not recommended).</param>
      public SafeEncryptedFileRawHandle(IntPtr handle, bool ownsHandle)
         : base(ownsHandle)
      {
         SetHandle(handle);
      }

      /// <summary>When overridden in a derived class, executes the code required to free the handle.</summary>
      /// <returns><see langword="true"/> if the handle is released successfully; otherwise, in the event of a catastrophic failure, <see langword="false"/>. In this case, it generates a ReleaseHandleFailed Managed Debugging Assistant.</returns>
      protected override bool ReleaseHandle()
      {
         NativeMethods.CloseEncryptedFileRaw(handle);
         return true;
      }
   }
}
