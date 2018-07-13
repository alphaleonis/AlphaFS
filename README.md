# AlphaFS

[![Codacy Badge](https://api.codacy.com/project/badge/Grade/9b3af141aae74656b6135413564b7e5d)](https://app.codacy.com/app/Yomodo/AlphaFS?utm_source=github.com&utm_medium=referral&utm_content=alphaleonis/AlphaFS&utm_campaign=badger)

AlphaFS is a .NET library providing more complete Win32 file system functionality to the .NET platform than the standard `System.IO` classes.

## Introduction

The file system support in .NET is pretty good for most uses. However there are a few shortcomings, which this library tries to alleviate. The most notable deficiency of the standard .NET `System.IO` is the lack of support of advanced NTFS features, most notably extended length path support (eg. file/directory paths longer than 260 characters: `System.IO.PathTooLongException`).

### Feature Highlights

* Support for extended length paths (longer than 260 characters)
* Creating Junctions/Hardlinks
* Enumeration of folders and files with exception reporting and recovery, custom filtering.
* Enumeration of volumes
* Accessing hidden volumes
* Transactional file operations
* Support for NTFS Alternate Data Streams on files and folders
* Accessing SMB and DFS network resources
* Create and access folders/files that contain space(s) in their name.
* ...and much more!

## What does AlphaFS provide?

AlphaFS provides a namespace (`Alphaleonis.Win32.Filesystem`) containing a number of classes. Most notable
are replications of the `System.IO.Path`, `System.IO.File`, `System.IO.FileInfo`, `System.IO.Directory` and `System.IO.DirectoryInfo`, all with support for the extended-length paths (up to 32.000 chars), full UNC support,
recursive file enumerations, native backups and manipulations with advanced flags and options.
They also contain extensions to these, and there are many more features for several functions.

When only  these `System.IO` classes are used, it is just a matter of replacing `using System.IO`
with `using Alphaleonis.Win32.Filesystem`, which makes AlphaFS a **true** drop-in replacement.

The ability to report and recover from, usually access denied, exceptions while enumerating files and folders is one of the
shortcomings that AlphaFS tries to fix, together with the ability of custom filtering to get only the files and folders you need.

Another thing AlphaFS brings to the table is support for transactional NTFS (TxF). Almost every method in
these classes exist in two versions. One normal, and one that can work with transactions, more specifically the
kernel transaction manager. This means that file operations can be performed using the simple, lightweight KTM 
on NTFS file systems, through .NET, using the interface of the standard classes we are all used to.

AlphaFS also contains some NTFS security related functionality (in `Alphaleonis.Win32.Security`), providing 
the ability to enable token privileges for a user, which may be necessary for eg. changing ownership of a file.

The `Alphaleonis.Win32.Network` namespace together with the `Alphaleonis.Win32.Network.Host` class offers
network functionality to connect to SMB/DFS resources and easily access files and folders on the network,
all with extended-length paths support.

The library is Open Source, licensed under the MIT license.
