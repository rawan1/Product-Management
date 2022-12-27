using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; 
namespace ProductManagement.DB.Models
{
    public class ProductDbContext : IdentityUserContext<IdentityUser>
    {
        public ProductDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<ProductImages> ProductImages { get; set; }

    }
}
