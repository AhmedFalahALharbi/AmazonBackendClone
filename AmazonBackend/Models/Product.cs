using System.ComponentModel.DataAnnotations;

namespace AmazonBackend.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; } // Optional

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 99999.99, ErrorMessage = "Price must be between 0.01 and 99,999.99")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stock quantity is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be a positive number")]
        public int Stock { get; set; }

        // Navigation: A product can be in multiple order items
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
