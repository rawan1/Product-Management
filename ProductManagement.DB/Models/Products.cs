using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManagement.DB.Models
{

    [Index(nameof(Title), IsUnique = true)]
    public class Products
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public DateTime AddedDate { get; set; }
        public virtual IEnumerable<ProductImages> ImagesUrls { get; set; }

    }
}
