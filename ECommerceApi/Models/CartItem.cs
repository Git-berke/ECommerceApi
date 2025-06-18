using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceApi.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } // Hangi kullanıcıya ait

        public int ProductId { get; set; }
        public Product Product { get; set; } // Hangi ürün

        [Required]
        public int Quantity { get; set; } // Kaç adet

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; } // Sepete eklendiği anki ürün fiyatı (fiyat değişimi ihtimaline karşı)
    }
}