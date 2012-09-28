# NuPeek

NuPeek (previously NuRep) is a NuGet repository that also serves symbols and sources.

Install NuPeek on your dev site, publish both nupkg and symbols.nupkg at http://myserver/NuPeek (the http path where you installed your application)

Then add http://myserver/NuPeek/nuget as package source, and http://myserver/NuPeek/symbols as symbol file server.

Don't forget to Enable source server support and disable Just My code.

have fun.

jeremie / thinkbeforecoding