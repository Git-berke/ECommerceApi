using ECommerceApi.Data;
using ECommerceApi.DTOs;
using ECommerceApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceApi.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ECommerceDbContext _context;

        public CategoryService(ECommerceDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            return await _context.Categories
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return null;
            }
            return new CategoryDto { Id = category.Id, Name = category.Name };
        }

        public async Task<CategoryDto> CreateCategoryAsync(CategoryCreateDto categoryDto)
        {
            var category = new Category
            {
                Name = categoryDto.Name
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return new CategoryDto { Id = category.Id, Name = category.Name };
        }

        public async Task<bool> CategoryExistsAsync(int id)
        {
            return await _context.Categories.AnyAsync(e => e.Id == id);
        }
    }
}