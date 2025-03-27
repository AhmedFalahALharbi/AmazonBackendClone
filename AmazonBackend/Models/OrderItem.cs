using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmazonBackend.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }

        [Required(ErrorMessage = "Order is required")]
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        [Required(ErrorMessage = "Product is required")]
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
    }
}
