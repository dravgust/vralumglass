using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dropbox.Api;
using Dropbox.Api.Files;
using Microsoft.Extensions.Options;
using Vralumglass.Core;

namespace Vralumglass.Dropbox
{
	public class DropboxStorage : IFileStorage
	{
		private FileStorageSettings FileStorageSettings { get; }

		public DropboxStorage(IOptions<FileStorageSettings> fileStorageOptions)
		{
			FileStorageSettings = fileStorageOptions?.Value;
		}

		public async Task<List<string>> Search(string path, string query, ulong maxResults = 100UL)
		{
			var result = new List<string>();
			using (var dbx = new DropboxClient(FileStorageSettings.AccessToken))
			{
				var searchResult = await dbx.Files.SearchAsync(path, query, 0, maxResults, SearchMode.Filename.Instance);
				foreach (var resultMatch in searchResult.Matches)
				{
					result.Add(resultMatch.Metadata.PathLower);
				}
			}

			return result;
		}

		public async Task<List<FileStorageInfo>> ListRootFolderAsync()
		{
			var result = new List<FileStorageInfo>();

			using (var dbx = new DropboxClient(FileStorageSettings.AccessToken))
			{
				var list = await dbx.Files.ListFolderAsync(string.Empty);


				foreach (var item in list.Entries.Where(i => i.IsFolder))
				{
					result.Add(new FileStorageInfo
					{
						Id = item.AsFolder.Id,
						Name = item.Name,
						Path  = item.PathDisplay,
						IsFolder = true
					});
				}

				foreach (var item in list.Entries.Where(i => i.IsFile))
				{
					result.Add(new FileStorageInfo
					{
						Id = item.AsFile.Id,
						Name = item.Name,
						Path = item.PathDisplay,
						Size = item.AsFile.Size
					});
				}
			}

			return result;
		}

		public async Task<string> Upload(string folder, string file, string content)
		{
			using (var dbx = new DropboxClient(FileStorageSettings.AccessToken))
			{
				using (var mem = new MemoryStream(Encoding.UTF8.GetBytes(content)))
				{
					var updated = await dbx.Files.UploadAsync(folder + "/" + file, WriteMode.Overwrite.Instance, body: mem);

					return $"Saved {folder}/{file} rev {updated.Rev}";
				}
			}
		}

		async Task ListRootFolder(DropboxClient dbx)
		{
			var list = await dbx.Files.ListFolderAsync(string.Empty);

			// show folders then files
			foreach (var item in list.Entries.Where(i => i.IsFolder))
			{
				Console.WriteLine("D  {0}/", item.Name);
			}

			foreach (var item in list.Entries.Where(i => i.IsFile))
			{
				Console.WriteLine("F{0,8} {1}", item.AsFile.Size, item.Name);
			}
		}

		public async Task Download(DropboxClient dbx, string folder, string file)
		{
			using (var response = await dbx.Files.DownloadAsync(folder + "/" + file))
			{
				Trace.WriteLine(await response.GetContentAsStringAsync());
			}
		}

		public async Task Upload(DropboxClient dbx, string folder, string file, string content)
		{
			using (var mem = new MemoryStream(Encoding.UTF8.GetBytes(content)))
			{
				var updated = await dbx.Files.UploadAsync(
					folder + "/" + file,
					WriteMode.Overwrite.Instance,
					body: mem);
				Trace.WriteLine($"Saved {folder}/{file} rev {updated.Rev}");
			}
		}

		public void Dispose()
		{

		}
	}
}
