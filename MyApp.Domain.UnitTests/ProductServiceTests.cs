using Data.Contracts;
using Data.Entities;
using Domain.Models;
using Domain.Services;
using Moq;
using MyApp.Shared;

namespace MyApp.Domain.UnitTests;

public class ProductServiceTests
{
    [Fact]
    public void CanCreateProductService()
    {
        var mockProductRepository = new Mock<IProductRepository>();
        var sut = new ProductService(mockProductRepository.Object);
        Assert.NotNull(sut);
    }

    [Fact]
    public void CanGetAProductById()
    {
        var mockProductRepository = new Mock<IProductRepository>();
        const int productId = 1;

        var fakeProductEntity = new ProductEntity()
        {
            Id = productId,
            Name = "Test Product Entity",
        };
        
        mockProductRepository.Setup(m => m.GetProduct(productId))
                .Returns(Result<ProductEntity>.Success(fakeProductEntity));
        
        var sut = new ProductService(mockProductRepository.Object);
        var productResult = sut.GetProduct(productId);
        var product = productResult.Value;
        
        Assert.IsType<Result<Product>>(productResult);
        Assert.True(productResult.Status is ResultStatus.Success);
        
        Assert.NotNull(product);
        Assert.IsType<Product>(product);
        Assert.Equal(productId, product.Id);
    }
    
    [Fact]
    public void CorrectlyReturnsANotFoundResult()
    {
        var mockProductRepository = new Mock<IProductRepository>();
        var productId = 1234;
        mockProductRepository.Setup(m => m.GetProduct(productId))
            .Returns(Result<ProductEntity>.Failure(ResultStatus.NotFound));
        
        var sut = new ProductService(mockProductRepository.Object);
        var productResult = sut.GetProduct(productId);
        var product = productResult.Value;
        
        Assert.IsType<Result<Product>>(productResult);
        Assert.True(productResult.Status is ResultStatus.Success);
        
        Assert.NotNull(product);
        Assert.IsType<Product>(product);
        Assert.Equal(productId, product.Id);
    }

    [Fact]
    public async Task CanAddProducts()
    {
        var mockProductRepository = new Mock<IProductRepository>();

        var productsToAdd = new List<Product>
        {
            new() { Id = 1, Name = "Product 1" },
            new() { Id = 2, Name = "Product 2" },
        };

        var returnedEntities = productsToAdd.Select(Product.ToEntity).ToList();

        mockProductRepository.Setup(m => m.AddProductsAsync(It.IsAny<List<ProductEntity>>()))
            .ReturnsAsync(Result<List<ProductEntity>>.Success(returnedEntities));

        var sut = new ProductService(mockProductRepository.Object);
        var result = await sut.AddProductsAsync(productsToAdd);

        Assert.IsType<Result<List<Product>>>(result);
        Assert.True(result.Status is ResultStatus.Success);

        Assert.NotNull(result.Value);
        Assert.Equal(2, result.Value.Count);
        Assert.Equal(1, result.Value[0].Id);
        Assert.Equal("Product 1", result.Value[0].Name);
        Assert.Equal(2, result.Value[1].Id);
        Assert.Equal("Product 2", result.Value[1].Name);
    }

    [Fact]
    public async Task AddProductsReturnsFailureWhenRepositoryFails()
    {
        var mockProductRepository = new Mock<IProductRepository>();

        var productsToAdd = new List<Product>
        {
            new() { Id = 1, Name = "Product 1" },
        };

        mockProductRepository.Setup(m => m.AddProductsAsync(It.IsAny<List<ProductEntity>>()))
            .ReturnsAsync(Result<List<ProductEntity>>.Failure(ResultStatus.Error, "Database error"));

        var sut = new ProductService(mockProductRepository.Object);
        var result = await sut.AddProductsAsync(productsToAdd);

        Assert.IsType<Result<List<Product>>>(result);
        Assert.True(result.Status is ResultStatus.Error);
        Assert.Equal("Database error", result.ErrorMessage);
        Assert.Null(result.Value);
    }

}