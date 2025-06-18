using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceApi.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } // Örneğin: Pending, Completed, Shipped, Cancelled

        // Navigation Property: Bir siparişin birden fazla sipariş öğesi olabilir.
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}