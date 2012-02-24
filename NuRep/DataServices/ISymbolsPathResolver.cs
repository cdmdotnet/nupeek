using NuGet;

namespace NuRep
{
	public interface ISymbolsPathResolver
	{
		string GetSymbolsPath(IPackage package, IPackageFile symbolFile);
		string GetSymbolsPath(string localPath);
		string GetSymbolsPath();
	}
}