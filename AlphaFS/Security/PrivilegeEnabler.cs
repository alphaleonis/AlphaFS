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

namespace Alphaleonis.Win32.Security
{
    /// <summary>
    /// Used to enable one or more privileges. The privileges specified will be enabled during the 
    /// lifetime of the instance. Users create an instance of this object in a <c>using</c> statement
    /// to ensure that it is properly disposed when the elevated privileges are no longer needed.
    /// </summary>
    public sealed class PrivilegeEnabler : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrivilegeEnabler"/> class. This will enable the
        /// privileges specified (unless already enabled), and ensure that they are disabled again when
        /// the object is disposed. (Any privileges already enabled will not be disabled).
        /// </summary>
        /// <param name="privilege">The privilege to enable.</param>
        /// <param name="privileges">Additional privileges to enable.</param>
        public PrivilegeEnabler(Privilege privilege, params Privilege[] privileges)
        {
            mEnabledPrivileges.Add(new InternalPrivilegeEnabler(privilege));
            if (null != privileges)
            {
                foreach (Privilege priv in privileges)
                    mEnabledPrivileges.Add(new InternalPrivilegeEnabler(priv));
            }
        }

        /// <summary>
        /// Makes sure any privileges enabled by this instance are disabled.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public void Dispose()
        {            
            for (int i = 0; i < mEnabledPrivileges.Count; i++)
            {
                try
                {
                    mEnabledPrivileges[i].Dispose();
                }
                catch (Exception)
                {
                    // We ignore any exceptions here
                }
            }
        }

        /// <summary>
        /// Gets the enabled privileges. Note that this might not contain all privileges specified
        /// to the constructor. Only the privileges actually enabled by this instance is returned.
        /// </summary>
        /// <value>The enabled privileges.</value>
        public IEnumerable<Privilege> EnabledPrivileges
        {
            get
            {
                foreach (InternalPrivilegeEnabler priv in mEnabledPrivileges)
                {
                    if (priv.EnabledPrivilege != null)
                        yield return priv.EnabledPrivilege;
                }
            }
        }
       
        private List<InternalPrivilegeEnabler> mEnabledPrivileges = new List<InternalPrivilegeEnabler>();
    }
}
