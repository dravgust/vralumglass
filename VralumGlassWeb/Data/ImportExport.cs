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
		public byte[] Export(IList<Customer> customers)
		{
			using (var fs = new MemoryStream())
			{
				var workbook = new XSSFWorkbook();
				ISheet excelSheet = workbook.CreateSheet("Customers");
				IRow row = excelSheet.CreateRow(0);

				row.CreateCell(0).SetCellValue("ID");
				row.CreateCell(1).SetCellValue("Name");


				for (var i = 0; i < customers.Count; i++)
				{
					var c = customers[i];
					row = excelSheet.CreateRow(i);
					row.CreateCell(0).SetCellValue(c.CustomerId);
					row.CreateCell(1).SetCellValue(c.Name);
				}

				workbook.Write(fs);

				return fs.ToArray();
			}
		}
	}
}
