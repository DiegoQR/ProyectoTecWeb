using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreAPI.Data.Entities
{
    public class BookEntity
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public string Author { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public float? Price { get; set; }
        public string Description { get; set; }
        public int? QuantitySold { get; set; }
        public string ImagePath { get; set; }
        [ForeignKey("EditorialId")]
        public virtual EditorialEntity Editorial { get; set; }
    }
}
