using System.ComponentModel.DataAnnotations;

namespace ECommerceApi.DTOs
{
    public class CategoryCreateDto
    {
        [Required(ErrorMessage = "Kategori adı zorunludur.")]
        [StringLength(100, ErrorMessage = "Kategori adı en fazla 100 karakter olmalıdır.")]
        public string Name { get; set; }
    }
}