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


			Kernel.Bind<ServerPackageRepository>().ToConstant(new ServerPackageRepository(NuRepConfiguration.SymbolPackagePath)).WhenInjectedInto<SymbolPackageService>();

			Kernel.Bind<ISymbolsPathResolver>().To<SymbolsPathResolver>();
			Kernel.Bind<ISymbolPackagePathResolver>().To<SymbolPackagePathResolver>();
			Kernel.Bind<IToolPathResolver>().To<ToolPathResolver>();
		}
	}
}