using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VralumGlassWeb.Data.Models
{
    public class ManagementDefect
    {
		[Required]
		public string CustomerId { get; set; }

        [Required, StringLength(256)]
        public string Address { get; set; }

        [Required, StringLength(32)]
        public string City { get; set; }

        [Required, StringLength(3)]
        public string Building { get; set; }

        public bool GlassBroken { get; set; }

        public bool ScratchedAluminum { get; set; }

        public bool Other { get; set; }

        public string[] Sizes { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }
    }
}
