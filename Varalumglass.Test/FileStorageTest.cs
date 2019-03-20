using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using Vralumglass.Core;
using Vralumglass.Dropbox;

namespace Tests
{
	public class Tests
	{
		private IOptions<FileStorageSettings> _fileStorageOptions;
		[SetUp]
		public void Setup()
		{
			_fileStorageOptions = Options.Create(new FileStorageSettings
			{
				AccessToken = "wG1RXvQ0W5AAAAAAAAAADBdy-u6D4-z5nRcY5o2w21vgjCPnahe5nFieaMVe2qRA"
			});
		}

		[Test]
		public void Test1()
		{
			using (var dbx = new DropboxStorage(_fileStorageOptions))
			{
				var root = dbx.ListRootFolderAsync().Result;
				foreach (var item in root)
				{
					Console.WriteLine(item.ToJson());
				}
			}
			Assert.Pass();
		}

		[Test]
		public void Test2()
		{
			using (var dbx = new DropboxStorage(_fileStorageOptions))
			{
				var res = dbx.Upload("/BeerSheva_Rambam.497c06f4-9c6b-4d62-a1e3-8bac769047b4/4/35", "metadata.txt", "test").Result;
				Console.WriteLine(res);
			}

			Assert.Pass();
		}

		[Test]
		public void Test3()
		{
			using (var dbx = new DropboxStorage(_fileStorageOptions))
			{
				var res = dbx.Search("", ".png").Result;
				Console.WriteLine(res.ToJson());
			}

			Assert.Pass();
		}
	}
}