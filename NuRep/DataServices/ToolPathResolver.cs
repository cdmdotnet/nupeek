using System.IO;

namespace NuRep
{
	public class ToolPathResolver : IToolPathResolver
	{
		public string GetToolPath(string name)
		{
			var basePath = NuRepConfiguration.ToolsPath;
			return Path.Combine(basePath, name);

		}
	}
}