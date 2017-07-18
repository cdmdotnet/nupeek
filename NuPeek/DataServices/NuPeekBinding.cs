using Ninject;
using Ninject.Modules;
using NuGet;
using NuGet.Server.Infrastructure;
using NuGet.Server.Logging;
using NuGet.Server.Publishing;

namespace NuPeek.DataServices
{
	public class NuPeekBinding : NinjectModule
	{
		public override void Load()
		{
			Bind<ISymbolsPathResolver>().To<SymbolsPathResolver>();
			Bind<ISymbolPackagePathResolver>().To<SymbolPackagePathResolver>();
			Bind<IToolPathResolver>().To<ToolPathResolver>();
			Bind<IPackageAuthenticationService>().To<PackageAuthenticationService>();

			var serverPackageRepository = new ServerPackageRepository
			(
				NuPeekConfiguration.NugetPackagePath,
				new CryptoHashProvider(),
				new TraceLogger()
			);
			Bind<IServerPackageRepository>().ToConstant(serverPackageRepository);
			var symbolPackageService = new SymbolPackageService
			(
				new ServerPackageRepository
				(
					NuPeekConfiguration.SymbolPackagePath,
					new CryptoHashProvider(),
					new TraceLogger()
				),
				Kernel.Get<IPackageAuthenticationService>()
			);
			Bind<SymbolPackageService>().ToConstant(symbolPackageService);
			var symbolService = new SymbolService
			(
				Kernel.Get<PackageService>(),
				symbolPackageService,
				Kernel.Get<ISymbolsPathResolver>(),
				Kernel.Get<ISymbolPackagePathResolver>(),
				Kernel.Get<SymbolTools>()
			);
			Bind<SymbolService>().ToConstant(symbolService);
		}
	}
}