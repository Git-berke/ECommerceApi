﻿using ECommerceApi.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceApi.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto> GetCategoryByIdAsync(int id);
        Task<CategoryDto> CreateCategoryAsync(CategoryCreateDto categoryDto);
        Task<bool> CategoryExistsAsync(int id); // Kategori var mı kontrolü için
    }
}