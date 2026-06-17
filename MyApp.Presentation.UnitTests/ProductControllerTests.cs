using Domain.Contracts;
using Domain.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MyApp.Presentation.Controllers;
using MyApp.Presentation.Models.Api;
using MyApp.Shared;

namespace MyApp.Presentation.UnitTests;

public class ProductControllerTests
{
    [Fact]
    public void CanBeConstructed()
    {
        var mockProductService = new Mock<IProductService>();
        
        // act
        var sut = new ProductsController(mockProductService.Object);
        
        // assert
        Assert.NotNull(sut);
    }
    
    /* CREATE */
    [Fact]
    public async Task CanCreateProducts()
    {
        var fakeDomainProducts = new List<Product>
        {
            new Product()
            {
                Id = 1,
                Name = "Test Product 1",
            },
            new Product()
            {
                Id = 2,
                Name = "Test Product 2",
            }
        };
        
        var mockProductService = new Mock<IProductService>();
        
        mockProductService.Setup(s => s.AddProductsAsync(fakeDomainProducts))
            .ReturnsAsync(Result<List<Product>>.Success(fakeDomainProducts));
        
        var apiProductsToCreate = fakeDomainProducts.Select(ProductApiModel.FromDomain).ToList();
        
        var sut = new ProductsController(mockProductService.Object);
        var actionResult = await sut.Post(apiProductsToCreate);
        var objectResult = actionResult.Result as CreatedResult;
        var result = objectResult?.Value as List<ProductApiModel> ?? [];
        
        mockProductService.Verify(s => s.AddProductsAsync(fakeDomainProducts), Times.Once);
        
        Assert.NotNull(result);
        Assert.IsType<List<ProductApiModel>>(result);
        Assert.Equal(fakeDomainProducts.Count, result.Count);
    }
    
    
    /* READ */
    [Fact]
    public void CanGetProductById()
    {
        var fakeDomainProduct = new Product()
        {
            Id = 1,
            Name = "Test Product",
        };
        
        var mockProductService = new Mock<IProductService>();

        mockProductService.Setup(moq => moq.GetProduct(1) )
            .Returns(Result<Product>.Success(fakeDomainProduct));
        
        var sut = new ProductsController(mockProductService.Object);
        
        // act 
        var controllerResult = sut.GetProductById(1);
        var apiProductResult = controllerResult.Result as OkObjectResult;
        var apiProduct = apiProductResult?.Value as ProductApiModel;
        
        // assert
        
        // service was called
        mockProductService.Verify(m => m.GetProduct(1), Times.Once);
        
        Assert.NotNull(apiProduct);  
        Assert.IsType<ProductApiModel>(apiProduct);        
        Assert.Equal(fakeDomainProduct.Id, apiProduct.Id);
    }
    
    [Fact]
    public void Returns404WhenProductDoesNotExist()
    {
        var mockProductService = new Mock<IProductService>();

        var productId = 1234;
        mockProductService.Setup(moq => moq.GetProduct(productId) )
            .Returns(Result<Product>.Failure(ResultStatus.NotFound));
        
        var sut = new ProductsController(mockProductService.Object);
        
        // act 
        var controllerResult = sut.GetProductById(1234);
        var apiProductResult = controllerResult.Result;
        
        // assert
        mockProductService.Verify(m => m.GetProduct(productId), Times.Once);
        Assert.IsType<NotFoundResult>(apiProductResult);  
    }

}