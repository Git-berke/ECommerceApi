using ECommerceApi.DTOs;
using ECommerceApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceApi.Controllers
{
    [Route("api/[controller]")] // API URL yolu: /api/Products
    [ApiController] // Bu sınıfın bir API kontrolcüsü olduğunu belirtir
    public class ProductsController : ControllerBase // API kontrolcüleri ControllerBase'den türetilir
    {
        private readonly IProductService _productService;

        // IProductService'i bağımlılık enjeksiyonu ile alıyoruz
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products); // 200 OK yanıtı ile ürün listesini döndür
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound(); // 404 Not Found yanıtı
            }

            return Ok(product); // 200 OK yanıtı ile tek ürünü döndür
        }

        // POST: api/Products
        // Body'den ProductCreateDto alır
        [HttpPost]
        public async Task<ActionResult<ProductDto>> PostProduct(ProductCreateDto productDto)
        {
            if (!ModelState.IsValid) // DTO üzerindeki doğrulama kurallarını kontrol et
            {
                return BadRequest(ModelState); // Geçersizse 400 Bad Request döndür
            }

            try
            {
                var createdProduct = await _productService.CreateProductAsync(productDto);
                // 201 Created yanıtı: Yeni bir kaynak oluşturulduğunu belirtir
                // CreatedAtAction: Yeni kaynağın URL'sini de içeren bir yanıt başlığı ekler
                return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
            }
            catch (ArgumentException ex) // Servisten fırlattığımız özel hata
            {
                return BadRequest(ex.Message); // Kategori bulunamadı gibi hatalar için
            }
        }

        // PUT: api/Products/5
        // Body'den ProductUpdateDto alır
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, ProductUpdateDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // ID'lerin uyumlu olup olmadığını kontrol et (genellikle DTO içinde ID olmaz, URL'den alınır)
            // Bu örnekte ProductUpdateDto'da ID olmadığı için bu kontrol gereksiz.
            // if (id != productDto.Id) { return BadRequest(); } // Eğer DTO'da Id olsaydı

            try
            {
                var result = await _productService.UpdateProductAsync(id, productDto);
                if (!result)
                {
                    return NotFound(); // Ürün bulunamadıysa 404
                }
                return NoContent(); // 204 No Content yanıtı: Güncelleme başarılı, içerik döndürülmüyor
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // Kategori bulunamadı gibi hatalar için
            }
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            if (!result)
            {
                return NotFound(); // Ürün bulunamadıysa 404
            }
            return NoContent(); // 204 No Content yanıtı: Silme başarılı, içerik döndürülmüyor
        }
    }
}