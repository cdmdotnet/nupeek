using System;
using System.IO;
using NuGet;

namespace NuRep
{
	public class SymbolsPathResolver : ISymbolsPathResolver
	{
		public string GetSymbolsPath(IPackage package, IPackageFile symbolFile)
		{
			string localPath = String.Format(@"temp\{0}.{1}\{2}", package.Id, package.Version, symbolFile.Path);
			return GetSymbolsPath(localPath);
		}

		public string GetSymbolsPath(string localPath)
		{
			return Path.Combine(GetSymbolsPath(), localPath);
		}

		public string GetSymbolsPath()
		{
			return NuRepConfiguration.SymbolsPath;
		}
	}
}