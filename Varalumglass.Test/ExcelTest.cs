using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Options;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NUnit.Framework;
using Vralumglass.Core;
using Vralumglass.Dropbox;

namespace Varalumglass.Test
{
    class ExcelTest
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Test1()
        {
            var result = new List<ManagementDefect>();
            var data = File.ReadAllBytes(@"D:\My Downloads\DefectList.xlsx");
            using (var ms = new MemoryStream(data))
            {
                ms.Position = 0;
                var wb = new XSSFWorkbook(ms);
                ISheet excelSheet = wb.GetSheetAt(0);

                for (var i = 1; i <= excelSheet.LastRowNum; i++)
                {
                    try
                    {
                        IRow row = excelSheet.GetRow(i);

                        var CustomerId = row.GetCell(0).ToString();
                        var City = row.GetCell(1).ToString();
                        var Address = row.GetCell(2).ToString();
                        var Building = row.GetCell(3).ToString();
                        var Apartment = row.GetCell(4).ToString();
                        var GlassBroken = row.GetCell(5).ToString();
                        var ScratchedAluminum = row.GetCell(6).ToString();
                        var Other = row.GetCell(7).ToString();
                        var Description = row.GetCell(8).ToString();
                        var Sizes = row.GetCell(9).ToString().Split(',');

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }

            Assert.Pass();
        }

        public class ManagementDefect
        {
            public string CustomerId { get; set; }

            public string City { get; set; }
            public string Address { get; set; }

            public string Building { get; set; }

            public int? Apartment { get; set; }

            public bool GlassBroken { get; set; }

            public bool ScratchedAluminum { get; set; }

            public bool Other { get; set; }

            public string[] Sizes { get; set; }

            public string Description { get; set; }
        }
    }
}
