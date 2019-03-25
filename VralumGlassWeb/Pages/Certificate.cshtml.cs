using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using VralumGlassWeb.Data;
using VralumGlassWeb.Data.Models;

namespace VralumGlassWeb.Pages
{
    public class CertificateModel : PageModel
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<SupportModel> _logger;

        [BindProperty]
        public Customer Customer { get; set; }

        public CertificateModel(IHostingEnvironment hostingEnvironment, SignInManager<IdentityUser> signInManager, ILogger<SupportModel> logger)
        {
            _hostingEnvironment = hostingEnvironment;
            _signInManager = signInManager;
            _logger = logger;
        }

        public IActionResult OnGet(string id)
        {
            if (_signInManager.IsSignedIn(User))
            {
                return Redirect("~/Management/Defect?id=" + id);
            }

            Customer = new Customer
            {
                CustomerId = id
            };

            return Page();
        }

        public async Task<IActionResult> OnPostExport()
        {
            var sWebRootFolder = _hostingEnvironment.WebRootPath;
            const string sFileName = @"attachment.pdf";
            var file = Path.Combine(sWebRootFolder, "storage", sFileName);

            var memory = new MemoryStream();
            using (var fs = new FileStream(file, FileMode.Open))
            {
                await fs.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/pdf", "certificate.pdf");
        }

        public IActionResult OnPostRequest()
        {
            return Redirect("~/Request?id=" + Customer.CustomerId);
        }
    }
}