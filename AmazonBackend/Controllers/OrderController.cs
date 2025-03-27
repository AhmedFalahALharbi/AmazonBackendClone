using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AmazonBackend.Data;
using AmazonBackend.DTOs;
using AmazonBackend.Models;
using AmazonBackend.Repositories; 
using System.Security.Claims;
using System.Threading.Tasks;

namespace AmazonBackend.Controllers
{
    [Route("api/orders")]
    [ApiController]
    [Authorize]  // Only authenticated users can place/view orders
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IOrderRepository _orderRepository; // Injecting Dapper Repo

        public OrderController(AppDbContext context, IOrderRepository orderRepository)
        {
            _context = context;
            _orderRepository = orderRepository;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto orderDto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                return Unauthorized(new { error = "Invalid user ID in token." });

            // Fetch all products at once to minimize database queries
            var productIds = orderDto.OrderItems.Select(item => item.ProductId).ToList();
            var products = await _context.Products
                                         .Where(p => productIds.Contains(p.ProductId))
                                         .ToDictionaryAsync(p => p.ProductId);

            if (products.Count != productIds.Count)
                return BadRequest(new { error = "Some products were not found." });

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = 0,
                OrderItems = new List<OrderItem>() // Initialize the list
            };

            foreach (var itemDto in orderDto.OrderItems)
            {
                if (!products.TryGetValue(itemDto.ProductId, out var product))
                    return BadRequest(new { error = $"Product with ID {itemDto.ProductId} not found." });

                var orderItem = new OrderItem
                {
                    ProductId = product.ProductId,
                    Quantity = itemDto.Quantity,
                    Price = product.Price
                };

                order.OrderItems.Add(orderItem);
                order.TotalAmount += orderItem.Quantity * orderItem.Price;
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Order created successfully.", orderId = order.OrderId });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                return Unauthorized(new { error = "Invalid user ID in token." });

            // Using EF Core eager loading to fetch order with related entities
            var order = await _context.Orders
                .Where(o => o.OrderId == id && o.UserId == userId)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.User)
                .FirstOrDefaultAsync();

            if (order == null)
                return NotFound(new { error = "Order not found or unauthorized access." });

            return Ok(order);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserOrders()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                return Unauthorized(new { error = "Invalid user ID in token." });

            // Using EF Core eager loading to fetch orders with related entities
            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return Ok(orders);
        }
    }
}