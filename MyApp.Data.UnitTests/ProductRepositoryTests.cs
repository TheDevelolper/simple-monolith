using Data;
using Data.Entities;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using MyApp.Shared;

namespace MyApp.Data.UnitTests;

public class ProductRepositoryTests
{
    [Fact]
    public void CanCreateRepository()
    {
        var mockDbCtx = new Mock<ProductDbContext>();
        var productRepository = new ProductRepository(mockDbCtx.Object);
        Assert.NotNull(productRepository);
    }
    
    [Fact]
    public void CanGetProductById()
    {
        var fakeProduct1 = new ProductEntity
        {
            Id = 532,
            Name = "Product 1"
        };
        
        var fakeProduct2 = new ProductEntity
        {
            Id = 533,
            Name = "Product 2"
        };
        
        var options = new DbContextOptionsBuilder<ProductDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var fakeDbContext = new ProductDbContext(options);
        fakeDbContext.Products.Add(fakeProduct1);
        fakeDbContext.Products.Add(fakeProduct2);
        fakeDbContext.SaveChanges();
        
        var productRepository = new ProductRepository(fakeDbContext);
        
        // act
        var products = productRepository.GetProducts().Value;

        // assert
        Assert.NotNull(products);
        Assert.NotEmpty(products);
        Assert.Equal(2, products.Count());
        Assert.Equal(fakeProduct1, products.First());
        Assert.Equal(fakeProduct2, products.Last());
    }

    [Fact]
    public async Task CanAddProducts()
    {
        var productsToAdd = new List<ProductEntity>
        {
            new() { Id = 1, Name = "Product 1" },
            new() { Id = 2, Name = "Product 2" },
        };

        var options = new DbContextOptionsBuilder<ProductDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var fakeDbContext = new ProductDbContext(options);
        var productRepository = new ProductRepository(fakeDbContext);

        var result = await productRepository.AddProductsAsync(productsToAdd);

        Assert.IsType<Result<List<ProductEntity>>>(result);
        Assert.True(result.Status is ResultStatus.Success);

        Assert.NotNull(result.Value);
        Assert.Equal(2, result.Value.Count);
        Assert.Equal(1, result.Value[0].Id);
        Assert.Equal("Product 1", result.Value[0].Name);
        Assert.Equal(2, result.Value[1].Id);
        Assert.Equal("Product 2", result.Value[1].Name);

        Assert.Equal(2, fakeDbContext.Products.Count());
    }
}