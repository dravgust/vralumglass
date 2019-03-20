using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Vralumglass.Core;

namespace VralumGlassWeb.Pages
{
	public class IndexModel : PageModel
	{
	    private readonly ILogger<IndexModel> _logger;
	    private readonly IFileStorage _fileStorage;

	    public IndexModel(IFileStorage fileStorage, ILogger<IndexModel> logger)
	    {
		    _fileStorage = fileStorage;
	        _logger = logger;
	    }

		public async void OnGet()
		{
            _logger.LogInformation("INIT");
		}
	}
}
