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
    /// <returns>An API product model derived from the domain <see cref="Product"/> input parameter</returns>
    internal static ProductApiModel? FromDomain(Product? domainProductApiModel)
    {
        if (domainProductApiModel == null) return null;
        
        var result = new ProductApiModel()
        {
            Id = domainProductApiModel.Id,
            Name = domainProductApiModel.Name
        };

        return result;
    }
}

