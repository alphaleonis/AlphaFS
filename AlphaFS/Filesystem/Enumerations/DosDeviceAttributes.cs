/* Copyright (c) 2008-2014 Peter Palotas, Jeffrey Jangli, Normalex
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

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>Defines the controllable aspects of the <see cref="T:Volume.DefineDosDevice(string, string, DosDeviceAttributes)"/> method.</summary>
   [Flags]
   public enum DosDeviceAttributes
   {
      /// <summary>(0x00000004) DDD_EXACT_MATCH_ON_REMOVE - Default.</summary>
      None = 0,

      /// <summary>(0x00000001) DDD_RAW_TARGET_PATH
      /// <para>&#160;</para>
      /// <para>Uses the targetPath string as is.</para>
      /// <para>Otherwise, it is converted from an MS-DOS path to a path.</para>
      /// </summary>
      RawTargetPath = 1,

      /// <summary>(0x00000002) DDD_REMOVE_DEFINITION - Removes the specified definition for the specified device.
      /// <para>&#160;</para>
      /// <para>To determine which definition to remove, the function walks the list of mappings for the device,</para>
      /// <para>looking for a match of targetPath against a prefix of each mapping associated with this device.</para>
      /// <para>The first mapping that matches is the one removed, and then the function returns.</para>
      /// <para>If targetPath is null or a pointer to a null string, the function will remove the first mapping</para>
      /// <para> associated with the device and pop the most recent one pushed. If there is nothing left to pop, the device name will be removed.</para>
      /// <para>If this value is not specified, the string pointed to by the targetPath parameter will become the new mapping for this device.</para>
      /// </summary>
      RemoveDefinition = 2,

      /// <summary>(0x00000004) DDD_EXACT_MATCH_ON_REMOVE
      /// <para>&#160;</para>
      /// <para>If this value is specified along with <see cref="T:RemoveDefinition"/>,</para>
      /// <para>the function will use an exact match to determine which mapping to remove.</para>
      /// Use this value to ensure that you do not delete something that you did not define.
      /// </summary>
      ExactMatchOnRemove = 4,

      /// <summary>(0x00000008) DDD_NO_BROADCAST_SYSTEM
      /// <para>&#160;</para>
      /// <para>Do not broadcast the WM_SETTINGCHANGE message.</para>
      /// <para>By default, this message is broadcast to notify the shell and applications of the change.</para>
      /// </summary>
      NoBroadcastSystem = 8,
   }
}