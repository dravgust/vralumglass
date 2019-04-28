using System.ComponentModel.DataAnnotations;

namespace VralumGlassWeb.Data.Models
{
    public class ContactUsForm
    {
        [Required, Display(Name = "Name"), StringLength(20)]
        public string Name { get; set; }

        [Required, Display(Name = "Phone"), StringLength(20)]
        public string Phone { get; set; }

        [Required, Display(Name = "Subject"), StringLength(128)]
        public string Subject { get; set; }

        [Required, Display(Name = "Message"), StringLength(500)]
        public string Message { get; set; }
    }
}
