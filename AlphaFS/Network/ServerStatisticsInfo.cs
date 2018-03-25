/*  Copyright (C) 2008-2018 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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

namespace Alphaleonis.Win32.Network
{
   /// <summary>Contains operating statistics for the Server service.</summary>
   [Serializable]
   public sealed class ServerStatisticsInfo
   {
      [NonSerialized] private DateTime _dateTimeNowUtc;
      [NonSerialized] private NativeMethods.STAT_SERVER_0 _serverStat;


      /// <summary>Create a ServerStatisticsInfo instance from the local host.</summary>
      public ServerStatisticsInfo() : this(Environment.MachineName, null)
      {
      }


      /// <summary>Create a ServerStatisticsInfo instance from the specified host name.</summary>
      public ServerStatisticsInfo(string hostName) : this(hostName, null)
      {
      }


      /// <summary>Create a ServerStatisticsInfo instance from the specified host name.</summary>
      internal ServerStatisticsInfo(string hostName, NativeMethods.STAT_SERVER_0? serverStat)
      {
         HostName = !Utils.IsNullOrWhiteSpace(hostName) ? hostName : Environment.MachineName;

         if (serverStat.HasValue)
         {
            _dateTimeNowUtc = DateTime.UtcNow;

            _serverStat = (NativeMethods.STAT_SERVER_0) serverStat;
         }

         else
            Refresh();
      }


      #region Properties

      /// <summary>The number of server access permission errors.</summary>
      public int AccessPermissionErrors
      {
         get { return (int) _serverStat.sts0_permerrors; }
      }


      /// <summary>The average server response time.</summary>
      public TimeSpan AverageResponseTime
      {
         get { return TimeSpan.FromMilliseconds(_serverStat.sts0_avresponse); }
      }


      /// <summary>The number of times the server required a big buffer but failed to allocate one. This value indicates that the server parameters may need adjustment.</summary>
      public int BufferAllocationFailed
      {
         get { return (int) _serverStat.sts0_bigbufneed; }
      }


      /// <summary>The number of times the server required a request buffer but failed to allocate one. This value indicates that the server parameters may need adjustment.</summary>
      public int BufferRequestFailed
      {
         get { return (int) _serverStat.sts0_reqbufneed; }
      }


      /// <summary>The number of server bytes received from the network.</summary>
      public long BytesReceived
      {
         get { return Filesystem.NativeMethods.ToLong(_serverStat.sts0_bytesrcvd_high, _serverStat.sts0_bytesrcvd_low); }
      }


      /// <summary>The number of server bytes received from the network, formatted as a unit size.</summary>
      public string BytesReceivedUnitSize
      {
         get { return Utils.UnitSizeToText(BytesReceived); }
      }


      /// <summary>The number of server bytes sent to the network.</summary>
      public long BytesSent
      {
         get { return Filesystem.NativeMethods.ToLong(_serverStat.sts0_bytessent_high, _serverStat.sts0_bytessent_low); }
      }


      /// <summary>The number of server bytes sent to the network, formatted as a unit size.</summary>
      public string BytesSentUnitSize
      {
         get { return Utils.UnitSizeToText(BytesSent); }
      }
      

      /// <summary>The number of times a server device is opened.</summary>
      public int DevicesOpened
      {
         get { return (int) _serverStat.sts0_devopens; }
      }


      /// <summary>The number of times a file is opened on a server. This includes the number of times named pipes are opened.</summary>
      public int FilesOpened
      {
         get { return (int) _serverStat.sts0_fopens; }
      }


      /// <summary>The host name from where the statistics are gathered.</summary>
      public string HostName { get; private set; }


      /// <summary>The number of server print jobs spooled.</summary>
      public int JobsQueued
      {
         get { return (int) _serverStat.sts0_jobsqueued; }
      }


      /// <summary>The number of server password violations.</summary>
      public int PasswordViolations
      {
         get { return (int) _serverStat.sts0_pwerrors; }
      }


      /// <summary>The number of times the server sessions failed with an error.</summary>
      public int SessionsFailed
      {
         get { return (int) _serverStat.sts0_serrorout; }
      }


      /// <summary>The number of times the server session started.</summary>
      public int SessionsStarted
      {
         get { return (int) _serverStat.sts0_sopens; }
      }


      /// <summary>The number of times the server session automatically disconnected.</summary>
      public int SessionsTimedOut
      {
         get { return (int) _serverStat.sts0_stimedout; }
      }


      /// <summary>The local time when statistics collection started or when the statistics were last cleared.</summary>
      public DateTime StatisticsStartTime
      {
         get { return StatisticsStartTimeUtc.ToLocalTime(); }
      }


      /// <summary>The time when statistics collection started or when the statistics were last cleared.</summary>
      public DateTime StatisticsStartTimeUtc
      {
         get { return new DateTime((_dateTimeNowUtc - new DateTime(_serverStat.sts0_start, DateTimeKind.Utc)).Ticks, DateTimeKind.Utc); }
      }


      /// <summary>The number of server system errors.</summary>
      public int SystemErrors
      {
         get { return (int) _serverStat.sts0_syserrors; }
      }

      #endregion // Properties


      #region Methods

      /// <summary>Refreshes the state of the object.</summary>
      public void Refresh()
      {
         _dateTimeNowUtc = DateTime.UtcNow;

         _serverStat = Host.GetNetStatisticsNative<NativeMethods.STAT_SERVER_0>(true, HostName);
      }


      /// <summary>Returns the local time when statistics collection started or when the statistics were last cleared.</summary>
      /// <returns>A string that represents this instance.</returns>
      public override string ToString()
      {
         return HostName;
      }


      /// <summary>Determines whether the specified Object is equal to the current Object.</summary>
      /// <param name="obj">Another object to compare to.</param>
      /// <returns><see langword="true"/> if the specified Object is equal to the current Object; otherwise, <see langword="false"/>.</returns>
      public override bool Equals(object obj)
      {
         if (null == obj || GetType() != obj.GetType())
            return false;

         var other = obj as ServerStatisticsInfo;

         return null != other && null != other.HostName && other.HostName.Equals(HostName, StringComparison.OrdinalIgnoreCase) && other.StatisticsStartTimeUtc.Equals(StatisticsStartTimeUtc);
      }


      /// <summary>Serves as a hash function for a particular type.</summary>
      /// <returns>A hash code for the current Object.</returns>
      public override int GetHashCode()
      {
         return null != HostName ? HostName.GetHashCode() : 0;
      }


      /// <summary>Implements the operator ==</summary>
      /// <param name="left">A.</param>
      /// <param name="right">B.</param>
      /// <returns>The result of the operator.</returns>
      public static bool operator ==(ServerStatisticsInfo left, ServerStatisticsInfo right)
      {
         return ReferenceEquals(left, null) && ReferenceEquals(right, null) || !ReferenceEquals(left, null) && !ReferenceEquals(right, null) && left.Equals(right);
      }


      /// <summary>Implements the operator !=</summary>
      /// <param name="left">A.</param>
      /// <param name="right">B.</param>
      /// <returns>The result of the operator.</returns>
      public static bool operator !=(ServerStatisticsInfo left, ServerStatisticsInfo right)
      {
         return !(left == right);
      }

      #endregion // Methods
   }
}
