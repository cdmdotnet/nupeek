using System.Configuration;
using System.Web.Hosting;

namespace NuRep
{
	public static class NuRepConfiguration
	{

		public static string SymbolPackagePath
		{
			get
			{
				string path = GetSetting("symbolPackagesPath","~/SymbolPackages");

				if (path.StartsWith("~"))
					return HostingEnvironment.MapPath(path);

				return path;
			}
		}

		public static string SymbolsPath
		{
			get
			{
				string path = GetSetting("symbolsPath", "~/Symbols");

				if (path.StartsWith("~"))
					return HostingEnvironment.MapPath(path);

				return path;
			}
		}

		public static string ToolsPath
		{
			get
			{
				string path = GetSetting("toolsPath", "~/Tools");

				if (path.StartsWith("~"))
					return HostingEnvironment.MapPath(path);

				return path;
			}
		}

		private static string GetSetting(string name, string defaultValue)
		{
			var value = ConfigurationManager.AppSettings[name];
			if (string.IsNullOrEmpty(value))
				return defaultValue;
			return value;
		}
	}
}