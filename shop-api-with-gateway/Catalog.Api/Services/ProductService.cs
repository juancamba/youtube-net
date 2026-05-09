using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Api.Services;
public interface IProductService
{
    Task<ProductDto?> GetByIdAsync(int id);
}
public class ProductService : IProductService
{
    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        return await Task.FromResult(
            new ProductDto(id, "Keyboard")
        );
    }
}
