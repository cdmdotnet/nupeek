using NuGet.Server.Infrastructure;
using NuGet.Server.Publishing;

namespace NuPeek.DataServices
{
	public class SymbolPackageService : PackageService
	{
		public SymbolPackageService(IServerPackageRepository repository, IPackageAuthenticationService authenticationService) : base(repository, authenticationService)
		{
		}
	}
}