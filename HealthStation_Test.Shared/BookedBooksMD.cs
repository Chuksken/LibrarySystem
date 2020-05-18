using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthStation_Test.WEBAPI.Models
{
    public class BookedBooksMD
    {
        
        public string Title { get; set; }

        public string ISBN { get; set; }

        public Decimal Price { get; set; }

        public DateTime Publisher_Date { get; set; }

        public bool status { get; set; }



    }
}