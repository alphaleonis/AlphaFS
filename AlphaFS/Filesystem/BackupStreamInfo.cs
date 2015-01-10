using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>
   /// The <see cref="BackupStreamInfo"/> structure contains stream header data.
   /// </summary>
   /// <seealso cref="BackupFileStream"/>
   public sealed class BackupStreamInfo
   {
      #region Private Fields

      private readonly long m_size;
      private readonly string m_name;
      private readonly BackupStreamType m_streamType;
      private readonly StreamAttributes m_attributes;

      #endregion

      #region Constructor

      /// <summary>
      /// Initializes a new instance of the <see cref="BackupStreamInfo"/> class.
      /// </summary>
      /// <param name="streamID">The stream ID.</param>
      /// <param name="name">The name.</param>
      internal BackupStreamInfo(NativeMethods.WIN32_STREAM_ID streamID, string name)
      {
         m_size = (long)streamID.StreamSize;
         m_name = name;
         m_attributes = (StreamAttributes)streamID.StreamAttributes;
         m_streamType = (BackupStreamType)streamID.StreamType;
      }

      #endregion

      #region Public Properties

      /// <summary>
      /// Gets the size of the data in the substream, in bytes.
      /// </summary>
      /// <value>The size of the data in the substream, in bytes.</value>
      public long Size { get { return m_size; } }

      /// <summary>
      /// Gets a string that specifies the name of the alternative data stream.
      /// </summary>
      /// <value>A string that specifies the name of the alternative data stream.</value>
      public string Name { get { return m_name; } }

      /// <summary>
      /// Gets the type of the data in the stream.
      /// </summary>
      /// <value>The type of the data in the stream.</value>
      public BackupStreamType StreamType { get { return m_streamType; } }

      /// <summary>
      /// Gets the attributes of the data to facilitate cross-operating system transfer.
      /// </summary>
      /// <value>Attributes of the data to facilitate cross-operating system transfer.</value>
      public StreamAttributes Attributes { get { return m_attributes; } }

      #endregion
   }
}
