using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Data.Interfaces
{
    public interface IImageService
    {
        public void DeleteProductImg(int ImgId);
        public void DeleteImg(string imgPath);
        public Task StoreImageAsync(IFormFile image, string filePath);
        public string CreatePath(string ProductName, string extention);



    }
}
