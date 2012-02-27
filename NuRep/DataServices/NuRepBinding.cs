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
			Kernel.Bind<SymbolPackageService>().ToConstant(new SymbolPackageService(new ServerPackageRepository(NuRepConfiguration.SymbolPackagePath), Kernel.Get<IPackageAuthenticationService>()));
			Kernel.Bind<ISymbolsPathResolver>().To<SymbolsPathResolver>();
			Kernel.Bind<ISymbolPackagePathResolver>().To<SymbolPackagePathResolver>();
			Kernel.Bind<IToolPathResolver>().To<ToolPathResolver>();
		}
	}
}