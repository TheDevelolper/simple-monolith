using Data.Entities;

namespace Domain.Models;

/// <summary>
/// Product domain entity represents a product from the business's perspective
/// </summary>
public record Product()
{
    public required int Id { get; init; }
    public required string Name { get; init; }

    /// <summary>
    /// Creates a domain product given a product entity.
    /// </summary>
    /// <param name="productEntity">The product entity to map</param>
    /// <returns></returns>
    public static Product From(ProductEntity productEntity)
    {
        var result = new Product()
        {
            Id = productEntity.Id,
            Name = productEntity.Name
        };
        
        return result;
    }

    /// <summary>
    /// Creates a product entity from a domain product.
    /// </summary>
    /// <param name="product">The domain product to map.</param>
    /// <returns>The mapped product entity.</returns>
    public static ProductEntity ToEntity(Product product)
    {
        var result = new ProductEntity()
        {
            Id = product.Id,
            Name = product.Name
        };

        return result;
    }
}