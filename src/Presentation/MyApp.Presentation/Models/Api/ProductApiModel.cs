using Domain.Models;

namespace MyApp.Presentation.Models.Api;

/// <summary>
/// Product domain entity represents a product from the API's perspective
/// </summary>
public class ProductApiModel()
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    
    /// <summary>
    /// Maps a domain's product into the API model's product representation
    /// </summary>
    /// <param name="domainProductApiModel">The domain model for the product to be mapped</param>
    /// <returns>An <see cref="ProductApiModel"/> derived from the domain <see cref="Product"/> input parameter</returns>
    public static ProductApiModel FromDomain(Product domainProductApiModel)
    {
        var result = new ProductApiModel()
        {
            Id = domainProductApiModel.Id,
            Name = domainProductApiModel.Name
        };

        return result;
    }
    
    /// <summary>
    /// Maps an api  product into the domain product model
    /// </summary>
    /// <param name="productApiModel">The api model for the product to be mapped</param>
    /// <returns>An <see cref="Product"/> model derived from the domain <see cref="ProductApiModel"/> input parameter</returns>
    public static Product ToDomain(ProductApiModel productApiModel)
    {
        var result = new Product()
        {
            Id = productApiModel.Id,
            Name = productApiModel.Name
        };

        return result;
    }
    
}

