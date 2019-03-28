using System;
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
			if (string.IsNullOrEmpty(id))
			{
				return RedirectToPage("./Index");
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
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var folder = $"/{Defect.CustomerId}/{DateTime.Now:F}";
            var res1 = await _fileStorage.Upload(folder, "description.txt",
                Encoding.UTF8.GetBytes(Defect.Description));

            using (var ms = new MemoryStream())
            {
                await Defect.UploadPhoto.CopyToAsync(ms);
                var res = await _fileStorage.Upload(folder, "photo.jpg", ms.ToArray());
            }

            var email = "dravgust@hotmail.com";
            var subject = "Customer Request";
            var body = $"customer: {Defect.CustomerId} request was sent.";

            await _emailSender.SendEmailAsync(email, subject, body);

			return Redirect("~/Certificate?id=" + Defect.CustomerId);
		}
    }
}