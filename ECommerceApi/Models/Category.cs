using System.ComponentModel.DataAnnotations;

namespace ECommerceApi.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        // Navigation Property: Bir kategoriye ait birden fazla ürün olabilir.
        public ICollection<Product> Products { get; set; }
    }
}