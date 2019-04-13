using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Vralumglass.Core;
using VralumGlassWeb.Data;
using VralumGlassWeb.Data.Models;
using VralumGlassWeb.Pages;

namespace VralumGlassWeb.Areas.Management.Pages
{
	[Authorize]
    public class DefectModel : PageModel
    {
        private readonly IFileStorage _fileStorage;

        private readonly IEmailSender _emailSender;

        private readonly ILogger<SupportModel> _logger;

		public DefectModel(IFileStorage fileStorage, IEmailSender emailSender, ILogger<SupportModel> logger)
        {
            _fileStorage = fileStorage;
            _emailSender = emailSender;
            _logger = logger;
        }

        [BindProperty]
        public ManagementDefect Defect { get; set; }

		public IActionResult OnGet(string id)
        {
            if (!ProjectIdentity.TryParse(id, out var cIdentity))
            {
                return NotFound();
            }

            Defect = new ManagementDefect
            {
				CustomerId = id,

                City = cIdentity.City,
                Address = cIdentity.Address,
                Building = cIdentity.Building,
                Apartment = cIdentity.Apartment,

                Sizes = new string[3],
            };

			return Page();
		}

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ProjectIdentity.TryParse(Defect.CustomerId, out var cIdentity))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return RedirectToPage("Management/Defect", new
                {
                    id = Defect.CustomerId
                });
            }

            var deliveryFolder = $"/{cIdentity.City}/{cIdentity.Address}/{cIdentity.Building}/FirstDelivery";
            const string fileName = "DefectList.xlsx";

            var ie = new ImportExport();
            var defects = new List<ManagementDefect>();
            List<string> search = null;

            try
            {
                search = await _fileStorage.Search(deliveryFolder, fileName, 1);
                _logger.LogDebug($"search {fileName} => {search.Count}");
            }
            catch (Exception e)
            {
                _logger.LogDebug(e.Message);
            }

            if (search != null && search.Any())
            {
                var iData = await _fileStorage.Download(deliveryFolder, fileName);
                _logger.LogDebug($"download {fileName} => {iData.Length}");
                defects.AddRange(ie.Import(iData));

                _logger.LogDebug($"import {fileName} => {defects.ToJson()}");
            }

            defects.Add(Defect);
            var eData = ie.Export(defects);

            var res1 = await _fileStorage.Upload(deliveryFolder, fileName, eData);

            if (Defect.UploadPhoto != null)
            {
                var customerFolder =
                    $"/{cIdentity.City}/{cIdentity.Address}/{cIdentity.Building}/{cIdentity.Apartment}/FirstDelivery/{DateTime.Now:F}";
                using (var ms = new MemoryStream())
                {
                    await Defect.UploadPhoto.CopyToAsync(ms);
                    var res2 = await _fileStorage.Upload(customerFolder, "photo.jpg", ms.ToArray());
                }
            }

            var email = "dravgust@hotmail.com";
            var subject = "Work Manager Request";
            var body = $"Work manager: {Defect.CustomerId} sent defect.";

            await _emailSender.SendEmailAsync(email, subject, body);

            TempData["alerts"] = new List<string>{"Defect sent successfully."};

            return Redirect("~/Management/Defect?id=" + Defect.CustomerId);
		}

        public async Task<IActionResult> OnGetDownloadAsync(string id)
        {
            if (!ProjectIdentity.TryParse(id, out var cIdentity))
            {
                return NotFound();
            }

            var folder = $"/{id}";
	        var res1 = await _fileStorage.Download(folder, "drawing.pdf");

	        return File(res1, "application/pdf", "drawing.pdf");
        }
	}
}