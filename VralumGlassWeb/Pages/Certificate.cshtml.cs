using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VralumGlassWeb.Pages
{
    public class CertificateModel : PageModel
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public CertificateModel(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public void OnGet()
        {

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
    }
}