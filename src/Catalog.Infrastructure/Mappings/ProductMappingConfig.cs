using Catalog.Application.DTOs;
using Catalog.Domain.Products;
using Mapster;
namespace Catalog.Infrastructure.Mapping;
public static class ProductMappingConfig
{
    public static void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Product, ProductDto>();

        // you can fine-tune column mappings 
        //config.NewConfig<Product, ProductDto>()
        //    .Map(dest => dest.Name, src => src.Name)
        //    .Map(dest => dest.Sku, src => src.Sku)
        //    .Map(dest => dest.IsActive, src => src.IsActive);
    }
}

//public class ProductMappingConfig : IRegister
//{
//    public void Register(TypeAdapterConfig config)
//    {
//        config.NewConfig<Product, ProductDto>()
//            .Map(dest => dest.Name, src => src.ProductName)
//            .Map(dest => dest.Price, src => src.UnitPrice)
//            .Map(dest => dest.IsActive, src => src.IsEnabled);
//    }
//}



