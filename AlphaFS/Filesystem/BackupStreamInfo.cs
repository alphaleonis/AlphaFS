/*  Copyright (C) 2008-2016 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy 
 *  of this software and associated documentation files (the "Software"), to deal 
 *  in the Software without restriction, including without limitation the rights 
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
 *  copies of the Software, and to permit persons to whom the Software is 
 *  furnished to do so, subject to the following conditions:
 *  
 *  The above copyright notice and this permission notice shall be included in 
 *  all copies or substantial portions of the Software.
 *  
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN 
 *  THE SOFTWARE. 
 */

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>The <see cref="BackupStreamInfo"/> structure contains stream header data.</summary>
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

      /// <summary>Initializes a new instance of the <see cref="BackupStreamInfo"/> class.</summary>
      /// <param name="streamID">The stream ID.</param>
      /// <param name="name">The name.</param>
      internal BackupStreamInfo(NativeMethods.WIN32_STREAM_ID streamID, string name)
      {
         m_name = name;
         m_size = (long) streamID.Size;
         m_attributes = streamID.dwStreamAttributes;
         m_streamType = streamID.dwStreamId;
      }

      #endregion

      #region Public Properties

      /// <summary>Gets the size of the data in the substream, in bytes.</summary>
      /// <value>The size of the data in the substream, in bytes.</value>
      public long Size
      {
         get { return m_size; }
      }

      /// <summary>Gets a string that specifies the name of the alternative data stream.</summary>
      /// <value>A string that specifies the name of the alternative data stream.</value>
      public string Name
      {
         get { return m_name; }
      }

      /// <summary>Gets the type of the data in the stream.</summary>
      /// <value>The type of the data in the stream.</value>
      public BackupStreamType StreamType
      {
         get { return m_streamType; }
      }

      /// <summary>Gets the attributes of the data to facilitate cross-operating system transfer.</summary>
      /// <value>Attributes of the data to facilitate cross-operating system transfer.</value>
      public StreamAttributes Attributes
      {
         get { return m_attributes; }
      }

      #endregion // Public Properties
   }
}
