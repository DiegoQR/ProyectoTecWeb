using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreAPI.Models
{
    public class BookModel
    {

        public int? Id { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public string Author { get; set; }
        public float? Price { get; set; }
        public string Description { get; set; }
        public int? QuantitySold { get; set; }
        public string ImagePath { get; set; }
        public int EditorialId { get; set; }
    }
}
