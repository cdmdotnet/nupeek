using NuGet.Server;
using NuGet.Server.Infrastructure;

namespace NuRep
{
	public class SymbolPackageService : PackageService
	{
		public SymbolPackageService(IServerPackageRepository repository, IPackageAuthenticationService authenticationService) : base(repository, authenticationService)
		{
		}
	}
}