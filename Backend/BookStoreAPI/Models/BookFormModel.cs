using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreAPI.Models
{
    public class BookFormModel : BookModel
    {
        public IFormFile Image { get; set; }
        public string PriceForm { get; set; }
    }
}
