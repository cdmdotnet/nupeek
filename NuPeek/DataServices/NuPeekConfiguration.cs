using System;
using System.Configuration;
using System.IO;
using System.Web.Hosting;

namespace NuPeek.DataServices
{
	public static class NuPeekConfiguration
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

				string localPath = path.StartsWith("~") ? HostingEnvironment.MapPath(path) : path;

				return Path.Combine(localPath, Environment.Is64BitProcess ? "x64" : "x86");
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