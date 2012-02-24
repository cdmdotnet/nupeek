# NuRep

NuRep is a NuGet repository that also servers symbols and sources

Install NuRep on your dev site, publish both nupkg and symbols.nupkg at http://myserver/NuRep

Then add http://myserver/NuRep/api/v2 as package source, and http://myserver/NuRep/symbols as symbol file server.

Don't forget to Enable source server support and disable Just My code.

have fun.

jeremie / thinkbeforcoding