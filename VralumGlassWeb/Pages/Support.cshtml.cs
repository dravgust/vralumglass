using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VralumGlassWeb.Data;
using VralumGlassWeb.Data.Models;

namespace VralumGlassWeb.Pages
{
    public class SupportModel : PageModel
    {
	    private readonly ApplicationDbContext _db;
	    private readonly SignInManager<IdentityUser> _signInManager;
	    private readonly ILogger<SupportModel> _logger;

		public SupportModel(ApplicationDbContext db, SignInManager<IdentityUser> signInManager, ILogger<SupportModel> logger)
		{
			_db = db;
			_signInManager = signInManager;
			_logger = logger;
		}

		[BindProperty]
		public Customer Customer { get; set; }

        public async Task<IActionResult> OnGet(string id)
        {
	        if (string.IsNullOrEmpty(id))
	        {
		        return RedirectToPage("/Index");
	        }

	        if (_signInManager.IsSignedIn(User))
	        {
		        return Redirect("~/Request?id=" + id);
			}

			Customer = await _db.Customers.FirstOrDefaultAsync(c => c.CustomerId.Equals(id));

            if (Customer == null)
            {
                Customer = new Customer
                {
                    CustomerId = id
                };

                return Page();
            }
            else
            {
                return Redirect("~/Certificate?id=" + id);
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
	        if (!ModelState.IsValid)
	        {
		        return Page();
	        }

			var customer = await _db.Customers.FirstOrDefaultAsync(c => c.CustomerId.Equals(Customer.CustomerId));

			if (customer != null)
			{
				customer.Name = Customer.Name;
				_db.Attach(customer).State = EntityState.Modified;
			}
			else
			{
				_db.Attach(Customer).State = EntityState.Added;
			}

			try
			{
				await _db.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				throw new Exception($"Customer {Customer.Id} not found!");
			}

			return Redirect("~/Certificate?id=" + Customer.Id);
		}
    }
}