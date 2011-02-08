/* Copyright (c) 2008-2009 Peter Palotas
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
// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project. 
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc. 
//
// To add a suppression to this file, right-click the message in the 
// Error List, point to "Suppress Message(s)", and click 
// "In Project Suppression File". 
// You do not need to add suppressions to this file manually. 

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Filesystem", Scope = "namespace", Target = "Alphaleonis.Win32.Filesystem")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Alphaleonis", Scope = "namespace", Target = "Alphaleonis.Win32.Filesystem")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Scope = "member", Target = "Alphaleonis.Win32.Filesystem.PathInfo+Parser.#.cctor()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dfs", Scope = "member", Target = "Alphaleonis.Win32.Filesystem.ReparsePointTag.#Dfs")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dfsr", Scope = "member", Target = "Alphaleonis.Win32.Filesystem.ReparsePointTag.#Dfsr")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Hsm", Scope = "member", Target = "Alphaleonis.Win32.Filesystem.ReparsePointTag.#Hsm")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Hsm", Scope = "member", Target = "Alphaleonis.Win32.Filesystem.ReparsePointTag.#Hsm2")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Unc", Scope = "member", Target = "Alphaleonis.Win32.Filesystem.Path.#UncPrefix")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Unc", Scope = "member", Target = "Alphaleonis.Win32.Filesystem.Path.#LongPathUncPrefix")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest", Scope = "member", Target = "Alphaleonis.Win32.Filesystem.FileInfo.#CopyTo(System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest", Scope = "member", Target = "Alphaleonis.Win32.Filesystem.FileInfo.#CopyTo(System.String,System.Boolean)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest", Scope = "member", Target = "Alphaleonis.Win32.Filesystem.FileInfo.#MoveTo(System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA2101:SpecifyMarshalingForPInvokeStringArguments", MessageId = "FileSystemEntryInfo.cFileName", Scope = "member", Target = "Alphaleonis.Win32.Filesystem.NativeMethods.#FindNextFileW(Alphaleonis.Win32.Filesystem.SafeFindFileHandle,Alphaleonis.Win32.Filesystem.FileSystemEntryInfo)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA2101:SpecifyMarshalingForPInvokeStringArguments", MessageId = "FileSystemEntryInfo.cAlternateFileName", Scope = "member", Target = "Alphaleonis.Win32.Filesystem.NativeMethods.#FindNextFileW(Alphaleonis.Win32.Filesystem.SafeFindFileHandle,Alphaleonis.Win32.Filesystem.FileSystemEntryInfo)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA2101:SpecifyMarshalingForPInvokeStringArguments", MessageId = "FileSystemEntryInfo.cFileName", Scope = "member", Target = "Alphaleonis.Win32.Filesystem.NativeMethods.#FindFirstFileTransactedW(System.String,Alphaleonis.Win32.Filesystem.NativeMethods+FINDEX_INFO_LEVELS,Alphaleonis.Win32.Filesystem.FileSystemEntryInfo,Alphaleonis.Win32.Filesystem.NativeMethods+FINDEX_SEARCH_OPS,System.IntPtr,Alphaleonis.Win32.Filesystem.NativeMethods+FINDEX_FLAGS,System.Runtime.InteropServices.SafeHandle)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA2101:SpecifyMarshalingForPInvokeStringArguments", MessageId = "FileSystemEntryInfo.cAlternateFileName", Scope = "member", Target = "Alphaleonis.Win32.Filesystem.NativeMethods.#FindFirstFileTransactedW(System.String,Alphaleonis.Win32.Filesystem.NativeMethods+FINDEX_INFO_LEVELS,Alphaleonis.Win32.Filesystem.FileSystemEntryInfo,Alphaleonis.Win32.Filesystem.NativeMethods+FINDEX_SEARCH_OPS,System.IntPtr,Alphaleonis.Win32.Filesystem.NativeMethods+FINDEX_FLAGS,System.Runtime.InteropServices.SafeHandle)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA2101:SpecifyMarshalingForPInvokeStringArguments", MessageId = "FileSystemEntryInfo.cFileName", Scope = "member", Target = "Alphaleonis.Win32.Filesystem.NativeMethods.#FindFirstFileExW(System.String,Alphaleonis.Win32.Filesystem.NativeMethods+FINDEX_INFO_LEVELS,Alphaleonis.Win32.Filesystem.FileSystemEntryInfo,Alphaleonis.Win32.Filesystem.NativeMethods+FINDEX_SEARCH_OPS,System.IntPtr,Alphaleonis.Win32.Filesystem.NativeMethods+FINDEX_FLAGS)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA2101:SpecifyMarshalingForPInvokeStringArguments", MessageId = "FileSystemEntryInfo.cAlternateFileName", Scope = "member", Target = "Alphaleonis.Win32.Filesystem.NativeMethods.#FindFirstFileExW(System.String,Alphaleonis.Win32.Filesystem.NativeMethods+FINDEX_INFO_LEVELS,Alphaleonis.Win32.Filesystem.FileSystemEntryInfo,Alphaleonis.Win32.Filesystem.NativeMethods+FINDEX_SEARCH_OPS,System.IntPtr,Alphaleonis.Win32.Filesystem.NativeMethods+FINDEX_FLAGS)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Scope = "member", Target = "Alphaleonis.Win32.Filesystem.PathInfo+Parser.#MatchServerNameAndShare()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Unc", Scope = "member", Target = "Alphaleonis.Win32.Filesystem.Path.#GetMappedUncName(System.String)")]
