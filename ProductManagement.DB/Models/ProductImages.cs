using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManagement.DB.Models
{
    public class ProductImages
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int ProductId { get; set; }
        public virtual Products Product { get; set; }
    }
}
