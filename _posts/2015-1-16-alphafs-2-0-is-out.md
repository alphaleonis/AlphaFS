---
layout: post
title: AlphaFS 2.0 is Out!
author: Yomodo
---

<div class="well bg-info">
<span class="lead">Excited about your latest masterpiece, but filled with anger and fear
<br />when you find out that a .NET path can still be too long for your app.</span>
</div>

So, this is exactly how I felt, mostly angry.

I had written the best next-gen-file-explorer only to find out that it kinda sucked when I reached that dreaded `PATH TOO LONG` error.

Just as you did, I searched the net and searched the net and searched until I came across AlphaFS 1.5.
<br />Wohoo! I was SO happy to see that people actually had put together this marvelous library
<br />that in the end my next-gen became old-skool because I completely abandoned it and totally focused on AlphaFS.

The result of that is presented here today!

We are proud to present to you **version 2.0 of AlphaFS**.

> "When the Path is too long, one's journey may end.
<br />Observing the First, as Alpha moves on."

*Yomodo*

### Highlights of version 2.0
- AlphaFS is now distributed primarily at [NuGet](https://www.nuget.org/packages/AlphaFS/).
- Because of major inspiration, some backward compatibility may have been broken. Forgive us for this, but we're sure you'll quickly recover.
- Implementations of major .NET classes (File, Directory and Path, ...) up until .NET 4.5.
- Implemented a brand new file systems enumerator.
- Added support for working with files and folders with a trailing dot or space.
- Added support for AlternateDataStreams.
- Added support for accessing network resources, like SMB and DFS shares.
- The repository was moved to [GitHub](https://github.com/alphaleonis/AlphaFS/), to allow for third party developers to more easily contribute to the library.
