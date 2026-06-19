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
        var options = new DbContextOptionsBuilder<ProductDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        
        using var fakeDbContext = new ProductDbContext(options);
        var productRepository = new ProductRepository(fakeDbContext);
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

        // assert repository response
        Assert.NotNull(result);
        Assert.IsType<Result<List<ProductEntity>>>(result);
        Assert.True(result.Status is ResultStatus.Success);
        
        var apiResponseValues = result.Value;
        var expectedProductCount = productsToAdd.Count;
        
        Assert.NotNull(apiResponseValues);
 
        Assert.Equal(expectedProductCount, result.Value.Count);
        Assert.Equal(productsToAdd.First().Id, result.Value.First().Id);
        Assert.Equal(productsToAdd.Last().Id, result.Value.Last().Id);
        Assert.Equal(productsToAdd.First().Name, result.Value.First().Name);
        Assert.Equal(productsToAdd.Last().Name, result.Value.Last().Name);

        // assert db contents
        var actualDbProductCount = fakeDbContext.Products.Count();
        
        Assert.True(fakeDbContext.Products.Any());
        Assert.Equal(expectedProductCount, actualDbProductCount);
        
        var actualDbProduct1 = await fakeDbContext.Products.FirstOrDefaultAsync(product => product.Id == productsToAdd.First().Id);
        var actualDbProduct2 = await fakeDbContext.Products.FirstOrDefaultAsync(product => product.Id == productsToAdd.Last().Id);

        Assert.NotNull(actualDbProduct1);
        Assert.NotNull(actualDbProduct2);
        Assert.IsType<ProductEntity>(actualDbProduct1);
        Assert.IsType<ProductEntity>(actualDbProduct2);

        Assert.Equal(productsToAdd.First().Name, actualDbProduct1.Name);
        Assert.Equal(productsToAdd.Last().Name, actualDbProduct2.Name);
    }
}