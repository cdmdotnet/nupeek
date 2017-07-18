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

		public string GetSymbolSourcePath(string packageId, string version)
		{
			var basePath = Path.Combine(NuPeekConfiguration.SymbolPackagePath, packageId, version);
			return Path.Combine(basePath, packageId + "." + version + ".nupkg");
		}
	}
}