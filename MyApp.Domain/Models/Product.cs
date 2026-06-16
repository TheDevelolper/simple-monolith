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
}