using Data;
using Data.Entities;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

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
        var products = productRepository.GetProducts();

        // assert
        Assert.NotNull(products);
        Assert.NotEmpty(products);
        Assert.Equal(2, products.Count());
        Assert.Equal(fakeProduct1, products.First());
        Assert.Equal(fakeProduct2, products.Last());
    }
}