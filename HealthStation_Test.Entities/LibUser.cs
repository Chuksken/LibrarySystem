using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthStation_Test.Entities
{
   public class LibUser : IdentityUser
    {
        public string FullName { get; set; }
        public string NIN { get; set; }

        public virtual List<BookedHistory> Bookedhistory { get; set; }
    }


}
