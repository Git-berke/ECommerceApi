using ECommerceApi.DTOs;
using ECommerceApi.Models; // Product modelini kullanacağız

namespace ECommerceApi.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto> GetProductByIdAsync(int id);
        Task<ProductDto> CreateProductAsync(ProductCreateDto productDto);
        Task<bool> UpdateProductAsync(int id, ProductUpdateDto productDto);
        Task<bool> DeleteProductAsync(int id);
        Task<bool> ProductExistsAsync(int id); // Ürünün var olup olmadığını kontrol eden yardımcı metod
    }
}