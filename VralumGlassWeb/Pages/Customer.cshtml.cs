using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VralumGlassWeb.Data;
using VralumGlassWeb.Data.Models;

namespace VralumGlassWeb.Pages
{
	[Authorize]
    public class CustomerModel : PageModel
    {
	    private readonly ApplicationDbContext _db;

	    public CustomerModel(ApplicationDbContext db)
	    {
		    _db = db;
	    }

		public IList<Customer> Customers { get; private set; }

        public async Task OnGetAsync()
        {
	        Customers = await _db.Customers.AsNoTracking().ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
	        var customer = await _db.Customers.FindAsync(id);

	        if (customer != null)
	        {
		        _db.Customers.Remove(customer);
		        await _db.SaveChangesAsync();
	        }

	        return RedirectToPage();
        }
    }
}