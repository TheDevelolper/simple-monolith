using Data.Contracts;
using Domain.Contracts;
using Domain.Models;

namespace Domain.Services;

public class ProductService: IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository =  productRepository;
    }

    public Product GetProduct(int id)
    {
        var productEntity = _productRepository.GetProduct(id);
        var result = Product.From(productEntity);
        return result;
    }
    
}