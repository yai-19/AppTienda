using AutoMapper;
using AppTienda.Core.Application.Dtos.Product;
using AppTienda.Core.Application.Interfaces;
using AppTienda.Core.Domain.Entities;
using AppTienda.Core.Domain.Enums;
using AppTienda.Core.Domain.Interfaces;

namespace AppTienda.Core.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<List<ProductDto>> GetAllAsync(int? categoryId = null, string? searchTerm = null)
        {
            var products = await _productRepository.GetProductsWithCategoryAsync();

            if (categoryId.HasValue && categoryId.Value > 0)
            {
                products = products.Where(p => p.CategoryId == categoryId.Value).ToList();
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.Trim().ToLower();
                products = products.Where(p => 
                    p.Name.ToLower().Contains(term) || 
                    (p.Description != null && p.Description.ToLower().Contains(term)) ||
                    (p.Sku != null && p.Sku.ToLower().Contains(term))
                ).ToList();
            }

            return _mapper.Map<List<ProductDto>>(products);
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var entity = await _productRepository.GetProductWithCategoryByIdAsync(id);
            return entity == null ? null : _mapper.Map<ProductDto>(entity);
        }

        public async Task<SaveProductDto?> GetSaveByIdAsync(int id)
        {
            var entity = await _productRepository.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<SaveProductDto>(entity);
        }

        public async Task<SaveProductDto> AddAsync(SaveProductDto dto, string? imageUrl = null)
        {
            var entity = _mapper.Map<Product>(dto);
            entity.CreatedAt = DateTime.Now;
            if (!string.IsNullOrEmpty(imageUrl))
            {
                entity.ImageUrl = imageUrl;
            }

            // Adjust status automatically according to stock
            if (entity.Stock <= 0)
            {
                entity.Status = ProductStatus.Agotado;
            }
            else
            {
                entity.Status = ProductStatus.Disponible;
            }

            var result = await _productRepository.AddAsync(entity);
            return _mapper.Map<SaveProductDto>(result);
        }

        public async Task UpdateAsync(SaveProductDto dto, string? imageUrl = null)
        {
            var entity = await _productRepository.GetByIdAsync(dto.Id);
            if (entity != null)
            {
                entity.Name = dto.Name;
                entity.Description = dto.Description;
                entity.Sku = dto.Sku;
                entity.PurchasePrice = dto.PurchasePrice;
                entity.SalePrice = dto.SalePrice;
                entity.Stock = dto.Stock;
                entity.CategoryId = dto.CategoryId;
                entity.UpdatedAt = DateTime.Now;

                if (!string.IsNullOrEmpty(imageUrl))
                {
                    entity.ImageUrl = imageUrl;
                }

                if (entity.Stock <= 0 && entity.Status == ProductStatus.Disponible)
                {
                    entity.Status = ProductStatus.Agotado;
                }
                else if (entity.Stock > 0 && entity.Status == ProductStatus.Agotado)
                {
                    entity.Status = ProductStatus.Disponible;
                }
                else
                {
                    entity.Status = dto.Status;
                }

                await _productRepository.UpdateAsync(entity);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _productRepository.GetByIdAsync(id);
            if (entity != null)
            {
                await _productRepository.DeleteAsync(entity);
            }
        }

        public async Task<List<ProductDto>> GetAvailableProductsAsync()
        {
            var products = await _productRepository.GetProductsWithCategoryAsync();
            var available = products.Where(p => p.Stock > 0 && p.Status == ProductStatus.Disponible).ToList();
            return _mapper.Map<List<ProductDto>>(available);
        }

        public async Task<List<ProductDto>> GetLowStockProductsAsync(int threshold = 3)
        {
            var products = await _productRepository.GetLowStockProductsAsync(threshold);
            return _mapper.Map<List<ProductDto>>(products);
        }

        public async Task UpdateStockAndStatusAsync(int productId, int quantitySold)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product != null)
            {
                product.Stock -= quantitySold;
                if (product.Stock <= 0)
                {
                    product.Stock = 0;
                    product.Status = ProductStatus.Vendido;
                }
                product.UpdatedAt = DateTime.Now;
                await _productRepository.UpdateAsync(product);
            }
        }
    }
}
