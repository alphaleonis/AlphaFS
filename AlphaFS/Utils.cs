/* Copyright 2008-2015 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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

using Alphaleonis.Win32.Filesystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Alphaleonis
{
   internal static class Utils
   {
      #region EnumMemberToList

      public static IEnumerable<T> EnumMemberToList<T>()
      {
         Type enumType = typeof(T);

         // Can't use generic type constraints on value types, so have to do check like this.
         if (enumType.BaseType != typeof(Enum))
            throw new ArgumentException("T must be of type System.Enum");

         //Array enumValArray = Enum.GetValues(enumType);
         //List<T> enumValList = new List<T>(enumValArray.Length);
         IOrderedEnumerable<T> enumValArray = Enum.GetValues(enumType).Cast<T>().OrderBy(e => e.ToString());
         List<T> enumValList = new List<T>(enumValArray.Count());

         enumValList.AddRange(enumValArray.Select(val => (T)Enum.Parse(enumType, val.ToString())));
         return enumValList;
      }

      #endregion // EnumMemberToList

      #region GetEnumDescription

      /// <summary>Gets an attribute on an enum field value.</summary>
      /// <param name="enumValue">One of the <see cref="Alphaleonis.Win32.Filesystem.DeviceGuid"/> enum types.</param>
      /// <returns></returns>
      [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
      public static string GetEnumDescription(Enum enumValue)
      {
         FieldInfo fi = enumValue.GetType().GetField(enumValue.ToString());
         DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
         return attributes.Length > 0 ? attributes[0].Description : enumValue.ToString();
      }

      #endregion // GetEnumDescription

      #region MarshalPtrToStructure

      public static T MarshalPtrToStructure<T>(int offset, IntPtr buffer) where T : struct
      {
         var structure = new T();
         return (T) Marshal.PtrToStructure(new IntPtr(buffer.ToInt64() + offset*Marshal.SizeOf(structure)), typeof (T));
      }

      #endregion // MarshalPtrToStructure

      #region IsNullOrWhiteSpace

      /// <summary>Indicates whether a specified string is null, empty, or consists only of white-space characters.</summary>
      /// <returns><see langword="true"/> if the <paramref name="value"/> parameter is null or <see cref="String.Empty"/>, or if <paramref name="value"/> consists exclusively of white-space characters.</returns>
      /// <param name="value">The string to test.</param>
      public static bool IsNullOrWhiteSpace(string value)
      {
#if NET35
         if (value != null)
         {
            for (int index = 0; index < value.Length; ++index)
            {
               if (!char.IsWhiteSpace(value[index]))
                  return false;
            }
         }

         return true;
#else
         return string.IsNullOrWhiteSpace(value);
#endif
      }

      #endregion // IsNullOrWhiteSpace
      
      #region UnitSizeToText

      /// <summary>Converts a number of type T to string with UnitSize or Percentage suffixed.</summary>
      public static string UnitSizeToText<T>(T numberOfBytes, bool usePercent = false)
      {
         string template = "{0:0.00}{1}";
         string sfx = "B";

         double bytes = Convert.ToDouble(numberOfBytes, CultureInfo.InvariantCulture);

         if (bytes >= 1152921504606846976) { sfx = "EB"; bytes /= 1152921504606846976; }
         else if (bytes >= 1125899906842624) { sfx = "PB"; bytes /= 1125899906842624; }
         else if (bytes >= 1099511627776) { sfx = "TB"; bytes /= 1099511627776; }
         else if (bytes >= 1073741824) { sfx = "GB"; bytes /= 1073741824; }
         else if (bytes >= 1048576) { sfx = "MB"; bytes /= 1048576; }
         else if (bytes >= 1024) { sfx = "KB"; bytes /= 1024; }

         else if (!usePercent)
            // Will return "512 B" instead of "512,00 B".
            template = "{0:0}{1}";

         return string.Format(CultureInfo.CurrentCulture, template, bytes, usePercent ? "%" : " " + sfx);
      }

      /// <summary>Calculates a percentage value.</summary>
      /// <param name="currentValue"/>
      /// <param name="minimumValue"/>
      /// <param name="maximumValue"/>
      public static double PercentCalculate(double currentValue, double minimumValue, double maximumValue)
      {
         return (currentValue < 0 || maximumValue <= 0) ? 0 : currentValue * 100 / (maximumValue - minimumValue);
      }

      #endregion // UnitSizeToText
   }
}