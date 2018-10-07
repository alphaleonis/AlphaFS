# Differences compared to System.IO

**[[TODO: Split in several pages]]**

When applicable, the path result of AlphaFS *integration tests* are compared against
`System.IO` to ensure the highest compatibility with .NET.

Of course, AlphaFS should match with .NET as much as possible,
although in some cases it will not. This is due to .NET not being able
to handle the *long path format* in a consistent way.

For example: `System.IO.Path.GetPathRoot()`

    Input Path: [\\SERVER001\Share]
    System.IO : [\\SERVER001\Share]
    AlphaFS   : [\\SERVER001\Share]

    # Use UNC long path format.
    Input Path: [\\?\UNC\SERVER001\Share]
    System.IO : [\\?\UNC]
    AlphaFS   : [\\?\UNC\SERVER001\Share]

For example: `System.IO.Directory.GetDirectoryRoot()`

    Input Path: [\\?\C:\]
    Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
    System.IO : [null]
    AlphaFS   : [C:\]

    # Use UNC long path format.
    Input Path: [\\?\UNC\SERVER001\Share\folder2]
    Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
    System.IO : [null]
    AlphaFS   : [\\SERVER001\Share]



Below are methods where AlphaFS will return different results compared to .NET:
(This list has not yet been completed)
**[[TODO: Complete this list]]**

[[Directory.GetDirectoryRoot()|Path Retrieval - Directory.GetDirectoryRoot()]]

[[Path.GetFullPath()|Path Retrieval - Path.GetFullPath()]]
[[Path.GetPathRoot()|Path Retrieval - Path.GetPathRoot()]]

## System.IO.Directory.Exists()

`System.IO.Directory.Exists()` internally throws a `System.ArgumentException: Illegal characters in path` when encountering the long path notation: `\\?\` and returns `False`, even if the folder exists. AlphaFS returns `True` when the folder exists. This also applies to `System.IO.Path.GetFullPath()`


## System.IO.Path.GetFullPath()

Input Path: `\\?\GLOBALROOT\device\harddisk0\partition1\`

System.IO: `Caught [System.IO] System.ArgumentException: [Paths that begin with \\?\GlobalRoot are internal to the kernel and should not be opened by managed applications.`

AlphaFS: Returns Input Path: `\\?\GLOBALROOT\device\harddisk0\partition1\`

## Path Retrieval Directory.GetDirectoryRoot()

**Comparison AlphaFS vs System.IO**

    #001	Input Path: [.]
            System.IO : [C:\]
            AlphaFS   : [C:\]

    #002	Input Path: [.zip]
            System.IO : [C:\]
            AlphaFS   : [C:\]

    #003	Input Path: [C:\\test.txt]
            System.IO : [C:\]
            AlphaFS   : [C:\]

    #004	Input Path: [C:\/test.txt]
            System.IO : [C:\]
            AlphaFS   : [C:\]

    #005	Input Path: [\]
            System.IO : [C:\]
            AlphaFS   : [C:\]

    #006	Input Path: [\Program Files\Microsoft Office]
            System.IO : [C:\]
            AlphaFS   : [C:\]

    #007	Input Path: [\\?\GLOBALROOT\device\harddisk0\partition1\]
            Caught [System.IO] System.ArgumentException: [Paths that begin with \\?\GlobalRoot are internal to the kernel and should not be opened by managed applications.]
            System.IO : [null]
            AlphaFS   : [null]

    #008	Input Path: [\\?\Volume{XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX}\Program Files\notepad.exe]
            Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
            System.IO : [null]
            AlphaFS   : [null]

    #009	Input Path: [Program Files\Microsoft Office]
            System.IO : [C:\]
            AlphaFS   : [C:\]

    #010	Input Path: [C]
            System.IO : [C:\]
            AlphaFS   : [C:\]

    #011	Input Path: [C:]
            System.IO : [C:\]
            AlphaFS   : [C:\]

    #012	Input Path: [C:\]
            System.IO : [C:\]
            AlphaFS   : [C:\]

    #013	Input Path: [C:\a]
            System.IO : [C:\]
            AlphaFS   : [C:\]

    #014	Input Path: [C:\a\]
            System.IO : [C:\]
            AlphaFS   : [C:\]

    #015	Input Path: [C:\a\b]
            System.IO : [C:\]
            AlphaFS   : [C:\]

    #016	Input Path: [C:\a\b\]
            System.IO : [C:\]
            AlphaFS   : [C:\]

    #017	Input Path: [C:\a\b\c]
            System.IO : [C:\]
            AlphaFS   : [C:\]

    #018	Input Path: [C:\a\b\c\]
            System.IO : [C:\]
            AlphaFS   : [C:\]

    #019	Input Path: [C:\a\b\c\f]
            System.IO : [C:\]
            AlphaFS   : [C:\]

    #020	Input Path: [C:\a\b\c\f.]
            System.IO : [C:\]
            AlphaFS   : [C:\]

    #021	Input Path: [C:\a\b\c\f.t]
            System.IO : [C:\]
            AlphaFS   : [C:\]

    #022	Input Path: [C:\a\b\c\f.tx]
            System.IO : [C:\]
            AlphaFS   : [C:\]

    #023	Input Path: [C:\a\b\c\f.txt]
            System.IO : [C:\]
            AlphaFS   : [C:\]

    #024	Input Path: [\\?\Program Files\Microsoft Office]
            Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
            System.IO : [null]
            AlphaFS   : [null]

    #025	Input Path: [\\?\C]
            Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
            System.IO : [null]
            AlphaFS   : [null]

    #026	Input Path: [\\?\C:]
            Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
            System.IO : [null]
            AlphaFS   : [C:]

    #027	Input Path: [\\?\C:\]
            Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
            System.IO : [null]
            AlphaFS   : [C:\]

    #028	Input Path: [\\?\C:\a]
            Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
            System.IO : [null]
            AlphaFS   : [C:\]

    #029	Input Path: [\\?\C:\a\]
            Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
            System.IO : [null]
            AlphaFS   : [C:\]

    #030	Input Path: [\\?\C:\a\b]
            Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
            System.IO : [null]
            AlphaFS   : [C:\]

    #031	Input Path: [\\?\C:\a\b\]
            Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
            System.IO : [null]
            AlphaFS   : [C:\]

    #032	Input Path: [\\?\C:\a\b\c]
            Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
            System.IO : [null]
            AlphaFS   : [C:\]

    #033	Input Path: [\\?\C:\a\b\c\]
            Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
            System.IO : [null]
            AlphaFS   : [C:\]

    #034	Input Path: [\\?\C:\a\b\c\f]
            Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
            System.IO : [null]
            AlphaFS   : [C:\]

    #035	Input Path: [\\?\C:\a\b\c\f.]
            Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
            System.IO : [null]
            AlphaFS   : [C:\]

    #036	Input Path: [\\?\C:\a\b\c\f.t]
            Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
            System.IO : [null]
            AlphaFS   : [C:\]

    #037	Input Path: [\\?\C:\a\b\c\f.tx]
            Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
            System.IO : [null]
            AlphaFS   : [C:\]

    #038	Input Path: [\\?\C:\a\b\c\f.txt]
            Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
            System.IO : [null]
            AlphaFS   : [C:\]

    #039	Input Path: [\\SERVER001\Share]
            System.IO : [\\SERVER001\Share]
            AlphaFS   : [\\SERVER001\Share]

    #040	Input Path: [\\SERVER001\Share\]
            System.IO : [\\SERVER001\Share]
            AlphaFS   : [\\SERVER001\Share]

    #041	Input Path: [\\SERVER001\Share\d]
            System.IO : [\\SERVER001\Share]
            AlphaFS   : [\\SERVER001\Share]

    #042	Input Path: [\\SERVER001\Share\d1]
            System.IO : [\\SERVER001\Share]
            AlphaFS   : [\\SERVER001\Share]

    #043	Input Path: [\\SERVER001\Share\d1\]
            System.IO : [\\SERVER001\Share]
            AlphaFS   : [\\SERVER001\Share]

    #044	Input Path: [\\SERVER001\Share\d1\d]
            System.IO : [\\SERVER001\Share]
            AlphaFS   : [\\SERVER001\Share]

    #045	Input Path: [\\SERVER001\Share\d1\d2]
            System.IO : [\\SERVER001\Share]
            AlphaFS   : [\\SERVER001\Share]

    #046	Input Path: [\\SERVER001\Share\d1\d2\]
            System.IO : [\\SERVER001\Share]
            AlphaFS   : [\\SERVER001\Share]

    #047	Input Path: [\\SERVER001\Share\d1\d2\f]
            System.IO : [\\SERVER001\Share]
            AlphaFS   : [\\SERVER001\Share]

    #048	Input Path: [\\SERVER001\Share\d1\d2\fi]
            System.IO : [\\SERVER001\Share]
            AlphaFS   : [\\SERVER001\Share]

    #049	Input Path: [\\SERVER001\Share\d1\d2\fil]
            System.IO : [\\SERVER001\Share]
            AlphaFS   : [\\SERVER001\Share]

    #050	Input Path: [\\SERVER001\Share\d1\d2\file]
            System.IO : [\\SERVER001\Share]
            AlphaFS   : [\\SERVER001\Share]

    #051	Input Path: [\\SERVER001\Share\d1\d2\file.]
            System.IO : [\\SERVER001\Share]
            AlphaFS   : [\\SERVER001\Share]

    #052	Input Path: [\\SERVER001\Share\d1\d2\file.e]
            System.IO : [\\SERVER001\Share]
            AlphaFS   : [\\SERVER001\Share]

    #053	Input Path: [\\SERVER001\Share\d1\d2\file.ex]
            System.IO : [\\SERVER001\Share]
            AlphaFS   : [\\SERVER001\Share]

    #054	Input Path: [\\SERVER001\Share\d1\d2\file.ext]
            System.IO : [\\SERVER001\Share]
            AlphaFS   : [\\SERVER001\Share]

    #055	Input Path: [\\?\UNC\SERVER001\Share]
            Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
            System.IO : [null]
            AlphaFS   : [\\SERVER001\Share]

    #056	Input Path: [\\?\UNC\SERVER001\Share\]
            Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
            System.IO : [null]
            AlphaFS   : [\\SERVER001\Share]

    #057	Input Path: [\\?\UNC\SERVER001\Share\d]
            Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
            System.IO : [null]
            AlphaFS   : [\\SERVER001\Share]

    #058	Input Path: [\\?\UNC\SERVER001\Share\d1]
            Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
            System.IO : [null]
            AlphaFS   : [\\SERVER001\Share]

    #059	Input Path: [\\?\UNC\SERVER001\Share\d1\]
            Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
            System.IO : [null]
            AlphaFS   : [\\SERVER001\Share]

    #060	Input Path: [\\?\UNC\SERVER001\Share\d1\d]
            Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
            System.IO : [null]
            AlphaFS   : [\\SERVER001\Share]

    #061	Input Path: [\\?\UNC\SERVER001\Share\d1\d2]
            Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
            System.IO : [null]
            AlphaFS   : [\\SERVER001\Share]

    #062	Input Path: [\\?\UNC\SERVER001\Share\d1\d2\]
            Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
            System.IO : [null]
            AlphaFS   : [\\SERVER001\Share]

    #063	Input Path: [\\?\UNC\SERVER001\Share\d1\d2\f]
            Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
            System.IO : [null]
            AlphaFS   : [\\SERVER001\Share]

    #064	Input Path: [\\?\UNC\SERVER001\Share\d1\d2\fi]
            Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
            System.IO : [null]
            AlphaFS   : [\\SERVER001\Share]

    #065	Input Path: [\\?\UNC\SERVER001\Share\d1\d2\fil]
            Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
            System.IO : [null]
            AlphaFS   : [\\SERVER001\Share]

    #066	Input Path: [\\?\UNC\SERVER001\Share\d1\d2\file]
            Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
            System.IO : [null]
            AlphaFS   : [\\SERVER001\Share]

    #067	Input Path: [\\?\UNC\SERVER001\Share\d1\d2\file.]
            Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
            System.IO : [null]
            AlphaFS   : [\\SERVER001\Share]

    #068	Input Path: [\\?\UNC\SERVER001\Share\d1\d2\file.e]
            Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
            System.IO : [null]
            AlphaFS   : [\\SERVER001\Share]

    #069	Input Path: [\\?\UNC\SERVER001\Share\d1\d2\file.ex]
            Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
            System.IO : [null]
            AlphaFS   : [\\SERVER001\Share]

    #070	Input Path: [\\?\UNC\SERVER001\Share\d1\d2\file.ext]
            Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
            System.IO : [null]
            AlphaFS   : [\\SERVER001\Share]

            *Duration: [19] ms. (00:00:00.0197729)


## Path Retrieval Path.GetFullPath()

**Comparison AlphaFS vs System.IO**

    #001 Input Path: [.]
         System.IO : [C:\Users\jjangli\Documents\_DEV\AlphaFS.git\bin\Debug\Net35]
         AlphaFS   : [C:\Users\jjangli\Documents\_DEV\AlphaFS.git\bin\Debug\Net35]

    #002 Input Path: [.zip]
         System.IO : [C:\Users\jjangli\Documents\_DEV\AlphaFS.git\bin\Debug\Net35\.zip]
         AlphaFS   : [C:\Users\jjangli\Documents\_DEV\AlphaFS.git\bin\Debug\Net35\.zip]

    #003 Input Path: [C:\\test.txt]
         System.IO : [C:\test.txt]
         AlphaFS   : [C:\test.txt]

    #004 Input Path: [C:\/test.txt]
         System.IO : [C:\test.txt]
         AlphaFS   : [C:\test.txt]

    #005 Input Path: [\]
         System.IO : [C:\]
         AlphaFS   : [C:\]

    #006 Input Path: [\Program Files\Microsoft Office]
         System.IO : [C:\Program Files\Microsoft Office]
         AlphaFS   : [C:\Program Files\Microsoft Office]

    #007 Input Path: [\\?\GLOBALROOT\device\harddisk0\partition1\]
         Caught [System.IO] System.ArgumentException: [Paths that begin with \\?\GlobalRoot are internal to the kernel and should not be opened by managed applications.]
         System.IO : [null]
         AlphaFS   : [\\?\GLOBALROOT\device\harddisk0\partition1\]

    #008 Input Path: [\\?\Volume{XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX}\Program Files\notepad.exe]
         Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
         System.IO : [null]
         AlphaFS   : [\\?\Volume{XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX}\Program Files\notepad.exe]

    #009 Input Path: [dir1/dir2/dir3/]
         System.IO : [C:\Users\jjangli\Documents\_DEV\AlphaFS.git\bin\Debug\Net35\dir1\dir2\dir3\]
         AlphaFS   : [C:\Users\jjangli\Documents\_DEV\AlphaFS.git\bin\Debug\Net35\dir1\dir2\dir3\]

    #010 Input Path: [Program Files\Microsoft Office]
         System.IO : [C:\Users\jjangli\Documents\_DEV\AlphaFS.git\bin\Debug\Net35\Program Files\Microsoft Office]
         AlphaFS   : [C:\Users\jjangli\Documents\_DEV\AlphaFS.git\bin\Debug\Net35\Program Files\Microsoft Office]

    #011 Input Path: [C]
         System.IO : [C:\Users\jjangli\Documents\_DEV\AlphaFS.git\bin\Debug\Net35\C]
         AlphaFS   : [C:\Users\jjangli\Documents\_DEV\AlphaFS.git\bin\Debug\Net35\C]

    #012 Input Path: [C:]
         System.IO : [C:\Users\jjangli\Documents\_DEV\AlphaFS.git\bin\Debug\Net35]
         AlphaFS   : [C:\Users\jjangli\Documents\_DEV\AlphaFS.git\bin\Debug\Net35]

    #013 Input Path: [C:\]
         System.IO : [C:\]
         AlphaFS   : [C:\]

    #014 Input Path: [C:\a]
         System.IO : [C:\a]
         AlphaFS   : [C:\a]

    #015 Input Path: [C:\a\]
         System.IO : [C:\a\]
         AlphaFS   : [C:\a\]

    #016 Input Path: [C:\a\b]
         System.IO : [C:\a\b]
         AlphaFS   : [C:\a\b]

    #017 Input Path: [C:\a\b\]
         System.IO : [C:\a\b\]
         AlphaFS   : [C:\a\b\]

    #018 Input Path: [C:\a\b\c]
         System.IO : [C:\a\b\c]
         AlphaFS   : [C:\a\b\c]

    #019 Input Path: [C:\a\b\c\]
         System.IO : [C:\a\b\c\]
         AlphaFS   : [C:\a\b\c\]

    #020 Input Path: [C:\a\b\c\f]
         System.IO : [C:\a\b\c\f]
         AlphaFS   : [C:\a\b\c\f]

    #021 Input Path: [C:\a\b\c\f.]
         System.IO : [C:\a\b\c\f]
         AlphaFS   : [C:\a\b\c\f]

    #022 Input Path: [C:\a\b\c\f.t]
         System.IO : [C:\a\b\c\f.t]
         AlphaFS   : [C:\a\b\c\f.t]

    #023 Input Path: [C:\a\b\c\f.tx]
         System.IO : [C:\a\b\c\f.tx]
         AlphaFS   : [C:\a\b\c\f.tx]

    #024 Input Path: [C:\a\b\c\f.txt]
         System.IO : [C:\a\b\c\f.txt]
         AlphaFS   : [C:\a\b\c\f.txt]

    #025 Input Path: [\\?\Program Files\Microsoft Office]
         Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
         System.IO : [null]
         AlphaFS   : [Program Files\Microsoft Office]

    #026 Input Path: [\\?\C]
         Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
         System.IO : [null]
         AlphaFS   : [C]

    #027 Input Path: [\\?\C:]
         Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
         System.IO : [null]
         AlphaFS   : [C:]

    #028 Input Path: [\\?\C:\]
         Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
         System.IO : [null]
         AlphaFS   : [C:\]

    #029 Input Path: [\\?\C:\a]
         Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
         System.IO : [null]
         AlphaFS   : [C:\a]

    #030 Input Path: [\\?\C:\a\]
         Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
         System.IO : [null]
         AlphaFS   : [C:\a\]

    #031 Input Path: [\\?\C:\a\b]
         Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
         System.IO : [null]
         AlphaFS   : [C:\a\b]

    #032 Input Path: [\\?\C:\a\b\]
         Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
         System.IO : [null]
         AlphaFS   : [C:\a\b\]

    #033 Input Path: [\\?\C:\a\b\c]
         Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
         System.IO : [null]
         AlphaFS   : [C:\a\b\c]

    #034 Input Path: [\\?\C:\a\b\c\]
         Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
         System.IO : [null]
         AlphaFS   : [C:\a\b\c\]

    #035 Input Path: [\\?\C:\a\b\c\f]
         Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
         System.IO : [null]
         AlphaFS   : [C:\a\b\c\f]

    #036 Input Path: [\\?\C:\a\b\c\f.]
         Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
         System.IO : [null]
         AlphaFS   : [C:\a\b\c\f]

    #037 Input Path: [\\?\C:\a\b\c\f.t]
         Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
         System.IO : [null]
         AlphaFS   : [C:\a\b\c\f.t]

    #038 Input Path: [\\?\C:\a\b\c\f.tx]
         Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
         System.IO : [null]
         AlphaFS   : [C:\a\b\c\f.tx]

    #039 Input Path: [\\?\C:\a\b\c\f.txt]
         Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
         System.IO : [null]
         AlphaFS   : [C:\a\b\c\f.txt]

    #040 Input Path: [\\NL-DZ42KX1\Share]
         System.IO : [\\NL-DZ42KX1\Share]
         AlphaFS   : [\\NL-DZ42KX1\Share]

    #041 Input Path: [\\NL-DZ42KX1\Share\]
         System.IO : [\\NL-DZ42KX1\Share\]
         AlphaFS   : [\\NL-DZ42KX1\Share\]

    #042 Input Path: [\\NL-DZ42KX1\Share\d]
         System.IO : [\\NL-DZ42KX1\Share\d]
         AlphaFS   : [\\NL-DZ42KX1\Share\d]

    #043 Input Path: [\\NL-DZ42KX1\Share\d1]
         System.IO : [\\NL-DZ42KX1\Share\d1]
         AlphaFS   : [\\NL-DZ42KX1\Share\d1]

    #044 Input Path: [\\NL-DZ42KX1\Share\d1\]
         System.IO : [\\NL-DZ42KX1\Share\d1\]
         AlphaFS   : [\\NL-DZ42KX1\Share\d1\]

    #045 Input Path: [\\NL-DZ42KX1\Share\d1\d]
         System.IO : [\\NL-DZ42KX1\Share\d1\d]
         AlphaFS   : [\\NL-DZ42KX1\Share\d1\d]

    #046 Input Path: [\\NL-DZ42KX1\Share\d1\d2]
         System.IO : [\\NL-DZ42KX1\Share\d1\d2]
         AlphaFS   : [\\NL-DZ42KX1\Share\d1\d2]

    #047 Input Path: [\\NL-DZ42KX1\Share\d1\d2\]
         System.IO : [\\NL-DZ42KX1\Share\d1\d2\]
         AlphaFS   : [\\NL-DZ42KX1\Share\d1\d2\]

    #048 Input Path: [\\NL-DZ42KX1\Share\d1\d2\f]
         System.IO : [\\NL-DZ42KX1\Share\d1\d2\f]
         AlphaFS   : [\\NL-DZ42KX1\Share\d1\d2\f]

    #049 Input Path: [\\NL-DZ42KX1\Share\d1\d2\fi]
         System.IO : [\\NL-DZ42KX1\Share\d1\d2\fi]
         AlphaFS   : [\\NL-DZ42KX1\Share\d1\d2\fi]

    #050 Input Path: [\\NL-DZ42KX1\Share\d1\d2\fil]
         System.IO : [\\NL-DZ42KX1\Share\d1\d2\fil]
         AlphaFS   : [\\NL-DZ42KX1\Share\d1\d2\fil]

    #051 Input Path: [\\NL-DZ42KX1\Share\d1\d2\file]
         System.IO : [\\NL-DZ42KX1\Share\d1\d2\file]
         AlphaFS   : [\\NL-DZ42KX1\Share\d1\d2\file]

    #052 Input Path: [\\NL-DZ42KX1\Share\d1\d2\file.]
         System.IO : [\\NL-DZ42KX1\Share\d1\d2\file]
         AlphaFS   : [\\NL-DZ42KX1\Share\d1\d2\file]

    #053 Input Path: [\\NL-DZ42KX1\Share\d1\d2\file.e]
         System.IO : [\\NL-DZ42KX1\Share\d1\d2\file.e]
         AlphaFS   : [\\NL-DZ42KX1\Share\d1\d2\file.e]

    #054 Input Path: [\\NL-DZ42KX1\Share\d1\d2\file.ex]
         System.IO : [\\NL-DZ42KX1\Share\d1\d2\file.ex]
         AlphaFS   : [\\NL-DZ42KX1\Share\d1\d2\file.ex]

    #055 Input Path: [\\NL-DZ42KX1\Share\d1\d2\file.ext]
         System.IO : [\\NL-DZ42KX1\Share\d1\d2\file.ext]
         AlphaFS   : [\\NL-DZ42KX1\Share\d1\d2\file.ext]

    #056 Input Path: [\\?\UNC\NL-DZ42KX1\Share]
         Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
         System.IO : [null]
         AlphaFS   : [\\NL-DZ42KX1\Share]

    #057 Input Path: [\\?\UNC\NL-DZ42KX1\Share\]
         Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
         System.IO : [null]
         AlphaFS   : [\\NL-DZ42KX1\Share\]

    #058 Input Path: [\\?\UNC\NL-DZ42KX1\Share\d]
         Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
         System.IO : [null]
         AlphaFS   : [\\NL-DZ42KX1\Share\d]

    #059 Input Path: [\\?\UNC\NL-DZ42KX1\Share\d1]
         Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
         System.IO : [null]
         AlphaFS   : [\\NL-DZ42KX1\Share\d1]

    #060 Input Path: [\\?\UNC\NL-DZ42KX1\Share\d1\]
         Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
         System.IO : [null]
         AlphaFS   : [\\NL-DZ42KX1\Share\d1\]

    #061 Input Path: [\\?\UNC\NL-DZ42KX1\Share\d1\d]
         Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
         System.IO : [null]
         AlphaFS   : [\\NL-DZ42KX1\Share\d1\d]

    #062 Input Path: [\\?\UNC\NL-DZ42KX1\Share\d1\d2]
         Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
         System.IO : [null]
         AlphaFS   : [\\NL-DZ42KX1\Share\d1\d2]

    #063 Input Path: [\\?\UNC\NL-DZ42KX1\Share\d1\d2\]
         Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
         System.IO : [null]
         AlphaFS   : [\\NL-DZ42KX1\Share\d1\d2\]

    #064 Input Path: [\\?\UNC\NL-DZ42KX1\Share\d1\d2\f]
         Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
         System.IO : [null]
         AlphaFS   : [\\NL-DZ42KX1\Share\d1\d2\f]

    #065 Input Path: [\\?\UNC\NL-DZ42KX1\Share\d1\d2\fi]
         Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
         System.IO : [null]
         AlphaFS   : [\\NL-DZ42KX1\Share\d1\d2\fi]

    #066 Input Path: [\\?\UNC\NL-DZ42KX1\Share\d1\d2\fil]
         Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
         System.IO : [null]
         AlphaFS   : [\\NL-DZ42KX1\Share\d1\d2\fil]

    #067 Input Path: [\\?\UNC\NL-DZ42KX1\Share\d1\d2\file]
         Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
         System.IO : [null]
         AlphaFS   : [\\NL-DZ42KX1\Share\d1\d2\file]

    #068 Input Path: [\\?\UNC\NL-DZ42KX1\Share\d1\d2\file.]
         Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
         System.IO : [null]
         AlphaFS   : [\\NL-DZ42KX1\Share\d1\d2\file]

    #069 Input Path: [\\?\UNC\NL-DZ42KX1\Share\d1\d2\file.e]
         Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
         System.IO : [null]
         AlphaFS   : [\\NL-DZ42KX1\Share\d1\d2\file.e]

    #070 Input Path: [\\?\UNC\NL-DZ42KX1\Share\d1\d2\file.ex]
         Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
         System.IO : [null]
         AlphaFS   : [\\NL-DZ42KX1\Share\d1\d2\file.ex]

    #071 Input Path: [\\?\UNC\NL-DZ42KX1\Share\d1\d2\file.ext]
         Caught [System.IO] System.ArgumentException: [Illegal characters in path.]
         System.IO : [null]
         AlphaFS   : [\\NL-DZ42KX1\Share\d1\d2\file.ext]

## Path Retrieval Path.GetPathRoot()

**Comparison AlphaFS vs System.IO**

    #001	Input Path: [.]
            System.IO : []
            AlphaFS   : []

    #002	Input Path: [.zip]
            System.IO : []
            AlphaFS   : []

    #003	Input Path: [C:\\test.txt]
            System.IO : [C:\]
            AlphaFS   : [C:\]

    #004	Input Path: [C:\/test.txt]
            System.IO : [C:\]
            AlphaFS   : [C:\]

    #005	Input Path: [\]
            System.IO : [\]
            AlphaFS   : [\]

    #006	Input Path: [\Program Files\Microsoft Office]
            System.IO : [\]
            AlphaFS   : [\]

    #007	Input Path: [\\?\GLOBALROOT\device\harddisk0\partition1\]
            Caught [System.IO] System.ArgumentException: [Paths that begin with \\?\GlobalRoot are internal to the kernel and should not be opened by managed applications.]
            System.IO : [null]
            AlphaFS   : [\\?\GLOBALROOT]

    #008	Input Path: [\\?\Volume{XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX}\Program Files\notepad.exe]
            System.IO : [\\?\Volume{XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX}]
            AlphaFS   : [\\?\Volume{XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX}]

    #009	Input Path: [Program Files\Microsoft Office]
            System.IO : []
            AlphaFS   : []

    #010	Input Path: [C]
            System.IO : []
            AlphaFS   : []

    #011	Input Path: [C:]
            System.IO : [C:]
            AlphaFS   : [C:]

    #012	Input Path: [C:\]
            System.IO : [C:\]
            AlphaFS   : [C:\]

    #013	Input Path: [C:\a]
            System.IO : [C:\]
            AlphaFS   : [C:\]

    #014	Input Path: [C:\a\]
            System.IO : [C:\]
            AlphaFS   : [C:\]

    #015	Input Path: [C:\a\b]
            System.IO : [C:\]
            AlphaFS   : [C:\]

    #016	Input Path: [C:\a\b\]
            System.IO : [C:\]
            AlphaFS   : [C:\]

    #017	Input Path: [C:\a\b\c]
            System.IO : [C:\]
            AlphaFS   : [C:\]

    #018	Input Path: [C:\a\b\c\]
            System.IO : [C:\]
            AlphaFS   : [C:\]

    #019	Input Path: [C:\a\b\c\f]
            System.IO : [C:\]
            AlphaFS   : [C:\]

    #020	Input Path: [C:\a\b\c\f.]
            System.IO : [C:\]
            AlphaFS   : [C:\]

    #021	Input Path: [C:\a\b\c\f.t]
            System.IO : [C:\]
            AlphaFS   : [C:\]

    #022	Input Path: [C:\a\b\c\f.tx]
            System.IO : [C:\]
            AlphaFS   : [C:\]

    #023	Input Path: [C:\a\b\c\f.txt]
            System.IO : [C:\]
            AlphaFS   : [C:\]

    #024	Input Path: [\\?\Program Files\Microsoft Office]
            System.IO : [\\?\Program Files]
            AlphaFS   : [\\?\Program Files]

    #025	Input Path: [\\?\C]
            System.IO : [\\?\C]
            AlphaFS   : [\\?\C]

    #026	Input Path: [\\?\C:]
            System.IO : [\\?\C:]
            AlphaFS   : [\\?\C:]

    #027	Input Path: [\\?\C:\]
            System.IO : [\\?\C:]
            AlphaFS   : [\\?\C:]

    #028	Input Path: [\\?\C:\a]
            System.IO : [\\?\C:]
            AlphaFS   : [\\?\C:]

    #029	Input Path: [\\?\C:\a\]
            System.IO : [\\?\C:]
            AlphaFS   : [\\?\C:]

    #030	Input Path: [\\?\C:\a\b]
            System.IO : [\\?\C:]
            AlphaFS   : [\\?\C:]

    #031	Input Path: [\\?\C:\a\b\]
            System.IO : [\\?\C:]
            AlphaFS   : [\\?\C:]

    #032	Input Path: [\\?\C:\a\b\c]
            System.IO : [\\?\C:]
            AlphaFS   : [\\?\C:]

    #033	Input Path: [\\?\C:\a\b\c\]
            System.IO : [\\?\C:]
            AlphaFS   : [\\?\C:]

    #034	Input Path: [\\?\C:\a\b\c\f]
            System.IO : [\\?\C:]
            AlphaFS   : [\\?\C:]

    #035	Input Path: [\\?\C:\a\b\c\f.]
            System.IO : [\\?\C:]
            AlphaFS   : [\\?\C:]

    #036	Input Path: [\\?\C:\a\b\c\f.t]
            System.IO : [\\?\C:]
            AlphaFS   : [\\?\C:]

    #037	Input Path: [\\?\C:\a\b\c\f.tx]
            System.IO : [\\?\C:]
            AlphaFS   : [\\?\C:]

    #038	Input Path: [\\?\C:\a\b\c\f.txt]
            System.IO : [\\?\C:]
            AlphaFS   : [\\?\C:]

    #039	Input Path: [\\SERVER001\Share]
            System.IO : [\\SERVER001\Share]
            AlphaFS   : [\\SERVER001\Share]

    #040	Input Path: [\\SERVER001\Share\]
            System.IO : [\\SERVER001\Share]
            AlphaFS   : [\\SERVER001\Share]

    #041	Input Path: [\\SERVER001\Share\d]
            System.IO : [\\SERVER001\Share]
            AlphaFS   : [\\SERVER001\Share]

    #042	Input Path: [\\SERVER001\Share\d1]
            System.IO : [\\SERVER001\Share]
            AlphaFS   : [\\SERVER001\Share]

    #043	Input Path: [\\SERVER001\Share\d1\]
            System.IO : [\\SERVER001\Share]
            AlphaFS   : [\\SERVER001\Share]

    #044	Input Path: [\\SERVER001\Share\d1\d]
            System.IO : [\\SERVER001\Share]
            AlphaFS   : [\\SERVER001\Share]

    #045	Input Path: [\\SERVER001\Share\d1\d2]
            System.IO : [\\SERVER001\Share]
            AlphaFS   : [\\SERVER001\Share]

    #046	Input Path: [\\SERVER001\Share\d1\d2\]
            System.IO : [\\SERVER001\Share]
            AlphaFS   : [\\SERVER001\Share]

    #047	Input Path: [\\SERVER001\Share\d1\d2\f]
            System.IO : [\\SERVER001\Share]
            AlphaFS   : [\\SERVER001\Share]

    #048	Input Path: [\\SERVER001\Share\d1\d2\fi]
            System.IO : [\\SERVER001\Share]
            AlphaFS   : [\\SERVER001\Share]

    #049	Input Path: [\\SERVER001\Share\d1\d2\fil]
            System.IO : [\\SERVER001\Share]
            AlphaFS   : [\\SERVER001\Share]

    #050	Input Path: [\\SERVER001\Share\d1\d2\file]
            System.IO : [\\SERVER001\Share]
            AlphaFS   : [\\SERVER001\Share]

    #051	Input Path: [\\SERVER001\Share\d1\d2\file.]
            System.IO : [\\SERVER001\Share]
            AlphaFS   : [\\SERVER001\Share]

    #052	Input Path: [\\SERVER001\Share\d1\d2\file.e]
            System.IO : [\\SERVER001\Share]
            AlphaFS   : [\\SERVER001\Share]

    #053	Input Path: [\\SERVER001\Share\d1\d2\file.ex]
            System.IO : [\\SERVER001\Share]
            AlphaFS   : [\\SERVER001\Share]

    #054	Input Path: [\\SERVER001\Share\d1\d2\file.ext]
            System.IO : [\\SERVER001\Share]
            AlphaFS   : [\\SERVER001\Share]

    #055	Input Path: [\\?\UNC\SERVER001\Share]
            System.IO : [\\?\UNC]
            AlphaFS   : [\\?\UNC\SERVER001\Share]

    #056	Input Path: [\\?\UNC\SERVER001\Share\]
            System.IO : [\\?\UNC]
            AlphaFS   : [\\?\UNC\SERVER001\Share]

    #057	Input Path: [\\?\UNC\SERVER001\Share\d]
            System.IO : [\\?\UNC]
            AlphaFS   : [\\?\UNC\SERVER001\Share]

    #058	Input Path: [\\?\UNC\SERVER001\Share\d1]
            System.IO : [\\?\UNC]
            AlphaFS   : [\\?\UNC\SERVER001\Share]

    #059	Input Path: [\\?\UNC\SERVER001\Share\d1\]
            System.IO : [\\?\UNC]
            AlphaFS   : [\\?\UNC\SERVER001\Share]

    #060	Input Path: [\\?\UNC\SERVER001\Share\d1\d]
            System.IO : [\\?\UNC]
            AlphaFS   : [\\?\UNC\SERVER001\Share]

    #061	Input Path: [\\?\UNC\SERVER001\Share\d1\d2]
            System.IO : [\\?\UNC]
            AlphaFS   : [\\?\UNC\SERVER001\Share]

    #062	Input Path: [\\?\UNC\SERVER001\Share\d1\d2\]
            System.IO : [\\?\UNC]
            AlphaFS   : [\\?\UNC\SERVER001\Share]

    #063	Input Path: [\\?\UNC\SERVER001\Share\d1\d2\f]
            System.IO : [\\?\UNC]
            AlphaFS   : [\\?\UNC\SERVER001\Share]

    #064	Input Path: [\\?\UNC\SERVER001\Share\d1\d2\fi]
            System.IO : [\\?\UNC]
            AlphaFS   : [\\?\UNC\SERVER001\Share]

    #065	Input Path: [\\?\UNC\SERVER001\Share\d1\d2\fil]
            System.IO : [\\?\UNC]
            AlphaFS   : [\\?\UNC\SERVER001\Share]

    #066	Input Path: [\\?\UNC\SERVER001\Share\d1\d2\file]
            System.IO : [\\?\UNC]
            AlphaFS   : [\\?\UNC\SERVER001\Share]

    #067	Input Path: [\\?\UNC\SERVER001\Share\d1\d2\file.]
            System.IO : [\\?\UNC]
            AlphaFS   : [\\?\UNC\SERVER001\Share]

    #068	Input Path: [\\?\UNC\SERVER001\Share\d1\d2\file.e]
            System.IO : [\\?\UNC]
            AlphaFS   : [\\?\UNC\SERVER001\Share]

    #069	Input Path: [\\?\UNC\SERVER001\Share\d1\d2\file.ex]
            System.IO : [\\?\UNC]
            AlphaFS   : [\\?\UNC\SERVER001\Share]

    #070	Input Path: [\\?\UNC\SERVER001\Share\d1\d2\file.ext]
            System.IO : [\\?\UNC]
            AlphaFS   : [\\?\UNC\SERVER001\Share]

            *Duration: [1] ms. (00:00:00.0017152)
