using Domain.Contracts;
using Microsoft.AspNetCore.Mvc;
using MyApp.Presentation.Models.Api;

namespace MyApp.Presentation.Controllers;

public class ProductsController(IProductService productService): ControllerBase
{
    
    public IActionResult Index()
    {
        return Content("hello");
    }
    
    [HttpGet]
    [Route("api/products/{id}")]
    public ActionResult<ProductApiModel> GetProductById(int id)
    {
        var domainProduct = ProductApiModel.FromDomain(productService.GetProduct(id));
        if (domainProduct == null) return NotFound();
        
        return Ok(domainProduct);
    }
}