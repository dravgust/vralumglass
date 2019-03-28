using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace VralumGlassWeb.Data.Models
{
    public class ManagementDefect
    {
		[Required]
		public string CustomerId { get; set; }

        [Required, Display(Name = "Address"), StringLength(256)]
        public string Address { get; set; }

        [Required, Display(Name = "City"), StringLength(32)]
        public string City { get; set; }

        [Required, Display(Name = "Building"), StringLength(3)]
        public string Building { get; set; }

        public bool GlassBroken { get; set; }

        public bool ScratchedAluminum { get; set; }

        public bool Other { get; set; }

        public string[] Sizes { get; set; }

        [Display(Name = "Description"), StringLength(2000)]
        public string Description { get; set; }

        [Display(Name = "Photo")]
        public IFormFile UploadPhoto { get; set; }
	}
}
