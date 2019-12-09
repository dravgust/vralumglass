using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NPOI.HSSF.Record.Aggregates;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Vralumglass.Core.Models;
using VralumGlassWeb.Data.Models;
using VralumGlassWeb.Pages;

namespace VralumGlassWeb.Data
{
	public class ImportExport
	{
        public IList<ManagementDefect> Import(byte[] data)
        {
            var result = new List<ManagementDefect>();
            using (var ms = new MemoryStream(data))
            {
                var wb = new XSSFWorkbook(ms);
                ISheet excelSheet = wb.GetSheetAt(0);

                for (var i = 1; i <= excelSheet.LastRowNum; i++)
                {
                    try
                    {
                        IRow row = excelSheet.GetRow(i);
                        result.Add(new ManagementDefect
                        {
                            CustomerId = row.GetCell(0).ToString(),
                            City = row.GetCell(1).ToString(),
                            Address = row.GetCell(2).ToString(),
                            Building = row.GetCell(3).ToString(),
                            Apartment = int.Parse(row.GetCell(4).ToString()),
                            GlassBroken = bool.Parse(row.GetCell(5).ToString()),
                            ScratchedAluminum = bool.Parse(row.GetCell(6).ToString()),
                            Other = bool.Parse(row.GetCell(7).ToString()),
                            Description = row.GetCell(8).ToString(),
                            Sizes = row.GetCell(9).ToString().Split(',')
                        });
                    }
                    catch { }
                }
            }

            return result;
        }

        public IList<CoronaSnippet> ImportSnippets(byte[] data)
        {
            var result = new List<CoronaSnippet>();
            using (var ms = new MemoryStream(data))
            {
                var wb = new XSSFWorkbook(ms);
                ISheet excelSheet = wb.GetSheetAt(0);

                for (var i = 1; i <= excelSheet.LastRowNum; i++)
                {
                    try
                    {
                        IRow row = excelSheet.GetRow(i);
                        result.Add(new CoronaSnippet(float.Parse(row.GetCell(0).ToString()))
                        {
                            Apartment = row.GetCell(1).ToString(),
                            Floor = row.GetCell(2).ToString()
                        });
                    }
                    catch { }
                }
            }

            return result;
        }

        public byte[] Export(IList<ManagementDefect> defects)
        {
            using (var fs = new MemoryStream())
            {
                var workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Defects");
                IRow row = excelSheet.CreateRow(0);

                row.CreateCell(0).SetCellValue("ID");
                row.CreateCell(1).SetCellValue("City");
                row.CreateCell(2).SetCellValue("Address");
                row.CreateCell(3).SetCellValue("Building");
                row.CreateCell(4).SetCellValue("Apartment");
                row.CreateCell(5).SetCellValue("Glass broken/cracked");
                row.CreateCell(6).SetCellValue("Scratched in aluminum");
                row.CreateCell(7).SetCellValue("Other");
                row.CreateCell(8).SetCellValue("Description");
                row.CreateCell(9).SetCellValue("Sizes");

                for (var i = 0; i < defects.Count; i++)
                {
                    var c = defects[i];
                    row = excelSheet.CreateRow(i + 1);
                    row.CreateCell(0).SetCellValue(c.CustomerId);
                    row.CreateCell(1).SetCellValue(c.City);
                    row.CreateCell(2).SetCellValue(c.Address);
                    row.CreateCell(3).SetCellValue(c.Building);
                    row.CreateCell(4).SetCellValue($"{c.Apartment}");
                    row.CreateCell(5).SetCellValue($"{c.GlassBroken}");
                    row.CreateCell(6).SetCellValue($"{c.ScratchedAluminum}");
                    row.CreateCell(7).SetCellValue($"{c.Other}");
                    row.CreateCell(8).SetCellValue(c.Description);
                    row.CreateCell(9).SetCellValue(string.Join(',', c.Sizes));
                }

                workbook.Write(fs);

                return fs.ToArray();
            }
        }

        public byte[] Export(IList<Plank> planks, float free, int planReserve)
		{
			using (var fs = new MemoryStream())
			{
				var workbook = new XSSFWorkbook();
				ISheet excelSheet = workbook.CreateSheet("Planks");
				IRow row = excelSheet.CreateRow(0);

				row.CreateCell(0).SetCellValue("Plank Length");
				row.CreateCell(1).SetCellValue("Snippet [Apartment]");
				row.CreateCell(2).SetCellValue("Waste");

				for (var i = 0; i < planks.Count; i++)
				{
					var plank = planks[i];
					row = excelSheet.CreateRow(i + 1);
					row.CreateCell(0).SetCellValue($"{plank.OriginalLength + planReserve} ~ {plank.OriginalLength}");
					row.CreateCell(1).SetCellValue(string.Join(", ", plank.Cuts));
					row.CreateCell(2).SetCellValue(plank.FreeLength);
				}

				workbook.Write(fs);

				return fs.ToArray();
			}
		}

		public byte[] Export(IList<Customer> customers)
		{
			using (var fs = new MemoryStream())
			{
				var workbook = new XSSFWorkbook();
				ISheet excelSheet = workbook.CreateSheet("Customers");
				IRow row = excelSheet.CreateRow(0);

				row.CreateCell(0).SetCellValue("ID");
				row.CreateCell(1).SetCellValue("First Name");
			    row.CreateCell(2).SetCellValue("Last Name");
			    row.CreateCell(3).SetCellValue("Identity Number");
			    row.CreateCell(4).SetCellValue("Age");
			    row.CreateCell(5).SetCellValue("Email");
			    row.CreateCell(6).SetCellValue("Address");
			    row.CreateCell(7).SetCellValue("City");
			    row.CreateCell(8).SetCellValue("Persons As Home");
			    row.CreateCell(9).SetCellValue("Key Received");
			    row.CreateCell(10).SetCellValue("Project Entr.");
			    row.CreateCell(11).SetCellValue("Constructor");

                for (var i = 0; i < customers.Count; i++)
				{
					var c = customers[i];
					row = excelSheet.CreateRow(i + 1);
					row.CreateCell(0).SetCellValue(c.CustomerId);
					row.CreateCell(1).SetCellValue(c.Name);
				    row.CreateCell(2).SetCellValue(c.Surname);
				    row.CreateCell(3).SetCellValue(c.Identity);
				    row.CreateCell(4).SetCellValue($"{c.Age}");
				    row.CreateCell(5).SetCellValue(c.Email);
				    row.CreateCell(6).SetCellValue(c.Address);
				    row.CreateCell(7).SetCellValue(c.City);
				    row.CreateCell(8).SetCellValue($"{c.PersonsAtHome}");
				    row.CreateCell(9).SetCellValue(c.KeyReceived.ToString("dd/MM/yyyy"));
				    row.CreateCell(10).SetCellValue(c.ProjectName);
				    row.CreateCell(11).SetCellValue(c.Constructor);
                }

				workbook.Write(fs);

				return fs.ToArray();
			}
		}
	}
}
