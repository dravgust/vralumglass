using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VralumGlassWeb.Data.Models
{
    public class ProjectIdentity
    {
        public string City { private set; get; }

        public string Address { private set; get; }

        public string Building { private set; get; }

        public int Apartment { private set; get; }

        public bool IsValid { private set; get; }

        public ProjectIdentity()
        {

        }

        public static bool TryParse(string id, out ProjectIdentity cIdentity)
        {
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    var data = Uri.UnescapeDataString(id).Trim('/').Split('/');
                    if (data.Length == 4)
                    {
                        cIdentity = new ProjectIdentity
                        {
                            City = data[0],
                            Address = data[1],
                            Building = data[2],
                            Apartment = Convert.ToInt32(data[3]),

                            IsValid = true
                        };
                        
                        return true;
                    }
                }
                catch{}
            }

            cIdentity = null;
            return false;
        }
    }
}
