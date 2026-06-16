using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities;

[PrimaryKey(nameof(Id))]

public class ProductEntity
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(50)]
    public required string Name { get; init; }
}