using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthStation_Test.Entities
{
   public class Book
    {
        [Key]
        public int ID { get; set; }
        public int CategoryID { get; set; }
        public string Title { get; set; }

        public string Author_Name { get; set; }
        public string ISBN { get; set; }

        public Decimal Price { get; set; }
        public int Quantity  { get; set; } 
        public DateTime Publisher_Date { get; set; }


        public virtual Category Category { get; set; }

        public virtual List<BookedHistory> Bookedhistories { get; set; }

    }
}
