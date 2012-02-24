using System.IO;

namespace NuRep
{
	public class SymbolPackagePathResolver : ISymbolPackagePathResolver
	{
		public string GetSymbolPackagePath(string packageId, string version)
		{
			var basePath = NuRepConfiguration.SymbolPackagePath;
			return Path.Combine(basePath, packageId + "." + version + ".nupkg");

		}
	}
}