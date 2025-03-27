using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AmazonBackend.DTOs;
using AmazonBackend.Models;
using AmazonBackend.Repositories;
using System.Threading.Tasks;

namespace AmazonBackend.Controllers
{
    [Authorize(Roles = "Admin")] // Only Admins can add or update products
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // Adds a new product to the database.
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] ProductDto productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Stock = productDto.Stock
            };

            return Ok("Product added successfully.");
        }

        // Updates an existing product in the database.
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDto productDto)
        {
            var product = await _productRepository.GetProductById(id);
            if (product == null)
            {
                return NotFound("Product not found.");
            }

            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.Price = productDto.Price;
            product.Stock = productDto.Stock;

            return Ok("Product updated successfully.");
        }

        // Fetch a product by ID using Dapper
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _productRepository.GetProductById(id);
            if (product == null)
            {
                return NotFound("Product not found.");
            }
            return Ok(product);
        }
    }
}
