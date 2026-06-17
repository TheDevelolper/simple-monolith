using Data.Entities;
using MyApp.Shared;

namespace Data.Contracts;

public interface IProductRepository
{
    Result<ProductEntity> GetProduct(int id);
    Result<IQueryable<ProductEntity>> GetProducts();
    Task<Result<List<ProductEntity>>> AddProductsAsync(List<ProductEntity> entities);
}