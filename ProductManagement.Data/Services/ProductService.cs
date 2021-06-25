using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductManagement.Data.Interfaces;
using ProductManagement.DB.Models;
using ProductManagement.Data.Dtos;
using ProductManagement.Data.Filter;
using ProductManagement.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProductManagement.Data.Services
{
    public class ProductService : IProductService
    {
        private string RootPath;
        private readonly ProductDbContext  ProductContext;
        internal IImageService _imageService;
        private readonly ILogger<ProductService> _logger;
        public ProductService(IWebHostEnvironment webHost, ProductDbContext _ProductContext, ILogger<ProductService> _logger, IImageService _imageService)
        {
            RootPath = Path.Combine(webHost.ContentRootPath, "Resources");
            ProductContext = _ProductContext;
            this._logger = _logger;
            this._imageService = _imageService;
        }
        public bool AddProduct(ProductDto Product, IFormFileCollection images)
        {
            using var transaction = ProductContext.Database.BeginTransaction();
            try
            {
                Products AddedProd = ProductContext.Products.Add(new Products()
                {
                    Title = Product.Title,
                    Description = Product.Description,
                    Quantity = Product.Quantity,
                    Price = Product.Price,
                    AddedDate = DateTime.Now,
                }).Entity;
                ProductContext.SaveChanges();
                AddProductImages(images, AddedProd.Title, AddedProd.Id);
                transaction.Commit();
                return true;
            }
            catch (Exception e)
            {
                transaction.Rollback();
                _logger.LogInformation(e.Message);
                return false;

            }
        }
        private void AddProductImages(IFormFileCollection images,string Title, int ProductId)
        {
            if(images != null )
            foreach (var image in images)
            {
                var filePath = _imageService.CreatePath(Title, Path.GetExtension(image.FileName));
                _imageService.StoreImageAsync(image, filePath);
                ProductContext.ProductImages.Add(new ProductImages()
                {
                    ProductId = ProductId,
                    Url = filePath.Substring(RootPath.Length).Trim(Path.DirectorySeparatorChar)
                });
                ProductContext.SaveChanges();
            }
        }
        public void DeleteProduct(int ProductId)
        {
            if(ProductId == 0)
            {
                return;
            }
            var produc = ProductContext.Products.FirstOrDefault(p => p.Id == ProductId);
            if (produc != null)
            {
                var ProducImgs = ProductContext.ProductImages.Where(p => p.ProductId == produc.Id).ToList();
                foreach(var img in ProducImgs)
                {
                    _imageService.DeleteImg(img.Url);

                }
                ProductContext.ProductImages.RemoveRange(ProducImgs);
                ProductContext.SaveChanges();
                ProductContext.Products.Remove(produc);
                ProductContext.SaveChanges();
            }
            return;
        }
  

        public void UpdateProduct(int Id,ProductDto Product, IFormFileCollection images)
        {
            if (Id == 0)
            {
                return;
            }
            Products ProductModel = ProductContext.Products.FirstOrDefault(p => p.Id == Id);
            if (ProductModel != null)
            {
                ProductModel.Description = Product.Description;
                ProductModel.Price = Product.Price;
                ProductModel.Quantity = Product.Quantity;
                ProductContext.SaveChanges();
                AddProductImages(images,ProductModel.Title, ProductModel.Id);
            }
        }
        

        public  List<Products> GetAll(PaginationFilter validFilter)
        {
            var pagedData =  ProductContext.Products.OrderByDescending(p => p.AddedDate).Include(p => p.ImagesUrls)
               .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
               .Take(validFilter.PageSize)
               .ToList();

            return pagedData;
        }

        public Products GetById(int ProductId)
        {
            if(ProductId == 0)
            {
                return null;
            }
            var Product = ProductContext.Products.FirstOrDefault(p => p.Id == ProductId);
            if(Product != null)
            {
                Product.ImagesUrls = ProductContext.ProductImages.Where(p => p.ProductId == ProductId)?.ToList();
                return Product;
            }
            return null;
        }
        public int GetProductsNum()
        {
            return ProductContext.Products.Count();
        }
    }
}
