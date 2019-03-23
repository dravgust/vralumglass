using System;
using System.Diagnostics;
using System.IO;
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
				AccessToken = "wG1RXvQ0W5AAAAAAAAAADBdy-u6D4-z5nRcY5o2w21vgjCPnahe5nFieaMVe2qRA",
                BaseFolder = null
			});
		}

		[Test]
		public void Test1()
        {
            var dbx = new DropboxStorage(_fileStorageOptions);
			{
				var root = dbx.ListFolderAsync().Result;
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
            var dbx = new DropboxStorage(_fileStorageOptions);
            {
                var f = File.ReadAllBytes(@"D:\\My Books\\startap-bez-byudzheta.pdf");

                var res = dbx.Upload("/BeerSheva_Rambam/A/35", "metadata.pdf", f).Result;
                Console.WriteLine(res);
            }

            Assert.Pass();
        }

        [Test]
		public void Test3()
		{
			var dbx = new DropboxStorage(_fileStorageOptions);
            {
				var res = dbx.Download("/BeerSheva_Rambam/A/35", "metadata2.pdf").Result;
                File.WriteAllBytes(@"D:\\My Books\\startap-bez-byudzheta2.pdf", res);
                Console.WriteLine(res);
			}

			Assert.Pass();
		}

		[Test]
		public void Test4()
        {
            var dbx = new DropboxStorage(_fileStorageOptions);
			{
				var res = dbx.Search("", ".png").Result;
				Console.WriteLine(res.ToJson());
			}

			Assert.Pass();
		}
	}
}