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

namespace AlphaFS.UnitTest
{
   /// <summary>Containts static variables, used by unit tests.</summary>
   public static partial class UnitTestConstants
   {
      public static string GetRandomFileNameWithDiacriticCharacters()
      {
         var randomFileName = System.IO.Path.GetRandomFileName();

         switch (new Random().Next(0, 4))
         {
            case 0:
               break;

            case 1:
               randomFileName = randomFileName.Replace("a", "ä");
               randomFileName = randomFileName.Replace("e", "ë");
               randomFileName = randomFileName.Replace("i", "ï");
               randomFileName = randomFileName.Replace("o", "ö");
               randomFileName = randomFileName.Replace("u", "ü");
               break;

            case 2:
               randomFileName = randomFileName.Replace("a", "á");
               randomFileName = randomFileName.Replace("e", "é");
               randomFileName = randomFileName.Replace("i", "í");
               randomFileName = randomFileName.Replace("o", "ó");
               randomFileName = randomFileName.Replace("u", "ú");
               break;

            case 3:
               randomFileName = randomFileName.Replace("a", "â");
               randomFileName = randomFileName.Replace("e", "ê");
               randomFileName = randomFileName.Replace("i", "î");
               randomFileName = randomFileName.Replace("o", "ô");
               randomFileName = randomFileName.Replace("u", "û");
               break;
         }


         return randomFileName;
      }
   }
}
