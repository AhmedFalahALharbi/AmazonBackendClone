using System.ComponentModel.DataAnnotations;

namespace AmazonBackend.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty; // ensures it's not null

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty; // This should be a hashed password

        [Required(ErrorMessage = "Role is required")]
        public UserRole Role { get; set; } // Only Customer or Admin are allowed

        // Navigation property: A user can have multiple orders
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
    public enum UserRole
    {
        Customer = 0,
        Admin = 1
    }