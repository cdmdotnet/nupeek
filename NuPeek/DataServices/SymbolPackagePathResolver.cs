using System.IO;

namespace NuPeek.DataServices
{
	public class SymbolPackagePathResolver : ISymbolPackagePathResolver
	{
		public string GetSymbolPackagePath(string packageId, string version)
		{
			var basePath = NuPeekConfiguration.SymbolPackagePath;
			return Path.Combine(basePath, packageId + "." + version + ".nupkg");

		}
	}
}