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

        [Required, Display(Name = "Identity Number"), StringLength(32)]
        public string Identity { get; set; }

        [Required, Display(Name = "Name"), StringLength(128)]
        public string Name { get; set; }

        [Required, Display(Name = "Surname"), StringLength(128)]
        public string Surname { get; set; }

        [Required, Display(Name = "City"), StringLength(32)]
        public string City { get; set; }

        [Required, Display(Name = "Address"), StringLength(256)]
        public string Address { get; set; }

        [Required, Display(Name = "Building"), StringLength(3)]
        public string Building { get; set; }

        [Required, Display(Name = "Apartment")]
        public int Apartment { get; set; }

        [Required, Display(Name = "Age")]
        public int? Age { get; set; }

        [Required, Display(Name = "Email"), StringLength(128)]
        public string Email { get; set; }

        [Required, Display(Name = "Persons At Home")]
        public int? PersonsAtHome { get; set; }

        [Display(Name = "Key Received")]
        public DateTime KeyReceived { get; set; }

        [Display(Name = "Project Entrepreneur"), StringLength(64)]
        public string ProjectName { get; set; }

        [Display(Name = "Constructor"), StringLength(64)]
        public string Constructor { get; set; }

        public bool Subscribed { get; set; }
    }
}
