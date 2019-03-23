using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VralumGlassWeb.Data.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required, StringLength(64)]
        public string CustomerId { get; set; }

        [Required, StringLength(32)]
        public string Identity { get; set; }

        [Required, StringLength(128)]
        public string Name { get; set; }

        [Required, StringLength(128)]
        public string Surname { get; set; }

        [Required, StringLength(256)]
        public string Address { get; set; }

        [Required, StringLength(32)]
        public string City { get; set; }

        [Required]
        public int? Age { get; set; }

        [Required, StringLength(128)]
        public string Email { get; set; }

        [Required]
        public int? PersonsAtHome { get; set; }

        public DateTime KeyReceived { get; set; }

        [StringLength(64)]
        public string ProjectName { get; set; }

        [StringLength(64)]
        public string Constructor { get; set; }

        public bool Subscribed { get; set; }
    }
}
