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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Security.AccessControl;

namespace AlphaFS.UnitTest
{
   /// <summary>Containts static variables, used by unit tests.</summary>
   public static partial class UnitTestConstants
   {
      public static void TestAccessRules(ObjectSecurity sysIO, ObjectSecurity alphaFS)
      {
         Console.WriteLine();


         Console.WriteLine("\tSystem.IO .AccessRightType: [{0}]", sysIO.AccessRightType);
         Console.WriteLine("\tAlphaFS   .AccessRightType: [{0}]", alphaFS.AccessRightType);
         Console.WriteLine();
         Assert.AreEqual(sysIO.AccessRightType, alphaFS.AccessRightType);


         Console.WriteLine("\tSystem.IO .AccessRuleType: [{0}]", sysIO.AccessRuleType);
         Console.WriteLine("\tAlphaFS   .AccessRuleType: [{0}]", alphaFS.AccessRuleType);
         Console.WriteLine();
         Assert.AreEqual(sysIO.AccessRuleType, alphaFS.AccessRuleType);


         Console.WriteLine("\tSystem.IO .AuditRuleType: [{0}]", sysIO.AuditRuleType);
         Console.WriteLine("\tAlphaFS   .AuditRuleType: [{0}]", alphaFS.AuditRuleType);
         Console.WriteLine();
         Assert.AreEqual(sysIO.AuditRuleType, alphaFS.AuditRuleType);




         Console.WriteLine("\tSystem.IO .AreAccessRulesProtected: [{0}]", sysIO.AreAccessRulesProtected);
         Console.WriteLine("\tAlphaFS   .AreAccessRulesProtected: [{0}]", alphaFS.AreAccessRulesProtected);
         Console.WriteLine();
         Assert.AreEqual(sysIO.AreAccessRulesProtected, alphaFS.AreAccessRulesProtected);


         Console.WriteLine("\tSystem.IO .AreAuditRulesProtected: [{0}]", sysIO.AreAuditRulesProtected);
         Console.WriteLine("\tAlphaFS   .AreAuditRulesProtected: [{0}]", alphaFS.AreAuditRulesProtected);
         Console.WriteLine();
         Assert.AreEqual(sysIO.AreAuditRulesProtected, alphaFS.AreAuditRulesProtected);


         Console.WriteLine("\tSystem.IO .AreAccessRulesCanonical: [{0}]", sysIO.AreAccessRulesCanonical);
         Console.WriteLine("\tAlphaFS   .AreAccessRulesCanonical: [{0}]", alphaFS.AreAccessRulesCanonical);
         Console.WriteLine();
         Assert.AreEqual(sysIO.AreAccessRulesCanonical, alphaFS.AreAccessRulesCanonical);


         Console.WriteLine("\tSystem.IO .AreAuditRulesCanonical: [{0}]", sysIO.AreAuditRulesCanonical);
         Console.WriteLine("\tAlphaFS   .AreAuditRulesCanonical: [{0}]", alphaFS.AreAuditRulesCanonical);
         Console.WriteLine();
         Assert.AreEqual(sysIO.AreAuditRulesCanonical, alphaFS.AreAuditRulesCanonical);
      }
   }
}
