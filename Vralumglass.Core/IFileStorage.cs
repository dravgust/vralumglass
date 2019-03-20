using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Vralumglass.Core
{
	public interface IFileStorage : IDisposable
	{
		Task<List<FileStorageInfo>> ListRootFolderAsync();
		Task<string> Upload(string folder, string file, string content);
		Task<List<string>> Search(string path, string query, ulong maxResults = 100UL);
	}
}
