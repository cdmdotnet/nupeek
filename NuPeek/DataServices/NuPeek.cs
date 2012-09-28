using System.Web.Routing;
using Ninject;
using NuGet.Server.Infrastructure;
using NuPeek.DataServices;
using RouteMagic;

[assembly: WebActivator.PreApplicationStartMethod(typeof(NuPeek.DataServices.NuPeek), "Start")]

namespace NuPeek.DataServices
{
	public class NuPeek
	{
		public static void Start()
		{
			NinjectBootstrapper.Kernel.Load(new NuPeekBinding());

			MapRoutes(RouteTable.Routes);
		}

		private static void MapRoutes(RouteCollection routes)
		{
			var routeBase = routes["CreatePackage"];
			routes.Remove(routeBase);
			routes.Remove(routes["CreatePackage-Root"]);

			routes.MapDelegate("Override_CreatePackage",
				"api/v2/package",
				new { httpMethod = new HttpMethodConstraint("PUT") },
				context => CreateSymbolService().CreatePackage(context.HttpContext));

			routes.MapDelegate("Override_CreatePackage-Root",
				"",
				new { httpMethod = new HttpMethodConstraint("PUT") },
				context => CreateSymbolService().CreatePackage(context.HttpContext));


			routes.MapDelegate("GetSymbols",
			    "symbols/{*path}",
			    new {httpMethod = new HttpMethodConstraint("GET")},
			    context =>CreateSymbolService().GetSymbols(context));

			routes.MapDelegate("GetSource",
				"source/{id}/{version}/{*path}",
			    new {httpMethod = new HttpMethodConstraint("GET")},
				context => CreateSymbolService().GetSource(context));
		}


		private static SymbolService CreateSymbolService()
		{
			return NinjectBootstrapper.Kernel.Get<SymbolService>();
		}
	}
}