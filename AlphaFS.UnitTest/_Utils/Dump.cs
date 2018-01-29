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
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace AlphaFS.UnitTest
{
   public static partial class UnitTestConstants
   {
      /// <summary>Shows the Object's available Properties and Values.</summary>
      public static bool Dump(object obj, int width = -35, bool indent = false)
      {
         var cnt = 0;
         const string nulll = "\t\tNULL";
         var template = "\t{0}#{1:000}\t{2, " + width + "} = [{3}]";

         if (obj == null)
         {
            Console.WriteLine(nulll);
            return false;
         }

         Console.WriteLine("\n\t{0}Instance: [{1}]\n", indent ? "\t" : "", obj.GetType().FullName);

         var loopOk = false;
         foreach (var descriptor in TypeDescriptor.GetProperties(obj).Sort().Cast<PropertyDescriptor>().Where(descriptor => descriptor != null))
         {
            string propValue = null;
            try
            {
               var value = descriptor.GetValue(obj);

               if (null != value)
               {
                  var propObjType = value.GetType();

                  if (propObjType.IsAssignableToAnyOf(typeof(Stopwatch)))
                  {
                     var propObj = value as Stopwatch;
                     if (null != propObj)
                        propValue = propObj.Elapsed.TotalMilliseconds.ToString(CultureInfo.InvariantCulture);
                  }

                  //else if (propObjType.IsAssignableToAnyOf(typeof(List<>)))
                  //{
                  //}

                  //else if (propObjType == typeof(List<>))
                  //{
                  //   var propObj = value as List<string>;
                  //   if (null != propObj)
                  //      propValue = propObj.Count.ToString(CultureInfo.InvariantCulture);
                  //}

                  else
                     propValue = value.ToString();
               }

               loopOk = true;
            }
            catch (Exception ex)
            {
               // Please do tell, oneliner preferably.
               propValue = ex.Message.Replace(Environment.NewLine, "  ");
            }


            if (null == propValue)
               propValue = "NULL";

            Console.WriteLine(template, indent ? "\t" : "", ++cnt, descriptor.Name, propValue);
         }

         return loopOk;
      }



      public static bool IsAssignableToAnyOf(this Type typeOperand, IEnumerable<Type> types)
      {
         return types.Any(type => type.IsAssignableFrom(typeOperand));
      }


      public static bool IsAssignableToAnyOf(this Type typeOperand, params Type[] types)
      {
         return IsAssignableToAnyOf(typeOperand, types.AsEnumerable());
      }


      public static bool IsAssignableToAnyOf<T1, T2, T3>(this Type typeOperand)
      {
         return typeOperand.IsAssignableToAnyOf(typeof(T1), typeof(T2), typeof(T3));
      }


      public static bool IsAssignableToAnyOf<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(this Type typeOperand)
      {
         return typeOperand.IsAssignableToAnyOf(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14), typeof(T15), typeof(T16), typeof(T17), typeof(T18), typeof(T19), typeof(T20));
      }
   }
}
