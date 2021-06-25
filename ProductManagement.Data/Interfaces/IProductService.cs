using Microsoft.AspNetCore.Http;
using ProductManagement.DB.Models;
using ProductManagement.Data.Dtos;
using ProductManagement.Data.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductManagement.Data.Interfaces
{
    public interface IProductService
    {
        bool AddProduct(ProductDto Product, IFormFileCollection images);
        public void UpdateProduct(int Id, ProductDto Product, IFormFileCollection images);
        public void DeleteProduct(int ProductId);
        public List<Products> GetAll(PaginationFilter filter);
        public Products GetById(int ProductId);
        public int GetProductsNum();
    }
}
