using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthStation_Test.Entities
{
   public class BookedHistory
    {
        [Key]
        public int ID { get; set; }

        public int BookID { get; set; }
        public int UserID { get; set; }
        public bool Status { get; set; }

        public DateTime BookDate { get; set; }

        public DateTime ReturnDate { get; set; }
        public DateTime ExpRet_Date { get; set; }
        public virtual Book Book { get; set; }


    }
}
