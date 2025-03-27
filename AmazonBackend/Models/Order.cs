using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmazonBackend.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        [Required(ErrorMessage = "User is required")]
        public int UserId { get; set; }
        public User User { get; set; } = null!; // Foreign key relation

        [Required(ErrorMessage = "Total amount is required")]
        [Column(TypeName = "decimal(18,2)")] // Ensures decimal precision in the database
        public decimal TotalAmount { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow; // Default to current date

        // Navigation: An order can have multiple order items
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
