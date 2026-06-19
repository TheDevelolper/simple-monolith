using System.Net;
using System.Net.Http.Json;
using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyApp.Presentation.Models.Api;
using TestStack.BDDfy;

namespace MyApp.Integration.Tests;

public class ProductApiModelSpecs(DbIntegrationTestFixture fixture) : IClassFixture<DbIntegrationTestFixture>
{
    private HttpResponseMessage? _response;
    
    // CREATE
    [Fact]
    public Task CanCreateProducts()
    {
        var exampleProducts = new[]
        {
            new ProductEntity
            {
                Name = "Example Product 2",
            },
            new ProductEntity
            {
                Name = "Example Product 3",
            }
        };
        
        this.Given(s => s.TheDatabaseIsEmpty())
            .When(s => s.TheProductsAreCreatedViaApi(exampleProducts))
            .Then(s => s.TheResponseStatusCodeShouldBe(HttpStatusCode.Created))
            .BDDfy();
        
        return Task.CompletedTask;
    }
    
    
    // READ
    [Fact]
    public Task CanGetAProductById()
    {
        var exampleProducts = new[]
        {
            new ProductEntity
            {
                Name = "Example Product 2",
            },
            new ProductEntity
            {
                Name = "Example Product 3",
            }
        };
        
        this.Given(s => s.TheDatabaseIsEmpty())
            .When(s => s.TheProductsAreAddedToTheDatabase(exampleProducts))
            .When(s => s.TheProductIsRequestedByIdViaApi(exampleProducts.First().Id))
            .Then(s => s.TheResponseStatusCodeShouldBe(HttpStatusCode.OK))
            .And(s => s.TheProductIsReturned(exampleProducts.First()))
            .BDDfy();
        
        return Task.CompletedTask;
    }

    [Fact]
    public Task Returns404WhenProductDoesNotExist()
    {
        this.Given(s => s.TheDatabaseIsEmpty())
            .When(s => s.TheProductIsRequestedByIdViaApi(1))
            .Then(s => s.TheResponseStatusCodeShouldBe(HttpStatusCode.NotFound))
            .BDDfy();
        
        return Task.CompletedTask;
    }
    
    
    /* STEP DEFINITIONS */
    private async Task TheDatabaseIsEmpty()
    {
        using var scope = fixture.WebAppFactory.Services.CreateScope();
        var dbCtx = scope.ServiceProvider.GetRequiredService<ProductDbContext>();
        await dbCtx.Database.EnsureCreatedAsync();
        await dbCtx.Products.ExecuteDeleteAsync(); // Delete everything
        await dbCtx.SaveChangesAsync();
    }
    
    private async Task TheProductsAreAddedToTheDatabase(IEnumerable<ProductEntity> products)
    {
        using var scope = fixture.WebAppFactory.Services.CreateScope();
        var dbCtx = scope.ServiceProvider.GetRequiredService<ProductDbContext>();
        await dbCtx.Database.EnsureCreatedAsync();
        await dbCtx.Products.AddRangeAsync(products);
        await dbCtx.SaveChangesAsync();
    }
    
    private async Task TheProductsAreCreatedViaApi(ProductEntity[] products)
    {
        var http = fixture.WebAppTestHttpClient;
        
        // design decision: This is a manual mapping, but it will only ever happen in test
        // as we do not map directly between entity and api elsewhere.
        var apiProducts =
            products
                .Select(entity => new ProductApiModel()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                });

        _response = await http.PostAsJsonAsync(http.BaseAddress + $"api/products/",  apiProducts);
    }

    
    private async Task TheProductIsRequestedByIdViaApi(int productId)
    {
        var http = fixture.WebAppTestHttpClient;
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