using Domain.Models;
using MyApp.Shared;

namespace Domain.Contracts;

public interface IProductService
{
    /// <summary>
    /// Gets a product by id
    /// </summary>
    /// <param name="id">the id to look up</param>
    /// <returns>A product domain model</returns>
    Result<Product> GetProduct(int id);
    
    // /// <summary>
    // /// Creates products
    // /// </summary>
    // /// <param name="products">list of products to create</param>
    // /// <returns>a list of Ids for the created products</returns>
    // Result<List<int>> CreateProducts(List<Product> products);
    Task<Result<List<Product>>> AddProductsAsync(List<Product> products);
}

