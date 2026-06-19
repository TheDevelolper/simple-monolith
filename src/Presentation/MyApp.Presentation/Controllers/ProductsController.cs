using Domain.Contracts;
using Microsoft.AspNetCore.Mvc;
using MyApp.Presentation.Models.Api;
using MyApp.Shared;

namespace MyApp.Presentation.Controllers;

public class ProductsController(IProductService productService): ControllerBase
{
    [HttpGet]
    [Route("api/products/{id:int}")]
    public ActionResult<ProductApiModel> GetProductById(int id)
    {
        var productResult = productService.GetProduct(id);

        return productResult.Status switch
        {
            ResultStatus.Success when productResult.Value is not null => 
                Ok(ProductApiModel.FromDomain(productResult.Value)),
            
            ResultStatus.NotFound => NotFound(),
            ResultStatus.Error => Problem(productResult.ErrorMessage),
            _ => Problem("An unknown error occurred.")
        };
    }
    
    [HttpPost]
    [Route("api/products")]
    public async Task<ActionResult<List<ProductApiModel>>> Post(
        [FromBody] List<ProductApiModel> apiProducts)
    {
        var domainProducts = apiProducts
            .Select(ProductApiModel.ToDomain)
            .ToList();

        var productResult = await productService.AddProductsAsync(domainProducts);

        // todo: Not sure I like the switch expression here, looks messy, we can't log errors properly.
        // will put up with it for now.
        return productResult.Status switch 
        {
            ResultStatus.Success when productResult.Value is not null => Created(
                "/api/products",
                productResult.Value.Select(ProductApiModel.FromDomain).ToList()),

            _ => Problem("An unknown error occurred.")
        };
    }
}