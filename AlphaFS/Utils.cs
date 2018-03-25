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
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Alphaleonis
{
   internal static class Utils
   {
      /// <summary>Gets an attribute on an enum field value.</summary>
      /// <returns>The description belonging to the enum option, as a string</returns>
      /// <param name="enumValue">One of the <see cref="Alphaleonis.Win32.Filesystem.DeviceGuid"/> enum types.</param>
      [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
      public static string GetEnumDescription(Enum enumValue)
      {
         var enumValueString = enumValue.ToString();

         var fi = enumValue.GetType().GetField(enumValueString);

         var attributes = (DescriptionAttribute[]) fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

         return attributes.Length > 0 ? attributes[0].Description : enumValueString;
      }


      /// <summary>Checks that the object is not null.</summary>
      public static bool IsNotNull<T>(T obj)
      {
         return !Equals(null, obj);
      }


      /// <summary>Indicates whether a specified string is null, empty, or consists only of white-space characters.</summary>
      /// <returns><see langword="true"/> if the <paramref name="value"/> parameter is null or <see cref="string.Empty"/>, or if <paramref name="value"/> consists exclusively of white-space characters.</returns>
      /// <param name="value">The string to test.</param>
      public static bool IsNullOrWhiteSpace(string value)
      {
#if NET35
         if (null != value)
         {
            for (int index = 0, l = value.Length; index < l; ++index)
               if (!char.IsWhiteSpace(value[index]))
                  return false;
         }

         return true;
#else
         return string.IsNullOrWhiteSpace(value);
#endif
      }


      /// <summary>Converts a number of type T to string formated using <see cref="CultureInfo.InvariantCulture"/>, suffixed with a unit size.</summary>
      public static string UnitSizeToText<T>(T numberOfBytes)
      {
         // CultureInfo.CurrentCulture uses the culture as set in the Region applet.

         return UnitSizeToText(numberOfBytes, CultureInfo.CurrentCulture);
      }


      /// <summary>Converts a number of type T to string formated using the specified <paramref name="cultureInfo"/>, suffixed with a unit size.</summary>
      public static string UnitSizeToText<T>(T numberOfBytes, CultureInfo cultureInfo)
      {
         var sizeFormats = new[] {"B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"};
         const int kb = 1024;
         var index = 0;

         var bytes = Convert.ToDouble(numberOfBytes, CultureInfo.InvariantCulture);

         if (bytes < 0)
            bytes = 0;
         
         else
            while (bytes > kb)
            {
               bytes /= kb;
               index++;
            }


         // Will return "512 B" instead of "512,00 B".

         return string.Format(cultureInfo, "{0} {1}", bytes.ToString(index == 0 ? "0" : "0.##", cultureInfo), sizeFormats[index]);
      }


      public static int CombineHashCodesOf<T1, T2>(T1 arg1, T2 arg2)
      {
         unchecked
         {
            var hash = 17;

            hash = hash * 23 + (null != arg1 ? arg1.GetHashCode() : 0);
            hash = hash * 23 + (null != arg2 ? arg2.GetHashCode() : 0);

            return hash;
         }
      }


      public static int CombineHashCodesOf<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3)
      {
         unchecked
         {
            var hash = CombineHashCodesOf(arg1, arg2);

            hash = hash * 23 + (null != arg3 ? arg3.GetHashCode() : 0);

            return hash;
         }
      }


      public static int CombineHashCodesOf<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
      {
         return CombineHashCodesOf(CombineHashCodesOf(arg1, arg2), CombineHashCodesOf(arg3, arg4));
      }
   }
}
