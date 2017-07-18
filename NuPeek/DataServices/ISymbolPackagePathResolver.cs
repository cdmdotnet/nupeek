namespace NuPeek.DataServices
{
	public interface ISymbolPackagePathResolver
	{
		string GetSymbolPackagePath(string packageId, string version);

		string GetSymbolSourcePath(string packageId, string version);
	}
}