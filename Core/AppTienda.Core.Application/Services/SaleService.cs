using AutoMapper;
using AppTienda.Core.Application.Dtos.Sale;
using AppTienda.Core.Application.Interfaces;
using AppTienda.Core.Domain.Entities;
using AppTienda.Core.Domain.Enums;
using AppTienda.Core.Domain.Interfaces;

namespace AppTienda.Core.Application.Services
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public SaleService(ISaleRepository saleRepository, IProductRepository productRepository, IMapper mapper)
        {
            _saleRepository = saleRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<List<SaleDto>> GetAllAsync()
        {
            var sales = await _saleRepository.GetSalesWithProductAsync();
            return _mapper.Map<List<SaleDto>>(sales.OrderByDescending(s => s.SaleDate).ToList());
        }

        public async Task<SaleDto?> GetByIdAsync(int id)
        {
            var sales = await _saleRepository.GetSalesWithProductAsync();
            var sale = sales.FirstOrDefault(s => s.Id == id);
            return sale == null ? null : _mapper.Map<SaleDto>(sale);
        }

        public async Task<SaleDto> RegisterSaleAsync(SaveSaleDto dto)
        {
            var product = await _productRepository.GetByIdAsync(dto.ProductId);
            if (product == null)
            {
                throw new Exception("El producto seleccionado no existe.");
            }

            if (product.Stock < dto.Quantity)
            {
                throw new Exception($"Stock insuficiente. Solo quedan {product.Stock} unidad(es) disponible(s).");
            }

            // Calculations
            var unitCost = product.PurchasePrice;
            var unitPrice = dto.UnitPrice > 0 ? dto.UnitPrice : product.SalePrice;
            var totalCost = unitCost * dto.Quantity;
            var totalAmount = unitPrice * dto.Quantity;
            var profit = totalAmount - totalCost;

            var sale = new Sale
            {
                SaleDate = dto.SaleDate,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                UnitCost = unitCost,
                UnitPrice = unitPrice,
                TotalCost = totalCost,
                TotalAmount = totalAmount,
                Profit = profit,
                PaymentMethod = dto.PaymentMethod,
                CustomerName = dto.CustomerName,
                Notes = dto.Notes
            };

            var createdSale = await _saleRepository.AddAsync(sale);

            // Deduct stock and update status
            product.Stock -= dto.Quantity;
            if (product.Stock <= 0)
            {
                product.Stock = 0;
                product.Status = ProductStatus.Vendido;
            }
            product.UpdatedAt = DateTime.Now;
            await _productRepository.UpdateAsync(product);

            // Fetch with navigation properties to return clean DTO
            var fullSale = await _saleRepository.GetByIdAsync(createdSale.Id);
            return _mapper.Map<SaleDto>(createdSale);
        }

        public async Task<List<SaleDto>> GetSalesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var sales = await _saleRepository.GetSalesByDateRangeAsync(startDate, endDate);
            return _mapper.Map<List<SaleDto>>(sales.OrderByDescending(s => s.SaleDate).ToList());
        }

        public async Task DeleteAsync(int id)
        {
            var sale = await _saleRepository.GetByIdAsync(id);
            if (sale != null)
            {
                // Restore stock to product
                var product = await _productRepository.GetByIdAsync(sale.ProductId);
                if (product != null)
                {
                    product.Stock += sale.Quantity;
                    if (product.Status == ProductStatus.Vendido || product.Status == ProductStatus.Agotado)
                    {
                        product.Status = ProductStatus.Disponible;
                    }
                    await _productRepository.UpdateAsync(product);
                }

                await _saleRepository.DeleteAsync(sale);
            }
        }
    }
}
