using Ninject;
using Ninject.Modules;
using NuGet.Server.Infrastructure;

namespace NuPeek.DataServices
{
	public class NuPeekBinding : NinjectModule
	{
		public override void Load()
		{
			Kernel.Bind<SymbolPackageService>().ToConstant(new SymbolPackageService(new ServerPackageRepository(NuPeekConfiguration.SymbolPackagePath), Kernel.Get<IPackageAuthenticationService>()));
			Kernel.Bind<ISymbolsPathResolver>().To<SymbolsPathResolver>();
			Kernel.Bind<ISymbolPackagePathResolver>().To<SymbolPackagePathResolver>();
			Kernel.Bind<IToolPathResolver>().To<ToolPathResolver>();
		}
	}
}