using Data.Contracts;
using Data.Entities;

namespace Data.Repositories;

public class ProductRepository(ProductDbContext dbContext): IProductRepository
{
    public ProductEntity? GetProduct(int id)
    {
        return dbContext.Products.FirstOrDefault(p => p.Id == id);
    }

    public IQueryable<ProductEntity> GetProducts()
    {
        return dbContext.Products;
    }
}