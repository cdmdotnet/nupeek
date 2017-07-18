using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Routing;
using NuGet;
using NuGet.Server.Publishing;

namespace NuPeek.DataServices
{
	public class SymbolService
	{
		private readonly PackageService packageService;
		private readonly PackageService symbolPackageService;
		private readonly ISymbolsPathResolver symbolsPathResolver;
		private readonly ISymbolPackagePathResolver symbolPackagePathResolver;
		private readonly SymbolTools symbolTools;


		public SymbolService(PackageService packageService, SymbolPackageService symbolPackageService,
			ISymbolsPathResolver symbolsPathResolver, ISymbolPackagePathResolver symbolPackagePathResolver,
			SymbolTools symbolTools)
		{
			this.packageService = packageService;
			this.symbolPackageService = symbolPackageService;
			this.symbolsPathResolver = symbolsPathResolver;
			this.symbolPackagePathResolver = symbolPackagePathResolver;
			this.symbolTools = symbolTools;
		}

		public void CreatePackage(HttpContextBase context)
		{
			var request = context.Request;
			var stream = request.Files.Count > 0 ? request.Files[0].InputStream : request.InputStream;
			var clonedStream = CloneStream(stream);
			var package = new ZipPackage(clonedStream); // provide a cloned stream because ZipPackage now dispose it

			var symbolFiles = package.GetFiles("lib").Where(f => Path.GetExtension(f.Path) == ".pdb").ToList();

			foreach (var symbolFile in symbolFiles)
				ProcessSymbolFile(symbolFile, package);

			if (IsSymbolPackage(package, symbolFiles))
				symbolPackageService.CreatePackage(context);
			else
				packageService.CreatePackage(context);
		}

		private static MemoryStream CloneStream(Stream stream)
		{
			var clonedStream = new MemoryStream();
			stream.CopyTo(clonedStream);
			stream.Position = 0;
			clonedStream.Position = 0;
			return clonedStream;
		}

		private static bool IsSymbolPackage(ZipPackage package, IEnumerable<IPackageFile> symbolFiles)
		{
			return symbolFiles.Any() && package.GetFiles("src").Any();
		}

		private void ProcessSymbolFile(IPackageFile symbolFile, ZipPackage package)
		{
			var tempSymbolFilePath = ExtractToSymbols(package, symbolFile);

			try
			{
				var referencedSources = symbolTools.GetSources(tempSymbolFilePath).ToList();

				var sourceFiles = new HashSet<string>(package.GetFiles("src").Select(f => f.Path.ToLowerInvariant()).ToList());
				if (referencedSources.Any() && sourceFiles.Any())
				{
					var basePath = FindSourceBasePath(sourceFiles, referencedSources);

					if (basePath.Length != 0)
						symbolTools.MapSources(package, tempSymbolFilePath, referencedSources, r => r.Substring(basePath.Length));
				}
			}
			finally
			{
				File.Delete(tempSymbolFilePath);
			}
		}

		private static string FindSourceBasePath(HashSet<string> sourceFiles, List<string> referencedSources)
		{
			return referencedSources.Aggregate(
				referencedSources[0],
				(b, s) =>
				{
					while (b.Length > 0 && (!s.StartsWith(b) || !sourceFiles.Contains(Path.Combine("src", s.Substring(b.Length)))))
						b = b.Substring(0, b.Length - 1);
					return b;
				});
		}

		private string ExtractToSymbols(IPackage package, IPackageFile symbolFile)
		{
			var path = symbolsPathResolver.GetSymbolsPath(package, symbolFile);

			EnsureDirectory(path);

			using (var file = File.Open(path, FileMode.OpenOrCreate))
				symbolFile.GetStream().CopyTo(file);

			return path;
		}

		private void EnsureDirectory(string path)
		{
			var directory = Path.GetDirectoryName(path);
			if (directory == null)
				throw new InvalidOperationException("Path has no directory");
			if (!Directory.Exists(directory))
				Directory.CreateDirectory(directory);
		}

		public void GetSymbols(RequestContext context)
		{
			var path = (string) context.RouteData.Values["path"];
			var fullPath = symbolsPathResolver.GetSymbolsPath(path);
			var expandedPath = symbolTools.ExpandSymbolFile(fullPath);

			var response = context.HttpContext.Response;
			if (!File.Exists(expandedPath))
			{
				response.StatusCode = 404;
				return;
			}

			response.ContentType = "application/octet-stream";
			response.WriteFile(expandedPath);
		}

		public void GetSource(RequestContext context)
		{
			var packageId = context.RouteData.Values["id"].ToString();
			var version = context.RouteData.Values["version"].ToString();
			var path = Path.Combine("src", context.RouteData.Values["path"].ToString().Replace('/', '\\'));

			var response = context.HttpContext.Response;

			ZipPackage package;
			try
			{
				package = new ZipPackage(symbolPackagePathResolver.GetSymbolSourcePath(packageId, version));
			}
			catch (FileNotFoundException)
			{
				response.StatusCode = 404;
				return;
			}

			string directory = Path.GetDirectoryName(path);
			var file =
				package.GetFiles(directory)
					.FirstOrDefault(f => string.Equals(f.Path, path, StringComparison.InvariantCultureIgnoreCase));
			if (file == null)
			{
				response.StatusCode = 404;
				return;
			}
			response.ContentType = "binary/octet-stream";
			file.GetStream().CopyTo(response.OutputStream);

		}
	}
}