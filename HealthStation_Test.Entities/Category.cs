using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthStation_Test.Entities
{
   public class Category
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

        public virtual List<Book> Books{ get; set; }
    }
}
