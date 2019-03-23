using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Vralumglass.Core;
using VralumGlassWeb.Data.Models; 

namespace VralumGlassWeb.Pages
{
    public class RequestModel : PageModel
    {
        private readonly IFileStorage _fileStorage;

        private readonly IEmailSender _emailSender;

        public RequestModel(IFileStorage fileStorage, IEmailSender emailSender)
        {
            _fileStorage = fileStorage;
            _emailSender = emailSender;
        }

        [BindProperty]
        public FileUpload FileUpload { get; set; }

        public void OnGet(string id)
        {
            FileUpload = new FileUpload
            {
                CustomerId = id
            };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var folder = $"/{FileUpload.CustomerId}/{DateTime.Now:F}";
            var res1 = await _fileStorage.Upload(folder, "description.txt",
                Encoding.UTF8.GetBytes(FileUpload.Description));

            using (var ms = new MemoryStream())
            {
                await FileUpload.UploadPhoto.CopyToAsync(ms);
                var res = await _fileStorage.Upload(folder, "photo.jpg", ms.ToArray());
            }

            var email = "dravgust@hotmail.com";
            var subject = "Customer Request";
            var body = $"customer: {FileUpload.CustomerId} request was sent.";

            await _emailSender.SendEmailAsync(email, subject, body);

            return RedirectToPage("./Index");
        }
    }
}