using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Vralumglass.Core;
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
                Sizes = new string[3]
            };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var d = Defect;
            await Task.Yield();
            return RedirectToPage("./Index");
        }
    }
}