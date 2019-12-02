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
            //var desiredLengths = new List<float>
            //{
            //    3057, 6774, 2312, 2282, 7031, 1277, 1252, 6998, 2252, 2337, 6739, 2312, 3097, 6797, 2348, 2275, 7039,
            //    1292, 3027, 3804, 252, 1277, 6974, 2267, 2332, 6789, 2342, 3092, 6847, 2322, 2272, 7017, 1289, 3022,
            //    3784, 227, 1272, 6946, 2272, 2347, 6764, 2347, 3113, 6769, 2317, 2287, 6987, 1292, 3050, 3754, 242,
            //    1257, 6917, 2217, 2357, 6759, 2352, 3112, 6779, 2347, 2312, 6979, 1282, 3052, 3784, 237, 1252, 6921,
            //    2212, 2372, 6734, 2352, 3117, 6804, 2382, 2297, 6995, 1307, 3047, 3774, 247, 1267, 6926, 2217, 2377,
            //    6794, 2372, 3132, 6784, 2387, 2272, 6975, 1312, 3062, 3814, 272, 1257, 6899, 2212, 2397, 6804, 2337,
            //    3152, 6831, 2422, 2262, 7057, 1292, 3059, 3854, 272, 1262, 6940, 2202, 2407, 6769, 2352, 3147, 6839,
            //    2417, 2302, 7059, 1262, 3067, 3854, 267, 1272, 6964, 2207, 2412, 6749, 2342, 3152, 6856, 2387, 2322,
            //    7052, 1282, 3044, 3829, 272, 1272, 6986, 2222, 2407, 6769, 2342,
            //};
            var desiredLengths = new List<(float length, int appartment, int floor)>
            {
                (1870, 4, 1), (5500, 4, 1), (2170, 4, 1), (1450, 5, 1), (360, 5, 1), (10450, 5, 1), (1450, 6, 1), (360, 6, 1), (2170, 7, 1), (4580, 7, 1), (2170, 7, 1), 
                (1870, 8, 2), (5500, 8, 2), (2170, 8, 2), (1450, 9, 2), (360, 9, 2), (10450, 9, 2), (1450, 10, 2), (360, 10 ,2), (2170, 11, 2), (4580, 11, 2), (2170, 11, 2), 
                (1870, 12, 3), (5500, 12, 3), (2170, 12, 3), (2600, 13, 3), (9020, 13, 3), (2770, 14, 3), (3910, 14, 3), (470, 14, 3), (2170, 15, 3), (4580, 15, 3), (2170, 15, 3), 
                (1870, 16, 4), (5500, 16, 4), (2170, 16, 4), (2770, 17, 4), (3950, 17, 4), (2770, 18, 4), (3910, 18, 4), (470, 18, 4), (2170, 19, 4), (4580, 19, 4), (2170, 19, 4), 
                (1870, 20, 5), (5500, 20, 5), (2170, 20, 5), (2770, 21, 5), (3950, 21, 5), (2770, 22, 5), (3910, 22, 5), (470, 22, 5), (2170, 23, 5), (4580, 23, 5), (2170, 23, 5),
                (1870, 24, 6), (5500, 24, 6), (2170, 24, 6), (2770, 25, 6), (3950, 25, 6), (2770, 26, 6), (3910, 26, 6), (470, 26, 6), (2170, 27, 6), (4580, 27, 6), (2170, 27, 6),
                (1870, 28, 7), (5500, 28, 7), (2170, 28, 7), (330, 29, 7), (1430, 29, 7), (3750, 29, 7), (330, 30, 7), (1430, 30, 7), (6150, 30, 7), (1600, 31, 7), (1820, 31, 7), (3680, 31, 7), (5440, 31, 7),
                (1870, 32, 8), (5500, 32, 8), (2170, 32, 8), (330, 33, 8), (1430, 33, 8), (3750, 33, 8), (330, 34, 8), (1430, 34, 8), (6150, 34, 8), (2170, 35, 8), (4580, 35, 8), (2170, 35, 8),
                (1870, 36, 9), (10220, 36, 9), (2170, 36, 9), (9600, 37, 9), (1100, 37, 9), (13400, 38, 9), (1100, 38, 9),  (2200, 39, 9), (12220, 39, 9), (2070, 39, 9), (400, 39, 9)
            };

            var snippets = new List<ISnippet>();
            foreach (var (length, apartment, floor) in desiredLengths)
            {
                snippets.Add(new TestSnippet(length) { Apartment = apartment, Floor = floor});
            }

            var cuttingStockJob = new CuttingStock(snippets);

            //The possible lengths of plank //
            var possibleLengths = new List<float> { 3690, 6000, 6500, 7000 };

            var planks = cuttingStockJob.CalculateCuts(possibleLengths);

            Console.WriteLine($"Use plank sizes: [{string.Join(", ", planks.Select(p => p.OriginalLength).Distinct())}]");
            Console.WriteLine();
            var columns = planks.Max(p => p.Cuts.Count);

            foreach (var plank in planks)
            {
                var tab = "";
                for (var i = 0; i < columns + 1 - plank.Cuts.Count; i++)
                {
                    tab += "\t\t\t";
                }

                Console.WriteLine("Cut a {0} by:\t\t{1}{2}with {3} waste.", plank.OriginalLength, string.Join(",\t", plank.Cuts), tab, plank.FreeLength);
            }

            var waste = CuttingStock.GetFree(planks);
            Console.WriteLine("\r\nFinished with {0} waste => {1:#.##}%", waste, waste * 100 / desiredLengths.Sum(i => i.length));

            Assert.Pass();
		}
	}

    public class TestSnippet : Snippet
    {
        public TestSnippet(float length) : base(length)
        {

        }

        public int Apartment { get; set; }
        public int Floor { get; set; }

        public override string ToString()
        {
            return $"{Length}[{Floor}/{Apartment}]";
        }

        public override ISnippet Clone(float length)
        {
            return new TestSnippet(length) { Apartment = Apartment, Floor = Floor };
        }
    }
}