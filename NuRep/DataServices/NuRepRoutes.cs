using System.Web.Routing;
using Ninject;
using Ninject.Planning.Bindings;
using NuGet.Server.Infrastructure;
using RouteMagic;

[assembly: WebActivator.PreApplicationStartMethod(typeof(NuRep.NuRepRoutes), "Start")]

namespace NuRep
{
	public class NuRepRoutes
	{
		public static void Start()
		{
			NinjectBootstrapper.Kernel.Load(new NuRepBinding());

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