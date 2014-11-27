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

using Alphaleonis.Win32.Filesystem;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Policy;

namespace Alphaleonis.Win32
{
   internal static class NativeError
   {
      #region ThrowException

      #region void

      [SecurityCritical]
      internal static void ThrowException()
      {
         ThrowException((uint) Marshal.GetLastWin32Error(), null, null);
      }

      #endregion // void

      #region int

      [SecurityCritical]
      public static void ThrowException(int errorCode, bool isIoException = false)
      {
         ThrowException((uint) errorCode, null, null, isIoException);
      }

      [SecurityCritical]
      public static void ThrowException(int errorCode, string readPath, bool isIoException = false)
      {
         ThrowException((uint) errorCode, readPath, null, isIoException);
      }

      [SecurityCritical]
      public static void ThrowException(int errorCode, string readPath, string writePath, bool isIoException = false)
      {
         ThrowException((uint) errorCode, readPath, writePath, isIoException);
      }

      #endregion // int

      #region uint

      [SecurityCritical]
      public static void ThrowException(uint errorCode, string readPath, bool isIoException = false)
      {
         ThrowException(errorCode, readPath, null, isIoException);
      }

      [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
      [SecurityCritical]
      public static void ThrowException(uint errorCode, string readPath, string writePath, bool isIoException = false)
      {
         string errorMessage = string.Format(CultureInfo.CurrentCulture, "({0}) {1}.", errorCode, new Win32Exception((int) errorCode).Message);

         if (!Utils.IsNullOrWhiteSpace(readPath))
            errorMessage = string.Format(CultureInfo.CurrentCulture, "{0}: [{1}]", errorMessage.TrimEnd('.'), readPath);

         if (!Utils.IsNullOrWhiteSpace(writePath))
            errorMessage = string.Format(CultureInfo.CurrentCulture, "{0}: [{1}]", errorMessage.TrimEnd('.'), writePath);


         if (isIoException)
            throw new IOException(errorMessage, (int) errorCode);

         switch (errorCode)
         {
            case Win32Errors.ERROR_INVALID_DRIVE:
               throw new DriveNotFoundException(errorMessage);

            case Win32Errors.ERROR_OPERATION_ABORTED:
               throw new OperationCanceledException(errorMessage);

            case Win32Errors.ERROR_FILE_NOT_FOUND:
               throw new FileNotFoundException(errorMessage);

            case Win32Errors.ERROR_PATH_NOT_FOUND:
               throw new DirectoryNotFoundException(errorMessage);
               
            case Win32Errors.ERROR_BAD_RECOVERY_POLICY:
               throw new PolicyException(errorMessage);

            case Win32Errors.ERROR_FILE_READ_ONLY:
            case Win32Errors.ERROR_ACCESS_DENIED:
            case Win32Errors.ERROR_NETWORK_ACCESS_DENIED:
               throw new UnauthorizedAccessException(errorMessage);

            #region Transacted

            case Win32Errors.ERROR_INVALID_TRANSACTION:
               throw new InvalidTransactionException(Resources.InvalidTransaction, Marshal.GetExceptionForHR(Win32Errors.GetHrFromWin32Error(errorCode)));

            case Win32Errors.ERROR_TRANSACTION_ALREADY_COMMITTED:
               throw new TransactionAlreadyCommittedException(Resources.TransactionAlreadyCommitted, Marshal.GetExceptionForHR(Win32Errors.GetHrFromWin32Error(errorCode)));

            case Win32Errors.ERROR_TRANSACTION_ALREADY_ABORTED:
               throw new TransactionAlreadyAbortedException(Resources.TransactionAlreadyAborted, Marshal.GetExceptionForHR(Win32Errors.GetHrFromWin32Error(errorCode)));

            case Win32Errors.ERROR_TRANSACTIONAL_CONFLICT:
               throw new TransactionalConflictException(Resources.TransactionalConflict, Marshal.GetExceptionForHR(Win32Errors.GetHrFromWin32Error(errorCode)));

            case Win32Errors.ERROR_TRANSACTION_NOT_ACTIVE:
               throw new TransactionException(Resources.TransactionNotActive, Marshal.GetExceptionForHR(Win32Errors.GetHrFromWin32Error(errorCode)));

            case Win32Errors.ERROR_TRANSACTION_NOT_REQUESTED:
               throw new TransactionException(Resources.TransactionNotRequested, Marshal.GetExceptionForHR(Win32Errors.GetHrFromWin32Error(errorCode)));

            case Win32Errors.ERROR_TRANSACTION_REQUEST_NOT_VALID:
               throw new TransactionException(Resources.InvalidTransactionRequest, Marshal.GetExceptionForHR(Win32Errors.GetHrFromWin32Error(errorCode)));

            case Win32Errors.ERROR_TRANSACTIONS_UNSUPPORTED_REMOTE:
               throw new UnsupportedRemoteTransactionException(Resources.InvalidTransactionRequest, Marshal.GetExceptionForHR(Win32Errors.GetHrFromWin32Error(errorCode)));

            case Win32Errors.ERROR_NOT_A_REPARSE_POINT:
               throw new NotAReparsePointException(Resources.NotAReparsePoint, Marshal.GetExceptionForHR(Win32Errors.GetHrFromWin32Error(errorCode)));

               #endregion // Transacted

            case Win32Errors.ERROR_SUCCESS:
            case Win32Errors.ERROR_SUCCESS_REBOOT_INITIATED:
            case Win32Errors.ERROR_SUCCESS_REBOOT_REQUIRED:
            case Win32Errors.ERROR_SUCCESS_RESTART_REQUIRED:
               // We should really never get here, throwing an exception for a successful operation.
               throw new NotImplementedException(string.Format(CultureInfo.CurrentCulture, "{0} {1}", Resources.AttemptingToGenerateExceptionFromSuccessfulOperation, errorMessage));

            default:
               // We don't have a specific exception to generate for this error.
               throw new IOException(errorMessage, (int) errorCode);
         }
      }

      #endregion // uint

      #region string

      [SecurityCritical]
      public static void ThrowException(string readPath, bool isIoException = false)
      {
         ThrowException((uint) Marshal.GetLastWin32Error(), readPath, null, isIoException);
      }

      [SecurityCritical]
      public static void ThrowException(string readPath, string writePath, bool isIoException = false)
      {
         ThrowException((uint) Marshal.GetLastWin32Error(), readPath, writePath, isIoException);
      }

      #endregion // string

      #endregion // ThrowException
   }
}