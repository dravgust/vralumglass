using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VralumGlassWeb.Data;
using VralumGlassWeb.Data.Models;

namespace VralumGlassWeb.Areas.Management.Pages
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
            throw new Exception("test");
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

        public async Task<IActionResult> OnPostExport()
        {
	        var ie = new ImportExport();

	        var customers = await _db.Customers.ToListAsync();

			var data = ie.Export(customers);

			return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Customers.xlsx");
        }
    }
}