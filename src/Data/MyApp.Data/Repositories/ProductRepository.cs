using Data.Contracts;
using Data.Entities;
using MyApp.Shared;

namespace Data.Repositories;

public class ProductRepository(ProductDbContext dbContext): IProductRepository
{
    public Result<ProductEntity> GetProduct(int id)
    {
        //todo: can be made async
        var result = dbContext.Products.FirstOrDefault(p => p.Id == id);
        return result switch
        {
            not null => Result<ProductEntity>.Success(result),
            null => Result<ProductEntity>.Failure(ResultStatus.NotFound)
        };
    }

    public Result<IQueryable<ProductEntity>> GetProducts()
    {
        // todo can be made async
        return dbContext.Products switch
        {
            not null => Result<IQueryable<ProductEntity>>.Success(dbContext.Products),
            null => Result<IQueryable<ProductEntity>>.Failure(ResultStatus.Error, "Unable to retrieve products")
        };
    }

    public async Task<Result<List<ProductEntity>>> AddProductsAsync(List<ProductEntity> entities)
    {
        await dbContext.Products.AddRangeAsync(entities);
        await dbContext.SaveChangesAsync();
        
        return Result<List<ProductEntity>>.Success(entities);
    }

   
}