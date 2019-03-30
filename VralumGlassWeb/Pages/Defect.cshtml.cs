using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Vralumglass.Core;
using VralumGlassWeb.Data.Models; 

namespace VralumGlassWeb.Pages
{
    public class RequestModel : PageModel
    {
        private readonly IFileStorage _fileStorage;

        private readonly IEmailSender _emailSender;

        private readonly SignInManager<IdentityUser> _signInManager;

        private readonly ILogger<SupportModel> _logger;

		public RequestModel(IFileStorage fileStorage, IEmailSender emailSender, SignInManager<IdentityUser> signInManager, ILogger<SupportModel> logger)
        {
            _fileStorage = fileStorage;
            _emailSender = emailSender;
            _signInManager = signInManager;
            _logger = logger;
        }

		[BindProperty]
        public CustomerDefect Defect { get; set; }

		public IActionResult OnGet(string id)
		{
            if (!ProjectIdentity.TryParse(id, out var cIdentity))
            {
                return NotFound();
            }

            if (_signInManager.IsSignedIn(User))
	        {
		        return Redirect("~/Management/Defect?id=" + id);
	        }

			Defect = new CustomerDefect
            {
                CustomerId = id
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
                return RedirectToPage("Defect", new
                {
                    id = Defect.CustomerId
                });
            }

            var folder = $"/{cIdentity.City}/{cIdentity.Address}/{cIdentity.Building}/{cIdentity.Apartment}/Defects/{DateTime.Now:F}";
            var res1 = await _fileStorage.Upload(folder, "description.txt",
                Encoding.UTF8.GetBytes(Defect.Description));

            using (var ms = new MemoryStream())
            {
                await Defect.UploadPhoto.CopyToAsync(ms);
                var res = await _fileStorage.Upload(folder, "photo.jpg", ms.ToArray());
            }

            var email = "dravgust@hotmail.com";
            var subject = "Customer Request";
            var body = $"customer: {Defect.CustomerId} sent defect.";

            await _emailSender.SendEmailAsync(email, subject, body);

            TempData["alerts"] = new List<string> { "Defect sent successfully." };

            return Redirect("~/Certificate?id=" + Defect.CustomerId);
		}
    }
}