using AutoMapper;
using AppTienda.Core.Application.Dtos.Category;
using AppTienda.Core.Application.Interfaces;
using AppTienda.Core.Domain.Entities;
using AppTienda.Core.Domain.Interfaces;

namespace AppTienda.Core.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<List<CategoryDto>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetCategoriesWithProductsAsync();
            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ProductsCount = c.Products?.Count ?? 0
            }).ToList();
        }

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            var entity = await _categoryRepository.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<CategoryDto>(entity);
        }

        public async Task<SaveCategoryDto?> GetSaveByIdAsync(int id)
        {
            var entity = await _categoryRepository.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<SaveCategoryDto>(entity);
        }

        public async Task<SaveCategoryDto> AddAsync(SaveCategoryDto dto)
        {
            var entity = _mapper.Map<Category>(dto);
            var result = await _categoryRepository.AddAsync(entity);
            return _mapper.Map<SaveCategoryDto>(result);
        }

        public async Task UpdateAsync(SaveCategoryDto dto)
        {
            var entity = await _categoryRepository.GetByIdAsync(dto.Id);
            if (entity != null)
            {
                entity.Name = dto.Name;
                entity.Description = dto.Description;
                await _categoryRepository.UpdateAsync(entity);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _categoryRepository.GetByIdAsync(id);
            if (entity != null)
            {
                await _categoryRepository.DeleteAsync(entity);
            }
        }
    }
}
