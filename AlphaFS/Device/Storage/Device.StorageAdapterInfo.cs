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
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Security;

namespace Alphaleonis.Win32.Device
{
   /// <summary>[AlphaFS] Provides access to adapter information of a storage device.</summary>
   [Serializable]
   [SecurityCritical]
   public sealed class StorageAdapterInfo
   {
      #region Constructors

      /// <summary>[AlphaFS] Initializes an empty StorageAdapterInfo instance.</summary>
      public StorageAdapterInfo()
      {
         BusType = StorageBusType.Unknown;

         DeviceNumber = -1;
      }


      internal StorageAdapterInfo(int diskNumber, NativeMethods.STORAGE_ADAPTER_DESCRIPTOR adapter) : this()
      {
         DeviceNumber = diskNumber;

         BusType = (StorageBusType) adapter.BusType;

         AcceleratedTransfer = adapter.AcceleratedTransfer;
         
         AdapterScansDown = adapter.AdapterScansDown;

         AdapterUsesPio = adapter.AdapterUsesPio;

         BusVersion = adapter.BusVersion;

         CommandQueueing = adapter.CommandQueueing;

         MaximumTransferBytes = (int) adapter.MaximumTransferLength;
      }

      #endregion // Constructors


      #region Properties

      /// <summary>When <c>true</c>, the storage adapter supports synchronous transfers as a way of speeding up I/O.</summary>
      public bool AcceleratedTransfer { get; private set; }


      /// <summary>When <c>true</c>, the storage adapter scans down for BIOS devices, that is, the storage adapter begins scanning with the highest device number rather than the lowest.</summary>
      public bool AdapterScansDown { get; private set; }


      /// <summary>When <c>true</c>, the storage adapter uses programmed I/O (PIO).</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Pio")]
      public bool AdapterUsesPio { get; private set; }


      /// <summary>The bus type of the storage adapter.</summary>
      public StorageBusType BusType { get; private set; }


      /// <summary>Represents a description of a device instance as identified by the bus.</summary>
      public string BusReportedDeviceDescription { get; internal set; }

      /// <summary>The version number, if any, of the storage adapter.</summary>
      public Version BusVersion { get; private set; }


      /// <summary>When <c>true</c>, the storage adapter supports SCSI tagged queuing and/or per-logical-unit internal queues, or the non-SCSI equivalent.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Queueing")]
      public bool CommandQueueing { get; private set; }


      /// <summary>The device number connected to this storage adapter, starting at 0.</summary>
      public int DeviceNumber { get; private set; }


      /// <summary>Specifies the maximum number of bytes the storage adapter can transfer in a single operation.</summary>
      public int MaximumTransferBytes { get; private set; }


#if DEBUG
      /// <summary>Specifies the maximum number of bytes the storage adapter can transfer in a single operation, formatted as a unit size.</summary>
      public string MaximumTransferBytesUnitSize
      {
         get { return Utils.UnitSizeToText(MaximumTransferBytes); }
      }
#endif

      #endregion // Properties


      #region Methods

      /// <summary>Returns storage device as: "BusType MaximumTransferBytes".</summary>
      /// <returns>Returns a string that represents this instance.</returns>
      public override string ToString()
      {
         return BusType == StorageBusType.Unknown ? StorageBusType.Unknown.ToString() : string.Format(CultureInfo.CurrentCulture, "{0} {1}", (BusType.ToString() + " " ).Trim(), Utils.UnitSizeToText(MaximumTransferBytes));
      }


      /// <summary>Determines whether the specified Object is equal to the current Object.</summary>
      /// <param name="obj">Another object to compare to.</param>
      /// <returns><c>true</c> if the specified Object is equal to the current Object; otherwise, <c>false</c>.</returns>
      public override bool Equals(object obj)
      {
         if (null == obj || GetType() != obj.GetType())
            return false;

         var other = obj as StorageAdapterInfo;

         return null != other &&
                other.AcceleratedTransfer == AcceleratedTransfer &&
                other.AdapterScansDown == AdapterScansDown &&
                other.AdapterUsesPio == AdapterUsesPio &&
                other.BusReportedDeviceDescription == BusReportedDeviceDescription &&
                other.BusType == BusType &&
                other.BusVersion == BusVersion &&
                other.CommandQueueing == CommandQueueing &&
                other.MaximumTransferBytes == MaximumTransferBytes;
      }


      /// <summary>Serves as a hash function for a particular type.</summary>
      /// <returns>Returns a hash code for the current Object.</returns>
      public override int GetHashCode()
      {
         unchecked
         {
            return MaximumTransferBytes.GetHashCode() + AcceleratedTransfer.GetHashCode() + AcceleratedTransfer.GetHashCode() + CommandQueueing.GetHashCode() + BusType.GetHashCode();
         }
      }


      /// <summary>Implements the operator ==</summary>
      /// <param name="left">A.</param>
      /// <param name="right">B.</param>
      /// <returns>The result of the operator.</returns>
      public static bool operator ==(StorageAdapterInfo left, StorageAdapterInfo right)
      {
         return ReferenceEquals(left, null) && ReferenceEquals(right, null) || !ReferenceEquals(left, null) && !ReferenceEquals(right, null) && left.Equals(right);
      }


      /// <summary>Implements the operator !=</summary>
      /// <param name="left">A.</param>
      /// <param name="right">B.</param>
      /// <returns>The result of the operator.</returns>
      public static bool operator !=(StorageAdapterInfo left, StorageAdapterInfo right)
      {
         return !(left == right);
      }

      #endregion // Methods
   }
}
