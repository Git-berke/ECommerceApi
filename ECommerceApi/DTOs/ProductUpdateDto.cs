using System.ComponentModel.DataAnnotations;

namespace ECommerceApi.DTOs
{
    public class ProductUpdateDto
    {
        // Güncelleme işleminde ID zaten URL'den gelir, DTO'ya dahil etmeyiz.
        [Required(ErrorMessage = "Ürün adı zorunludur.")]
        [StringLength(200, ErrorMessage = "Ürün adı en fazla 200 karakter olmalıdır.")]
        public string Name { get; set; }

        [StringLength(1000, ErrorMessage = "Açıklama en fazla 1000 karakter olmalıdır.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Fiyat zorunludur.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Fiyat sıfırdan büyük olmalıdır.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stok miktarı zorunludur.")]
        [Range(0, int.MaxValue, ErrorMessage = "Stok miktarı negatif olamaz.")]
        public int Stock { get; set; }

        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Kategori ID'si zorunludur.")]
        public int CategoryId { get; set; }
    }
}