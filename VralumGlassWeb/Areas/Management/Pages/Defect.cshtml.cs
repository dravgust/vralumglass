using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Vralumglass.Core;
using VralumGlassWeb.Data;
using VralumGlassWeb.Data.Models;

namespace VralumGlassWeb.Areas.Management.Pages
{
    public class DefectModel : PageModel
    {
        private readonly IFileStorage _fileStorage;

        private readonly IEmailSender _emailSender;

        public DefectModel(IFileStorage fileStorage, IEmailSender emailSender)
        {
            _fileStorage = fileStorage;
            _emailSender = emailSender;
        }

        [BindProperty]
        public ManagementDefect Defect { get; set; }

        public void OnGet(string id)
        {
            Defect = new ManagementDefect
            {
				CustomerId = id,
                Sizes = new string[3]
            };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var folder = $"/{Defect.CustomerId.Substring(0, Defect.CustomerId.LastIndexOf('/'))}/Delivery";
            var fileName = "defects.xlsx";

            var ie = new ImportExport();
            var data = ie.Export(new List<ManagementDefect>{ Defect });

			var res1 = await _fileStorage.Upload(folder, fileName, data);

			return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnGetDownload(string id)
        {
	        var folder = $"/{id}";
	        var res1 = await _fileStorage.Download(folder, "drawing.pdf");

	        return File(res1, "application/pdf", "drawing.pdf");
        }
	}
}