using System.ComponentModel.DataAnnotations;

namespace ECommerceApi.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Username { get; set; }

        [Required]
        [StringLength(200)]
        public string Email { get; set; }

        // Şifreler hashlenip saklanacak. Şimdilik düz string olarak bırakıyoruz.
        [Required]
        public string PasswordHash { get; set; }

        // Navigation Property: Bir kullanıcının birden fazla sepet öğesi veya siparişi olabilir.
        public ICollection<CartItem> CartItems { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}