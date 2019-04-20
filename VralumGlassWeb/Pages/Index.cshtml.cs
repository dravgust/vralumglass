using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Vralumglass.Core;

namespace VralumGlassWeb.Pages
{
	public class IndexModel : PageModel
	{
	    private readonly ILogger<IndexModel> _logger;
	    private readonly IStringLocalizer<IndexModel> _resources;

        public IndexModel(IStringLocalizer<IndexModel> resources, ILogger<IndexModel> logger)
	    {
            _resources = resources;
	        _logger = logger;
	    }

		public async void OnGet()
        {
            TempData["_welcome"] = $"res - {_resources["_welcome"]}";
            _logger.LogInformation("INIT");
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
