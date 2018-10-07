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
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace AlphaFS.UnitTest
{
   public static partial class UnitTestConstants
   {
      /// <summary>Shows the Object's available Properties and Values.</summary>
      public static void Dump(object obj, bool indent = false)
      {
         if (null == obj)
         {
            Console.WriteLine("\n\t\tNothing to dump because the instance is null.");
            return;
         }


         var allProperties = TypeDescriptor.GetProperties(obj).Sort().Cast<PropertyDescriptor>().Where(descriptor => null != descriptor).ToArray();


         // Determine widest property name, for layout.
         var width = allProperties.Select(prop => prop.Name.Length).Concat(new[] {0}).Max();

         var count = 0;
         var template = "\t{0}#{1:000}\t{2, " + -width + "} = [{3}]";

         Console.WriteLine("\n\t{0}Instance: [{1}]\n", indent ? "\t" : "", obj.GetType().FullName);


         foreach (var descriptor in allProperties)
            try
            {
               Console.WriteLine(template, indent ? "\t" : string.Empty, ++count, descriptor.Name, Write(descriptor.GetValue(obj)) ?? "NULL");
            }
            catch (Exception ex)
            {
               Console.WriteLine(template, indent ? "\t" : string.Empty, ++count, descriptor.Name, ex.Message.Replace(Environment.NewLine, "  "));
            }
      }


      private static string Write(object value)
      {
         if (null == value)
            return null;

         if (value is string)
            return value as string;


         long number;
         if (long.TryParse(value.ToString(), out number))
            return number.ToString("N0", CultureInfo.CurrentCulture);


         var objectType = value as IEnumerable;

         if (null == objectType)
            return value.ToString();


         var sb = new StringBuilder();

         foreach (var objectValue in objectType)
            sb.Append(objectValue + ", ");

         return sb.ToString().TrimEnd(',', ' ');
      }
   }
}
