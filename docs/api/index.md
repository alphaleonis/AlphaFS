# AlphaFS API Reference


@Alphaleonis.Win32

This namespace contains general classes related to the Win32 Api.

@Alphaleonis.Win32.Filesystem

The @Alphaleons.Win32.Filesystem namespace contains classes to access and work with the local filesystem. Many of the classes in this namespace are replicas of the ones available in the System.IO namespace, but with added functionality. All methods in this namespace accept long windows unicode paths (i.e. paths starting with `\\?\`). 

@Alphaleonis.Win32.Network

This namespace contains network related classes.

@Alphaleonis.Win32.Security

This namespace contains classes directly related to security such as authentication, authorization and privilege tokens that may be needed for some file operations. For an example the `SE_SECURITY_NAME` privilege needs to be held to be able to modify the SACL of any file.
