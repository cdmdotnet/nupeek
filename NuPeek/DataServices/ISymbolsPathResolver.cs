using NuGet;

namespace NuPeek.DataServices
{
	public interface ISymbolsPathResolver
	{
		string GetSymbolsPath(IPackage package, IPackageFile symbolFile);
		string GetSymbolsPath(string localPath);
		string GetSymbolsPath();
	}
}