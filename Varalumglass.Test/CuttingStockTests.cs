using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using Varalumglass.Test;
using Vralumglass.Core;
using Vralumglass.Core.Interfaces;
using Vralumglass.Core.Models;
using Vralumglass.Dropbox;

namespace Tests
{
	public class CuttingStockTests
	{
        public class MyClass
        {
            public List<float> Planks { get; set; } = new List<float>();

            public List<TestSnippet> Snippets { get; set; } = new List<TestSnippet>();
        }

		[SetUp]
		public void Setup()
		{
			
		}

		[Test]
		public void Test1()
        {
            //The cuts to be made
            var desiredLengths = new List<float>
            {
                3057, 6774, 2312, 2282, 7031, 1277, 1252, 6998, 2252, 2337, 6739, 2312, 3097, 6797, 2348, 2275, 7039,
                1292, 3027, 3804, 252, 1277, 6974, 2267, 2332, 6789, 2342, 3092, 6847, 2322, 2272, 7017, 1289, 3022,
                3784, 227, 1272, 6946, 2272, 2347, 6764, 2347, 3113, 6769, 2317, 2287, 6987, 1292, 3050, 3754, 242,
                1257, 6917, 2217, 2357, 6759, 2352, 3112, 6779, 2347, 2312, 6979, 1282, 3052, 3784, 237, 1252, 6921,
                2212, 2372, 6734, 2352, 3117, 6804, 2382, 2297, 6995, 1307, 3047, 3774, 247, 1267, 6926, 2217, 2377,
                6794, 2372, 3132, 6784, 2387, 2272, 6975, 1312, 3062, 3814, 272, 1257, 6899, 2212, 2397, 6804, 2337,
                3152, 6831, 2422, 2262, 7057, 1292, 3059, 3854, 272, 1262, 6940, 2202, 2407, 6769, 2352, 3147, 6839,
                2417, 2302, 7059, 1262, 3067, 3854, 267, 1272, 6964, 2207, 2412, 6749, 2342, 3152, 6856, 2387, 2322,
                7052, 1282, 3044, 3829, 272, 1272, 6986, 2222, 2407, 6769, 2342,
            };

            var snippets = new List<ISnippet>();
            var counter = 0;
            for (var i = 1; i <= 50; i++)
            {
                if (i == 3) continue;

                snippets.Add(new TestSnippet(desiredLengths[counter]) { Apartment = i });
                snippets.Add(new TestSnippet(desiredLengths[counter + 1]) { Apartment = i });
                snippets.Add(new TestSnippet(desiredLengths[counter + 2]) { Apartment = i });

                counter += 3;
            }

            var cuttingStockJob = new CuttingStock(snippets);

            //The possible lengths of plank //
            var possibleLengths = new List<float> { 7050, 4950 };

            var planks = cuttingStockJob.CalculateCuts(possibleLengths);

            Console.WriteLine($"Use plank sizes: [{string.Join(", ", planks.Select(p => p.OriginalLength).Distinct())}]");
            Console.WriteLine();
            foreach (var plank in planks)
            {
                Console.WriteLine("Cut a {0} by: {1} with {2} waste.", plank.OriginalLength, string.Join(", ", plank.Cuts), plank.FreeLength);
            }

            var waste = CuttingStock.GetFree(planks);
            Console.WriteLine("Finished with {0} waste => {1}%", waste, waste * 100 / desiredLengths.Sum());

            Assert.Pass();
		}
	}

    public class TestSnippet : Snippet
    {
        public TestSnippet(float length) : base(length)
        {

        }

        public int Apartment { get; set; }

        public override string ToString()
        {
            return $"{Length}[{Apartment}]";
        }

        public override ISnippet Clone(float length)
        {
            return new TestSnippet(length) { Apartment = Apartment };
        }
    }
}