using Ninject;
using Ninject.Modules;
using NuGet;
using NuGet.Server.Infrastructure;
using NuGet.Server.Logging;

namespace NuPeek.DataServices
{
	public class NuPeekBinding : NinjectModule
	{
		public override void Load()
		{
			Kernel.Bind<SymbolPackageService>().ToConstant
			(
				new SymbolPackageService
				(
					new ServerPackageRepository
					(
						NuPeekConfiguration.SymbolPackagePath,
						new CryptoHashProvider(),
						new TraceLogger()
					),
					Kernel.Get<IPackageAuthenticationService>()
				)
			);
			Kernel.Bind<ISymbolsPathResolver>().To<SymbolsPathResolver>();
			Kernel.Bind<ISymbolPackagePathResolver>().To<SymbolPackagePathResolver>();
			Kernel.Bind<IToolPathResolver>().To<ToolPathResolver>();
		}
	}
}