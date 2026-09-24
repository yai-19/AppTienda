using AutoMapper;
using AppTienda.Core.Application.Dtos.Category;
using AppTienda.Core.Application.Dtos.Expense;
using AppTienda.Core.Application.Dtos.Product;
using AppTienda.Core.Application.Dtos.Sale;
using AppTienda.Core.Domain.Entities;

namespace AppTienda.Core.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            #region Category
            CreateMap<Category, CategoryDto>()
                .ForMember(d => d.ProductsCount, opt => opt.MapFrom(s => s.Products != null ? s.Products.Count : 0));
            CreateMap<Category, SaveCategoryDto>().ReverseMap();
            #endregion

            #region Product
            CreateMap<Product, ProductDto>()
                .ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.Category != null ? s.Category.Name : "Sin Categoría"));
            CreateMap<Product, SaveProductDto>()
                .ForMember(d => d.ImageFile, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(d => d.Category, opt => opt.Ignore())
                .ForMember(d => d.Sales, opt => opt.Ignore());
            #endregion

            #region Sale
            CreateMap<Sale, SaleDto>()
                .ForMember(d => d.ProductName, opt => opt.MapFrom(s => s.Product != null ? s.Product.Name : "Desconocido"))
                .ForMember(d => d.ProductImageUrl, opt => opt.MapFrom(s => s.Product != null ? s.Product.ImageUrl : null))
                .ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.Product != null && s.Product.Category != null ? s.Product.Category.Name : "-"));
            CreateMap<SaveSaleDto, Sale>();
            #endregion

            #region Expense
            CreateMap<Expense, ExpenseDto>();
            CreateMap<Expense, SaveExpenseDto>().ReverseMap();
            #endregion
        }
    }
}
