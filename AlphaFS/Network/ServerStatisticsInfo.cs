/*  Copyright (C) 2008-2017 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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

using System;
using System.Globalization;
using Alphaleonis.Win32.Filesystem;

namespace Alphaleonis.Win32.Network
{
   /// <summary>Contains the identification number of a connection, number of open files, connection time, number of users on the connection, and the type of connection.</summary>
   [Serializable]
   public sealed class ServerStatisticsInfo
   {
      #region Constructor

      /// <summary>Create a OpenConnectionInfo instance.</summary>
      internal ServerStatisticsInfo(string hostName, NativeMethods.STAT_SERVER_0 serverStat)
      {
         var ts = TimeSpan.FromSeconds(serverStat.sts0_start);

         //StartTime =  DateTime.Now.
         //.DateAdd("s", NumberOfSeconds, "1/1/1970")


         AverageResponseTime = TimeSpan.FromMilliseconds(serverStat.sts0_avresponse);
      }

      #endregion // Constructor


      #region Properties

      /// <summary>The local time when statistics collection started (or when the statistics were last cleared).</summary>
      public DateTime StartTime
      {
         get { return StartTime.ToLocalTime(); }
      }

      /// <summary>The time when statistics collection started (or when the statistics were last cleared).</summary>
      public DateTime StartTimeUtc { get; set; }


      /// <summary>The number of times a file is opened on a server. This includes the number of times named pipes are opened.</summary>
      public int sts0_fopens { get; set; }

      /// <summary>The number of times a server device is opened.</summary>
      public int sts0_devopens { get; set; }

      /// <summary>The number of server print jobs spooled.</summary>
      public int JobsQueued { get; set; }

      /// <summary>The number of times the server session started.</summary>
      public int sts0_sopens { get; set; }

      /// <summary>The number of times the server session automatically disconnected.</summary>
      public int sts0_stimedout { get; set; }

      /// <summary>The number of times the server sessions failed with an error.</summary>
      public int sts0_serrorout { get; set; }

      /// <summary>The number of server password violations.</summary>
      public int sts0_pwerrors { get; set; }

      /// <summary>The number of server access permission errors.</summary>
      public int sts0_permerrors { get; set; }

      /// <summary>The number of server system errors.</summary>
      public int sts0_syserrors { get; set; }

      /// <summary>The number of server bytes sent to the network.</summary>
      public int sts0_bytessent_low { get; set; }

      /// <summary>The number of server bytes sent to the network.</summary>
      public int sts0_bytessent_high { get; set; }

      /// <summary>The number of server bytes received from the network.</summary>
      public int sts0_bytesrcvd_low { get; set; }

      /// <summary>The number of server bytes received from the network.</summary>
      public long sts0_bytesrcvd_high { get; set; }

      /// <summary>The average server response time.</summary>
      public TimeSpan AverageResponseTime { get; set; }

      /// <summary>The number of times the server required a request buffer but failed to allocate one. This value indicates that the server parameters may need adjustment.</summary>
      public int BufferAllocationFail { get; set; }

      /// <summary>The number of times the server required a big buffer but failed to allocate one. This value indicates that the server parameters may need adjustment.</summary>
      public int BigBufferNeeded { get; set; }

      #endregion // Properties


      //#region Methods

      ///// <summary>Returns the full path to the share.</summary>
      ///// <returns>A string that represents this instance.</returns>
      //public override string ToString()
      //{
      //   return Id.ToString(CultureInfo.InvariantCulture);
      //}

      //#endregion // Methods
   }
}
