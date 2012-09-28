using System.IO;

namespace NuPeek.DataServices
{
	public class ToolPathResolver : IToolPathResolver
	{
		public string GetToolPath(string name)
		{
			var basePath = NuPeekConfiguration.ToolsPath;
			return Path.Combine(basePath, name);

		}
	}
}