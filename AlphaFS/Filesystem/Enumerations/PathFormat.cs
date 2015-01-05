using System;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>Indicates the format of a path passed to a method.</summary>
   public enum PathFormat
   {
      /// <summary>The format of the path is automatically detected by the method and internally converted to an extended length path.</summary>
      Auto,
      /// <summary>
      /// The path is a full path in standard format. Internally it will be converted to an extended length path. Using this option has a very slight performance
      /// advantage compared to using <see cref="Auto"/>.
      /// </summary>
      Standard,

      /// <summary>
      /// The path is an extended length path. No additional processing will be done on the path, and it will be used as is. This option has a slight performance 
      /// advantage to using the <see cref="Auto"/> option.
      /// </summary>
      ExtendedLength
   }
}
