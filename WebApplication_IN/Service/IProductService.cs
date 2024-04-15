using Microsoft.AspNetCore.Mvc;
using WebApplication_IN.Enums;
using WebApplication_IN.Models;

namespace WebApplication_IN.Service
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetCollectionV1Async(ProductCollectionModel filter);
        Task<IEnumerable<Product>> GetCollectionV2Asyncstring(string categoryName, int page = 1, int pageSize = 10);
        Task<ActionResult<Product>> GetProductByIdAsync(int id);
        Task UpdateProductAsync(Product product);
        Task<ActionResult<Product>> CreateProductAsync(Product product);
        Task DeleteProductAsync(int id);
    }
}
