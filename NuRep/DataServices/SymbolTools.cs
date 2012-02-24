using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Web;
using System.Web.Hosting;
using NuGet;

namespace NuRep
{
	public class SymbolTools
	{
		private readonly IToolPathResolver toolPathResolver;
		private readonly ISymbolsPathResolver symbolsPathResolver;


		public IEnumerable<string> GetSources(string symbolFile)
		{
			return ExecuteTool("srctool", "-r \"" + symbolFile + "\"");
		}

		public SymbolTools(IToolPathResolver toolPathResolver, ISymbolsPathResolver symbolsPathResolver)
		{
			this.toolPathResolver = toolPathResolver;
			this.symbolsPathResolver = symbolsPathResolver;
		}

		private IEnumerable<string> ExecuteTool(string tool, string arguments)
		{
			var process = Process.Start(new ProcessStartInfo(toolPathResolver.GetToolPath(tool), arguments) {CreateNoWindow = true, RedirectStandardOutput = true, UseShellExecute = false, LoadUserProfile = true});

			while (!process.StandardOutput.EndOfStream)
				yield return process.StandardOutput.ReadLine();
		}

		private void ExecuteToolNoValue(string tool, string arguments)
		{
			var process = Process.Start(new ProcessStartInfo(toolPathResolver.GetToolPath(tool), arguments) { CreateNoWindow = true, RedirectStandardOutput = true, UseShellExecute = false, LoadUserProfile = true });
			process.WaitForExit();
		}


		public void MapSources(IPackage package, string symbolFile, IEnumerable<string> referencedSources, Func<string, string> transform)
		{
			var version = package.Version;
			var packageId = package.Id;

			string indexFile = Path.ChangeExtension(symbolFile, ".index");

			try
			{
				using (var writer = File.CreateText(indexFile))
				{
					writer.WriteLine("SRCSRV: ini ------------------------------------------------");
					writer.WriteLine("VERSION=2");
					writer.WriteLine("INDEXVERSION=2");
					writer.WriteLine("VERCTRL=NuGet");
					writer.WriteLine("DATETIME={0}", DateTime.UtcNow);
					writer.WriteLine("SRCSRV: variables ------------------------------------------");
					writer.WriteLine("SRCSRVVERCTRL=http");
					writer.WriteLine("NUGET={0}", SourceBaseUri);
					writer.WriteLine("HTTP_EXTRACT_TARGET=%NUGET%/%var4%/%var2%/%var5%");
					writer.WriteLine("SRCSRVTRG=%HTTP_EXTRACT_TARGET%");
					writer.WriteLine("SRCSRVCMD=");
					writer.WriteLine("SRCSRV: source files ---------------------------------------");
					foreach (var source in referencedSources)
					{
						writer.WriteLine("{0}*{1}*_*{2}*{3}", source, version, packageId, transform(source));
					}
					writer.WriteLine("SRCSRV: end ------------------------------------------------");
				}

				ExecuteToolNoValue("pdbstr", string.Format(" -w -p:\"{0}\" -i:\"{1}\" -s:srcsrv", symbolFile, indexFile));
				ExecuteToolNoValue("symstore", string.Format(" add /f {0} /s {1} /t {2} /v {3}", symbolFile, symbolsPathResolver.GetSymbolsPath(), packageId, version));
			}
			finally
			{
				File.Delete(indexFile);
			}
			
		}

		private static string SourceBaseUri
		{
			get
			{
				var httpRequest = HttpContext.Current.Request;
				
				var applicationUri = new Uri(new Uri(httpRequest.Url.GetLeftPart(UriPartial.Scheme | UriPartial.Authority)),httpRequest.ApplicationPath);
				return new Uri(applicationUri, "source").ToString();
			}
		}
	}
}