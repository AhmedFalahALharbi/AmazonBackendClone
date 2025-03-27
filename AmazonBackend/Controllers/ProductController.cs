using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AmazonBackend.DTOs;
using AmazonBackend.Models;
using AmazonBackend.Repositories;
using Microsoft.EntityFrameworkCore;
using AmazonBackend.Data;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AmazonBackend.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly AppDbContext _context;

        public ProductController(IProductRepository productRepository, AppDbContext context)
        {
            _productRepository = productRepository;
            _context = context;
        }

        // Get all products - accessible to all users
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllProducts()
        {
            // Using EF Core for product listing
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }

        // Adds a new product to the database.
        [HttpPost]
        [Authorize(Roles = "Admin")] // Only Admins can add products
        public async Task<IActionResult> AddProduct([FromBody] ProductDto productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Stock = productDto.Stock
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return Ok("Product added successfully.");
        }

        // Updates an existing product in the database.
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")] // Only Admins can update products
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