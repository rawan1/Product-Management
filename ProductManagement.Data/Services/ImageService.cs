using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ProductManagement.Data.Interfaces;
using ProductManagement.DB.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Data.Services
{
    public class ImageService : IImageService
    {
        private string RootPath;
        private readonly ProductDbContext ProductContext;
        public ImageService(IWebHostEnvironment webHost, ProductDbContext _ProductContext)
        {
            RootPath = Path.Combine(webHost.ContentRootPath, "Resources");
            ProductContext = _ProductContext;
        }
        private string Time
        {
            get => DateTime.UtcNow.ToString("ddMyyHmmss");
        }
        public void DeleteImg(string imgPath)
        {
            var filePath = Path.Combine(RootPath, imgPath);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
        public void DeleteProductImg(int ImgId)
        {

            var productImg = ProductContext.ProductImages.FirstOrDefault(p => p.Id == ImgId);
            if (productImg != null)
            {
                DeleteImg(productImg.Url);
                ProductContext.ProductImages.Remove(productImg);
                ProductContext.SaveChanges();
            }
        }
        public async Task StoreImageAsync(IFormFile image, string filePath)
        {
            try
            {
                if (image.Length > 0)
                {
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await image.CopyToAsync(stream);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public string CreatePath(string ProductName, string extention)
        {
            var path = Path.Combine(RootPath, ProductName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var FullPath = Path.Combine(path, $"{Time}{extention}");

            return FullPath;
        }

    }
}
