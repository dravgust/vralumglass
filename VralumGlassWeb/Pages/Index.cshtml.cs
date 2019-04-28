using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Vralumglass.Core;
using VralumGlassWeb.Data.Models;

namespace VralumGlassWeb.Pages
{
	public class IndexModel : PageModel
	{
	    private readonly ILogger<IndexModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IStringLocalizer<IndexModel> _resources;

        [BindProperty]
        public ContactUsForm ContactUsForm { get; set; }

        public IndexModel(IStringLocalizer<IndexModel> resources, IEmailSender emailSender, ILogger<IndexModel> logger)
	    {
            _resources = resources;
            _emailSender = emailSender;
	        _logger = logger;
	    }

		public async void OnGet()
        {
            TempData["_welcome"] = $"res - {_resources["_welcome"]}";
            _logger.LogInformation("INIT");
		}

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var email = "dravgust@hotmail.com";
            await _emailSender.SendEmailAsync(email, ContactUsForm.Subject, $"{ContactUsForm.Name}\r\n{ContactUsForm.Phone}\r\n{ContactUsForm.Message}");

            TempData["alerts"] = new List<string> { "Message sent successfully." };

            return Redirect("~/");
        }

        public IActionResult OnPostSetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }
    }
}
