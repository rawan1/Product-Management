using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProductManagement.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductManagement.Data.Interfaces;
using ProductManagement.Data.Filter;
using ProductManagement.Data.Dtos;
using ProductManagement.Data.Wrapper;

namespace ProductManagement.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IImageService _imageService;

        private readonly IProductService _productService;
        public ProductsController(IProductService productService ,IImageService imageService)
        {
            _productService = productService;
            _imageService = imageService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var products =  _productService.GetAll(validFilter);
            var totalNum  = _productService.GetProductsNum();
            return Ok(new PagedResponse<List<Products>>(products, validFilter.PageNumber, validFilter.PageSize, totalNum));

        }

        [HttpGet("{id}")]
        public Products Get(int id)
        {
            return _productService.GetById(id);
        }

        [HttpPost]
        public ActionResult Post(IFormCollection data)
        {
            var details = data["details"];
            ProductDto ProductDetails = JsonConvert.DeserializeObject<ProductDto>(details);
            var res = _productService.AddProduct(ProductDetails, data.Files);
            if( res)
            {
                return Ok(new Response<string>("Added successfully"));
            }
            else
            {
                return BadRequest("Invalid input");
            }

        }

        [HttpPut("{id}")]
        public void Put(int id, IFormCollection data)
        {
            var details = data["details"];
            ProductDto ProductDetails = JsonConvert.DeserializeObject<ProductDto>(details);

            _productService.UpdateProduct(id,ProductDetails, data.Files);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _productService.DeleteProduct(id);
        }
        [HttpDelete("images/{id}")]
        public void DeleteProductImg(int id)
        {
            _imageService.DeleteProductImg(id);
        }
    }
}
