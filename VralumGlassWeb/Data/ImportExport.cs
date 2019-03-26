using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using VralumGlassWeb.Data.Models;

namespace VralumGlassWeb.Data
{
	public class ImportExport
	{
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
				row.CreateCell(4).SetCellValue("Glass broken/cracked");
				row.CreateCell(5).SetCellValue("Scratched in aluminum");
				row.CreateCell(6).SetCellValue("Other");
				row.CreateCell(7).SetCellValue("Description");
				row.CreateCell(8).SetCellValue("Sizes");

				for (var i = 0; i < defects.Count; i++)
				{
					var c = defects[i];
					row = excelSheet.CreateRow(i);
					row.CreateCell(0).SetCellValue(c.CustomerId);
					row.CreateCell(1).SetCellValue(c.City);
					row.CreateCell(2).SetCellValue(c.Address);
					row.CreateCell(3).SetCellValue(c.Building);
					row.CreateCell(4).SetCellValue($"{c.GlassBroken}");
					row.CreateCell(5).SetCellValue($"{c.ScratchedAluminum}");
					row.CreateCell(6).SetCellValue($"{c.Other}");
					row.CreateCell(7).SetCellValue(c.Description);
					row.CreateCell(8).SetCellValue(string.Join(',', c.Sizes));
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
					row = excelSheet.CreateRow(i);
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
