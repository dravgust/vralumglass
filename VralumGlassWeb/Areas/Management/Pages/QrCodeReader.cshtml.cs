using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using VralumGlassWeb.Data.Models;
using VralumGlassWeb.Pages;

namespace VralumGlassWeb.Areas.Management.Pages
{
	[Authorize]
    public class QrCodeReaderModel : PageModel
    {
        private readonly ILogger<SupportModel> _logger;

		public QrCodeReaderModel(ILogger<SupportModel> logger)
        {
            _logger = logger;
        }

        [BindProperty]
        public ManagementDefect Defect { get; set; }

		public IActionResult OnGet()
        {
			return Page();
		}
	}
}