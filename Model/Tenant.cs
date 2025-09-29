using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reolmarked.Model
{
    namespace Reolmarked.Model
    {
        // Tenant class repræsenterer en lejer
        public class Tenant
        {
            public int TenantId { get; set; }        // Unikt ID for lejer
            public string TenantName { get; set; }   // Navn på lejer
            public string TenantEmail { get; set; }  // Email
            public string TenantPhone { get; set; }  // Telefonnummer
        }
    }

}
