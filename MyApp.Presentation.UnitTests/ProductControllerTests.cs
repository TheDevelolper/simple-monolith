using Domain.Contracts;
using Domain.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MyApp.Presentation.Controllers;
using MyApp.Presentation.Models.Api;

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
            .Returns(fakeDomainProduct);
        
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

}