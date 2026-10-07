using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechShop_API_backend_.Data;
using TechShop_API_backend_.Models;
using TechShop_API_backend_.Helpers;
using System.Security.Claims;

namespace TechShop_API_backend_.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductRepository _productRepository;
        private readonly ConverterHelper _converterHelper;

        public ProductController(ProductRepository productRepository, ConverterHelper converterHelper)
        {
            _productRepository = productRepository;
            _converterHelper = converterHelper;
        }




        [AllowAnonymous]
        [HttpGet("Fetch")]
        public async Task<IActionResult> GetListProducts(int number)
        {
            var isLogging = User.Identity?.IsAuthenticated == true;
            List<Product> products;

            if (isLogging)
            {
                products = await _productRepository.GetRandomProductAsync(number, null);
            }
            else
            {
                var categories = new List<string> { "Laptop", "Drones" };
                products = await _productRepository.GetRandomProductAsync(number, categories);
            }

            var productZip = _converterHelper.ConvertProductListToProductZipList(products);
            return Ok(productZip);
        }

        [AllowAnonymous]
        [HttpGet("Fetch/{category}/{number}")]
        public async Task<IActionResult> GetListProductsWithCategory(int number, string category)
        {
            var products = await _productRepository.GetByCategoryAsync(category);
            var productZip = _converterHelper.ConvertProductListToProductZipList(products);
            return Ok(productZip);
        }

        [AllowAnonymous]
        [HttpGet("All/Category")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _productRepository.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [AllowAnonymous]
        [HttpGet("Search/{keyword}")]
        public async Task<IActionResult> Search(string keyword)
        {
            var searchResultKeyword = await _productRepository.SearchAsync(keyword);
            var searchResultCategory = await _productRepository.GetByCategoryAsync(keyword);

            var combinedResults = searchResultKeyword
                .Concat(searchResultCategory)
                .Distinct()
                .ToList();

            var productZips = _converterHelper.ConvertProductListToProductZipList(combinedResults);
            return Ok(productZips);
        }

        [AllowAnonymous]
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> GetDetails(string id)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(id);
                if (product == null)
                {
                    return NotFound(new { message = "Product not found" });
                }

                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request", error = ex.Message });
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(id);
                if (product == null)
                {
                    return NotFound(new { message = "Product not found" });
                }

                await _productRepository.DeleteAsync(id);
                return Ok("The product has been deleted");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request", error = ex.Message });
            }
        }
    }
}
