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
            var plankReserve = 50;
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
            var possibleLengths = new List<float> {6000 - plankReserve, 6900 - plankReserve, 7000 - plankReserve };

            var planks = cuttingStockJob.CalculateCuts(possibleLengths);

            Console.WriteLine($"Use plank sizes: [{string.Join(", ", planks.Select(p => p.OriginalLength + plankReserve).Distinct())}]");
            Console.WriteLine();
            var columns = planks.Max(p => p.Cuts.Count);

            foreach (var plank in planks)
            {
                var tab = "";
                for (var i = 0; i < columns + 1 - plank.Cuts.Count; i++)
                {
                    tab += "\t\t\t";
                }

                Console.WriteLine($"Cut a {plank.OriginalLength + plankReserve} ~ {plank.OriginalLength} by:\t\t{string.Join(",\t", plank.Cuts)}{tab}with {plank.FreeLength} waste.");
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