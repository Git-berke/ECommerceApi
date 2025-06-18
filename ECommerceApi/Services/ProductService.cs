using ECommerceApi.Data;
using ECommerceApi.DTOs;
using ECommerceApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceApi.Services
{
    public class ProductService : IProductService
    {
        private readonly ECommerceDbContext _context;

        // DbContext'i bağımlılık enjeksiyonu ile alıyoruz
        public ProductService(ECommerceDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            // İlişkili Category verisini de Include ile yüklüyoruz (eager loading)
            return await _context.Products
                .Include(p => p.Category)
                .Select(p => new ProductDto // Product modelini ProductDto'ya dönüştürüyoruz
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Stock = p.Stock,
                    ImageUrl = p.ImageUrl,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.Name // İlişkili kategori adını alıyoruz
                })
                .ToListAsync();
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return null;
            }

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                ImageUrl = product.ImageUrl,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name // Null check yapıyoruz
            };
        }

        public async Task<ProductDto> CreateProductAsync(ProductCreateDto productDto)
        {
            // CategoryId'nin geçerli olup olmadığını kontrol edebiliriz
            var categoryExists = await _context.Categories.AnyAsync(c => c.Id == productDto.CategoryId);
            if (!categoryExists)
            {
                // Hata yönetimi için buraya özel bir BusinessException fırlatılabilir
                throw new ArgumentException("Belirtilen kategori bulunamadı.");
            }

            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Stock = productDto.Stock,
                ImageUrl = productDto.ImageUrl,
                CategoryId = productDto.CategoryId,
                // Category nesnesi doğrudan atanmıyor, EF Core CategoryId üzerinden ilişkiyi kuracak
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet

            // Oluşturulan ürünü DTO'ya dönüştürüp döndürüyoruz
            // Kategori adını almak için tekrar yüklememiz veya Category nesnesini manuel oluşturmamız gerekebilir
            // Basitlik adına, şimdilik sadece temel DTO'yu döndürelim veya yeniden GetProductByIdAsync çağırabiliriz.
            // Gerçek bir senaryoda bu kısım daha optimize edilebilir.
            var createdProductDto = await GetProductByIdAsync(product.Id);
            return createdProductDto;
        }

        public async Task<bool> UpdateProductAsync(int id, ProductUpdateDto productDto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return false; // Ürün bulunamadı
            }

            // CategoryId'nin geçerli olup olmadığını kontrol et
            var categoryExists = await _context.Categories.AnyAsync(c => c.Id == productDto.CategoryId);
            if (!categoryExists)
            {
                throw new ArgumentException("Belirtilen kategori bulunamadı.");
            }

            // Modeldeki alanları DTO'dan gelen değerlerle güncelliyoruz
            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.Price = productDto.Price;
            product.Stock = productDto.Stock;
            product.ImageUrl = productDto.ImageUrl;
            product.CategoryId = productDto.CategoryId; // Kategori ID'si de güncellenebilir

            try
            {
                await _context.SaveChangesAsync(); // Değişiklikleri kaydet
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ProductExistsAsync(id))
                {
                    return false; // Başka bir kullanıcı tarafından silinmiş olabilir
                }
                else
                {
                    throw; // Diğer konkurensi hatalarını fırlat
                }
            }
            return true; // Başarıyla güncellendi
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return false; // Ürün bulunamadı
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true; // Başarıyla silindi
        }

        public async Task<bool> ProductExistsAsync(int id)
        {
            return await _context.Products.AnyAsync(e => e.Id == id);
        }
    }
}