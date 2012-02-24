namespace NuRep
{
	public interface ISymbolPackagePathResolver
	{
		string GetSymbolPackagePath(string packageId, string version);
	}
}