using Data.Contracts;
using Domain.Contracts;
using Domain.Models;
using MyApp.Shared;

namespace Domain.Services;

public class ProductService: IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository =  productRepository;
    }

    public Result<Product> GetProduct(int id)
    {
        var entityResult = _productRepository.GetProduct(id);

        switch (entityResult.Status)
        {
            case ResultStatus.Success when entityResult.Value is not null:
                // todo add logging here
                return Result<Product>.Success(Product.From(entityResult.Value));
            case ResultStatus.Success when entityResult.Value is null:
                // todo this ABSOLUTELY must be logged as an error, should never happen.
                return Result<Product>.Failure(ResultStatus.Error);
            
            default:
                    // This will do for now, if domain logic becomes more complex.
                    // I'll need to think more about how to report back domain specific messages if needed. 
                    // todo add logging here 
                return Result<Product>.Failure(entityResult.Status, entityResult.ErrorMessage);
        }
       
    }

    public async Task<Result<List<Product>>> AddProductsAsync(List<Product> products)
    {
        var productEntities = products.Select(Product.ToEntity).ToList();
        var entityResult = await _productRepository.AddProductsAsync(productEntities);
        
        var domainProducts = entityResult.Value?.Select(Product.From).ToList();
        
        return new Result<List<Product>>(entityResult.Status, domainProducts ,entityResult.ErrorMessage);
    }
}