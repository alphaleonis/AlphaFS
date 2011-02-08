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
using System;
using System.Collections.Generic;
using System.Text;

namespace Alphaleonis.Win32.Filesystem
{
    /// <summary>
    /// The type of the data contained in the backup stream.
    /// </summary>
    public enum BackupStreamType
    {
        /// <summary>
        /// This indicates an error.
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// Standard data
        /// </summary>
        Data = 1,
        /// <summary>
        /// Extended attribute data
        /// </summary>
        ExtendedAttributesData = 2,
        /// <summary>
        /// Security descriptor data
        /// </summary>
        SecurityData = 3,
        /// <summary>
        /// Alternative data streams
        /// </summary>
        AlternateData = 4,
        /// <summary>
        /// Hard Link Information
        /// </summary>
        Link = 5,
        /// <summary>
        /// Property data
        /// </summary>
        PropertyData = 6,
        /// <summary>
        /// Object identifiers
        /// </summary>
        ObjectId = 7,
        /// <summary>
        /// Reparse points
        /// </summary>
        ReparseData = 8,
        /// <summary>
        /// Sparse file
        /// </summary>
        SparseBlock = 9
    }
}
