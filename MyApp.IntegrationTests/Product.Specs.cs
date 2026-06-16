using System.Net;
using System.Net.Http.Json;
using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TestStack.BDDfy;

namespace MyApp.Integration.Tests;

public class ProductApiModelSpecs(DbIntegrationTestFixture fixture) : IClassFixture<DbIntegrationTestFixture>
{
    private HttpResponseMessage? _response;

    [Fact]
    public Task CanGetAProductById()
    {
        var exampleProducts = new[]
        {
            new ProductEntity
            {
                Name = "Example Product 1",
            },
            new ProductEntity
            {
                Name = "Example Product 2",
            }
        };
        
        this.Given(s => s.TheProductsAreAddedToTheDatabase(exampleProducts))
            .When(s => s.TheProductIsRequestedById(exampleProducts.First().Id))
            .Then(s => s.TheResponseStatusCodeShouldBe(HttpStatusCode.OK))
            .And(s => s.TheProductIsReturned(exampleProducts.First()))
            .BDDfy();
        
        return Task.CompletedTask;
    }
    
    [Fact]
    public Task Returns404WhenProductDoesNotExist()
    {
        var exampleProducts = Array.Empty<ProductEntity>();
        
        this.Given(s => s.TheProductsAreAddedToTheDatabase(exampleProducts))
            .When(s => s.TheProductIsRequestedById(1))
            .Then(s => s.TheResponseStatusCodeShouldBe(HttpStatusCode.NotFound))
            .BDDfy();
        
        return Task.CompletedTask;
    }
    
    private async Task TheProductsAreAddedToTheDatabase(IEnumerable<ProductEntity> products)
    {
        using var scope = fixture.WebAppFactory.Services.CreateScope();
        var dbCtx = scope.ServiceProvider.GetRequiredService<ProductDbContext>();
        await dbCtx.Database.EnsureCreatedAsync();
        await dbCtx.Products.ExecuteDeleteAsync(); // ensure we're starting with clean entities
        await dbCtx.Products.AddRangeAsync(products);
        await dbCtx.SaveChangesAsync();
    }

    private async Task TheProductIsRequestedById(int productId)
    {   
        var http = fixture.WebAppTestHttpClient ?? throw new NullReferenceException("Could not create the HttpClient");
        _response = await http.GetAsync(http.BaseAddress + $"api/products/{productId}");
    }

    private Task TheResponseStatusCodeShouldBe(HttpStatusCode statusCode)
    {
        Assert.NotNull(_response);
        Assert.True(_response.StatusCode == statusCode);
        return Task.CompletedTask;
    }
    
    private async Task TheProductIsReturned(ProductEntity exampleProduct)
    {
        var content = _response?.Content ??  throw new NullReferenceException("Could not create the HttpClient");
        var product = await content.ReadFromJsonAsync<ProductEntity>();

        Assert.NotNull(product);
        Assert.Equal(exampleProduct.Id, product.Id);
        Assert.Equal(exampleProduct.Name, product.Name);
    }
}