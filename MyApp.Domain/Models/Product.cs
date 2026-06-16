namespace Domain.Models;

/// <summary>
/// Product domain entity represents a product from the business's perspective
/// </summary>
public record Product()
{
    public required int Id { get; init; }
    public required string Name { get; init; }
}