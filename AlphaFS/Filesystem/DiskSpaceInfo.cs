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
using System.Runtime.InteropServices;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>Retrieves information about the amount of space that is available on a disk volume, which is the total amount of space,
   /// the total amount of free space, and the total amount of free space available to the user that is associated with the calling thread.
   /// <para>This class cannot be inherited.</para>
   /// </summary>
   [Serializable]
   [SecurityCritical]
   public sealed class DiskSpaceInfo
   {
      #region Private Fields

      [NonSerialized] private CultureInfo _cultureInfo;
      [NonSerialized] private readonly bool _initGetClusterInfo = true;
      [NonSerialized] private readonly bool _initGetSpaceInfo = true;
      [NonSerialized] private readonly bool _continueOnAccessError;

      #endregion // Private Fields


      #region Constructors

      /// <summary>Initializes a DiskSpaceInfo instance.</summary>
      /// <Remark>This is a Lazyloading object; call <see cref="Refresh()"/> to populate all properties first before accessing.</Remark>
      /// <param name="driveName">A valid drive path or drive letter. This can be either uppercase or lowercase, 'a' to 'z' or a network share in the format: \\server\share</param>
      [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Utils.IsNullOrWhiteSpace validates arguments.")]
      [SecurityCritical]
      public DiskSpaceInfo(string driveName)
      {
         if (Utils.IsNullOrWhiteSpace(driveName))
            throw new ArgumentNullException("driveName");


         driveName = driveName.Length == 1 ? driveName + Path.VolumeSeparatorChar : Path.GetPathRoot(driveName, false);

         if (Utils.IsNullOrWhiteSpace(driveName))
            throw new ArgumentException(Resources.InvalidDriveLetterArgument, "driveName");


         _cultureInfo = CultureInfo.CurrentCulture;


         // MSDN:
         // If this parameter is a UNC name, it must include a trailing backslash (for example, "\\MyServer\MyShare\").
         // Furthermore, a drive specification must have a trailing backslash (for example, <c>C:\</c>).
         // The calling application must have FILE_LIST_DIRECTORY access rights for this directory.
         DriveName = Path.AddTrailingDirectorySeparator(driveName, false);
      }


      /// <summary>Initializes a DiskSpaceInfo instance.</summary>
      /// <param name="driveName">A valid drive path or drive letter. This can be either uppercase or lowercase, 'a' to 'z' or a network share in the format: \\server\share</param>
      /// <param name="spaceInfoType"><c>null</c> gets both size- and disk cluster information. <c>true</c> Get only disk cluster information, <c>false</c> Get only size information.</param>
      /// <param name="refresh">Refreshes the state of the object.</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown as a result from a failure, such as unavailable resources.</param>
      [SecurityCritical]
      public DiskSpaceInfo(string driveName, bool? spaceInfoType, bool refresh, bool continueOnException) : this(driveName)
      {
         if (null == spaceInfoType)
         {
            _initGetSpaceInfo = true;
            _initGetClusterInfo = true;
         }

         else
         {
            _initGetSpaceInfo = (bool) !spaceInfoType;
            _initGetClusterInfo = (bool) spaceInfoType;
         }

         _continueOnAccessError = continueOnException;

         if (refresh)
            Refresh();
      }

      #endregion // Constructors


      /// <summary>Indicates the amount of available free space on a drive, formatted as percentage.</summary>
      public string AvailableFreeSpacePercent
      {
         get
         {
            return PercentCalculate(TotalNumberOfBytes - (TotalNumberOfBytes - TotalNumberOfFreeBytes), 0, TotalNumberOfBytes).ToString("0.##", _cultureInfo) + "%";
         }
      }


      /// <summary>Indicates the amount of available free space on a drive, formatted as a unit size.</summary>
      public string AvailableFreeSpaceUnitSize
      {
         get { return Utils.UnitSizeToText(TotalNumberOfFreeBytes, _cultureInfo); }
      }


      /// <summary>Returns the Clusters size.</summary>
      public long ClusterSize
      {
         get { return SectorsPerCluster * BytesPerSector; }
      }


      /// <summary>Gets the name of a drive.</summary>
      /// <remarks>This property is the name assigned to the drive, such as: <c>C:\</c> or  <c>D:\</c></remarks>
      public string DriveName { get; private set; }


      /// <summary>The total number of bytes on a disk that are available to the user who is associated with the calling thread, formatted as a unit size.</summary>
      public string TotalSizeUnitSize
      {
         get { return Utils.UnitSizeToText(TotalNumberOfBytes, _cultureInfo); }
      }


      /// <summary>Indicates the amount of used space on a drive, formatted as percentage.</summary>
      public string UsedSpacePercent
      {
         get
         {
            return PercentCalculate(TotalNumberOfBytes - FreeBytesAvailable, 0, TotalNumberOfBytes).ToString("0.##", _cultureInfo) + "%";
         }
      }


      /// <summary>Indicates the amount of used space on a drive, formatted as a unit size.</summary>
      public string UsedSpaceUnitSize
      {
         get { return Utils.UnitSizeToText(TotalNumberOfBytes - FreeBytesAvailable, _cultureInfo); }
      }


      /// <summary>The total number of free bytes on a disk that are available to the user who is associated with the calling thread.</summary>
      public long FreeBytesAvailable { get; private set; }


      /// <summary>The total number of bytes on a disk that are available to the user who is associated with the calling thread.</summary>
      public long TotalNumberOfBytes { get; private set; }


      /// <summary>The total number of free bytes on a disk.</summary>
      public long TotalNumberOfFreeBytes { get; private set; }


      /// <summary>The number of bytes per sector.</summary>
      public int BytesPerSector { get; private set; }


      /// <summary>The total number of free clusters on the disk that are available to the user who is associated with the calling thread.</summary>
      public int NumberOfFreeClusters { get; private set; }


      /// <summary>The number of sectors per cluster.</summary>
      public int SectorsPerCluster { get; private set; }


      /// <summary>The total number of clusters on the disk that are available to the user who is associated with the calling thread.
      /// If per-user disk quotas are in use, this value may be less than the total number of clusters on the disk.
      /// </summary>
      public long TotalNumberOfClusters { get; private set; }




      /// <summary>Refreshes the state of the object.</summary>
      public void Refresh()
      {
         Reset();

         _cultureInfo = CultureInfo.CurrentCulture;


         using (new NativeMethods.ChangeErrorMode(NativeMethods.ErrorMode.FailCriticalErrors))
         {
            int lastError;


            // Get size information.

            if (_initGetSpaceInfo)
            {
               long freeBytesAvailable, totalNumberOfBytes, totalNumberOfFreeBytes;

               var success = Device.NativeMethods.GetDiskFreeSpaceEx(DriveName, out freeBytesAvailable, out totalNumberOfBytes, out totalNumberOfFreeBytes);

               lastError = Marshal.GetLastWin32Error();

               if (!success && !_continueOnAccessError && lastError != Win32Errors.ERROR_NOT_READY)
                  NativeError.ThrowException(lastError, DriveName);


               FreeBytesAvailable = freeBytesAvailable;
               TotalNumberOfBytes = totalNumberOfBytes;
               TotalNumberOfFreeBytes = totalNumberOfFreeBytes;
            }


            // Get cluster information.

            if (_initGetClusterInfo)
            {
               int sectorsPerCluster, bytesPerSector, numberOfFreeClusters;
               uint totalNumberOfClusters;

               var success = Device.NativeMethods.GetDiskFreeSpace(DriveName, out sectorsPerCluster, out bytesPerSector, out numberOfFreeClusters, out totalNumberOfClusters);

               lastError = Marshal.GetLastWin32Error();

               if (!success && !_continueOnAccessError && lastError != Win32Errors.ERROR_NOT_READY)
                  NativeError.ThrowException(lastError, DriveName);


               BytesPerSector = bytesPerSector;
               NumberOfFreeClusters = numberOfFreeClusters;
               SectorsPerCluster = sectorsPerCluster;
               TotalNumberOfClusters = totalNumberOfClusters;
            }
         }
      }


      /// <summary>Initializes all <see ref="Alphaleonis.Win32.Filesystem.DiskSpaceInfo"/> properties to 0.</summary>
      private void Reset()
      {
         if (_initGetSpaceInfo)
         {
            FreeBytesAvailable = 0;
            TotalNumberOfBytes = 0;
            TotalNumberOfFreeBytes = 0;
         }


         if (_initGetClusterInfo)
         {
            BytesPerSector = 0;
            NumberOfFreeClusters = 0;
            SectorsPerCluster = 0;
            TotalNumberOfClusters = 0;
         }
      }


      /// <summary>Returns the drive name.</summary>
      /// <returns>Returns a string that represents this object.</returns>
      public override string ToString()
      {
         return DriveName;
      }


      /// <summary>Calculates a percentage value.</summary>
      private static double PercentCalculate(double currentValue, double minimumValue, double maximumValue)
      {
         return currentValue < 0 || maximumValue <= 0 ? 0 : currentValue * 100 / (maximumValue - minimumValue);
      }
   }
}
