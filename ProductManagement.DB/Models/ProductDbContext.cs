using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManagement.DB.Models
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions options) : base(options)
        {
        }

       public virtual DbSet<Products> Products { get; set; }
       public virtual DbSet<ProductImages> ProductImages { get; set; }

    }
}
