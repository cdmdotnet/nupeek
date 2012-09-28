namespace NuPeek.DataServices
{
	public interface ISymbolPackagePathResolver
	{
		string GetSymbolPackagePath(string packageId, string version);
	}
}