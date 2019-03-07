using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VralumGlassWeb.Data;
using VralumGlassWeb.Data.Models;

namespace VralumGlassWeb.Pages
{
    public class SupportModel : PageModel
    {
	    private readonly ApplicationDbContext _db;

		public SupportModel(ApplicationDbContext db)
		{
			_db = db;
		}

		[BindProperty]
		public Customer Customer { get; set; }

        public async Task OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
	        if (!ModelState.IsValid)
	        {
		        return Page();
	        }

	        _db.Customers.Add(Customer);
	        await _db.SaveChangesAsync();
	        return RedirectToPage("/Index");
        }
    }
}