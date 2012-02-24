using System.Web;
using Ninject;
using Ninject.Modules;
using NuGet.Server;
using NuGet.Server.Infrastructure;

namespace NuRep
{
	public class NuRepBinding : NinjectModule
	{
		public override void Load()
		{
			var defaultPackageService = Kernel.Get<PackageService>();

			Kernel.Bind<PackageService>().ToConstant(defaultPackageService).Named("Default");

			Kernel.Bind<PackageService>().ToConstant(new PackageService(new ServerPackageRepository(NuRepConfiguration.SymbolPackagePath), Kernel.Get<IPackageAuthenticationService>())).Named("Symbols");
			Kernel.Bind<ISymbolsPathResolver>().To<SymbolsPathResolver>();
			Kernel.Bind<ISymbolPackagePathResolver>().To<SymbolPackagePathResolver>();
			Kernel.Bind<IToolPathResolver>().To<ToolPathResolver>();
		}
	}
}