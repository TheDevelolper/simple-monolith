using Data.Contracts;
using Data.Entities;
using Domain.Models;
using Domain.Services;
using Moq;

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
                .Returns(fakeProductEntity);
        
        var sut = new ProductService(mockProductRepository.Object);

        var product = sut.GetProduct(productId);
        
        Assert.NotNull(product);
        Assert.IsAssignableFrom<Product>(product);
        Assert.Equal(productId, product.Id);
    }
}