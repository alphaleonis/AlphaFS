# AlphaFS

AlphaFS is a .NET library providing more complete Win32 file system functionality to the .NET platform than the standard System.IO classes.

## Introduction

The file system support in .NET is pretty good for most uses. However there are a few shortcomings, which this library tries to alleviate. The most notable deficiency of the standard .NET System.IO is the lack of support of advanced NTFS features, most notably extended length path support (eg. file/directory paths longer than 260 characters).

### Feature Highlights

* Support for extended length paths (longer than 260 characters)
* Creating Hardlinks
* Accessing hidden volumes
* Enumeration of volumes
* Transactional file operations
* Support for NTFS Alternate Data Streams
* ...and much more. 

## What does AlphaFS provide?

AlphaFS provides a namespace (Alphaleonis.Win32.Filesystem) containing a number of classes. Most notable
are replications of the System.IO.File, System.IO.Directory and System.IO.Path, all with support for the
extended-length paths (up to 32000 chars), recursive file enumerations, native backups and manipulations with 
advanced flags and options. They also contain extensions to these, and there are many more features 
for several functions.

Another thing AlphaFS brings to the table is support for transactional NTFS (TxF). Almost every method in
these classes exist in two versions. One normal, and one that can work with transactions, more specifically the
kernel transaction manager. This means that file operations can be performed using the simple, lightweight KTM 
on NTFS file systems, through .NET, using the interface of the standard classes we are all used to.

AlphaFS also contains some NTFS security related functionality (in Alphaleonis.Win32.Security), providing 
the ability to enable token privileges for a user, which may be necessary for eg. changing ownership of a file.

The library is Open Source, licensed under the MIT license.
