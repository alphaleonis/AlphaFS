/*  Copyright (C) 2008-2016 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
using System.Security.Policy;

namespace Alphaleonis.Win32
{
   internal static class NativeError
   {
      internal static void ThrowException()
      {
         ThrowException((uint)Marshal.GetLastWin32Error(), null, null);
      }

      public static void ThrowException(int errorCode)
      {
         ThrowException((uint)errorCode, null, null);
      }

      public static void ThrowException(int errorCode, string readPath)
      {
         ThrowException((uint)errorCode, readPath, null);
      }

      public static void ThrowException(int errorCode, string readPath, string writePath)
      {
         ThrowException((uint)errorCode, readPath, writePath);
      }

      public static void ThrowException(uint errorCode, string readPath)
      {
         ThrowException(errorCode, readPath, null);
      }

      [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]      
      public static void ThrowException(uint errorCode, string readPath, string writePath)
      {
         string errorMessage = string.Format(CultureInfo.CurrentCulture, "({0}) {1}.", errorCode, new Win32Exception((int)errorCode).Message);

         if (!Utils.IsNullOrWhiteSpace(readPath))
            errorMessage = string.Format(CultureInfo.CurrentCulture, "{0}: [{1}]", errorMessage.TrimEnd('.'), readPath);

         if (!Utils.IsNullOrWhiteSpace(writePath))
            errorMessage = string.Format(CultureInfo.CurrentCulture, "{0}: [{1}]", errorMessage.TrimEnd('.'), writePath);

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

            case Win32Errors.ERROR_ALREADY_EXISTS:
            case Win32Errors.ERROR_FILE_EXISTS:
               throw new AlreadyExistsException(errorMessage);

            case Win32Errors.ERROR_DIR_NOT_EMPTY:
               throw new DirectoryNotEmptyException(errorMessage);

            case Win32Errors.ERROR_NOT_READY:
                  throw new DeviceNotReadyException(errorMessage);


            #region Transactional

            case Win32Errors.ERROR_INVALID_TRANSACTION:
               throw new InvalidTransactionException(Resources.Transaction_Invalid, Marshal.GetExceptionForHR(Win32Errors.GetHrFromWin32Error(errorCode)));

            case Win32Errors.ERROR_TRANSACTION_ALREADY_COMMITTED:
               throw new TransactionAlreadyCommittedException(Resources.Transaction_Already_Committed, Marshal.GetExceptionForHR(Win32Errors.GetHrFromWin32Error(errorCode)));

            case Win32Errors.ERROR_TRANSACTION_ALREADY_ABORTED:
               throw new TransactionAlreadyAbortedException(Resources.Transaction_Already_Aborted, Marshal.GetExceptionForHR(Win32Errors.GetHrFromWin32Error(errorCode)));

            case Win32Errors.ERROR_TRANSACTIONAL_CONFLICT:
               throw new TransactionalConflictException(Resources.Transactional_Conflict, Marshal.GetExceptionForHR(Win32Errors.GetHrFromWin32Error(errorCode)));

            case Win32Errors.ERROR_TRANSACTION_NOT_ACTIVE:
               throw new TransactionException(Resources.Transaction_Not_Active, Marshal.GetExceptionForHR(Win32Errors.GetHrFromWin32Error(errorCode)));

            case Win32Errors.ERROR_TRANSACTION_NOT_REQUESTED:
               throw new TransactionException(Resources.Transaction_Not_Requested, Marshal.GetExceptionForHR(Win32Errors.GetHrFromWin32Error(errorCode)));

            case Win32Errors.ERROR_TRANSACTION_REQUEST_NOT_VALID:
               throw new TransactionException(Resources.Invalid_Transaction_Request, Marshal.GetExceptionForHR(Win32Errors.GetHrFromWin32Error(errorCode)));

            case Win32Errors.ERROR_TRANSACTIONS_UNSUPPORTED_REMOTE:
               throw new UnsupportedRemoteTransactionException(Resources.Invalid_Transaction_Request, Marshal.GetExceptionForHR(Win32Errors.GetHrFromWin32Error(errorCode)));

            case Win32Errors.ERROR_NOT_A_REPARSE_POINT:
               throw new NotAReparsePointException(Resources.Not_A_Reparse_Point, Marshal.GetExceptionForHR(Win32Errors.GetHrFromWin32Error(errorCode)));

            #endregion // Transacted

            case Win32Errors.ERROR_SUCCESS:
            case Win32Errors.ERROR_SUCCESS_REBOOT_INITIATED:
            case Win32Errors.ERROR_SUCCESS_REBOOT_REQUIRED:
            case Win32Errors.ERROR_SUCCESS_RESTART_REQUIRED:
               // We should really never get here, throwing an exception for a successful operation.
               throw new NotImplementedException(string.Format(CultureInfo.CurrentCulture, "{0} {1}", Resources.Exception_From_Successful_Operation, errorMessage));

            default:
               // We don't have a specific exception to generate for this error.               
               throw new IOException(errorMessage, Win32Errors.GetHrFromWin32Error(errorCode));
         }
      }


      public static void ThrowException(string readPath)
      {
         ThrowException((uint)Marshal.GetLastWin32Error(), readPath, null);
      }
      
      public static void ThrowException(string readPath, string writePath)
      {
         ThrowException((uint)Marshal.GetLastWin32Error(), readPath, writePath);
      }
   }
}
