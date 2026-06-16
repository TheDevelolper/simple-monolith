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

    public ActionResult<ProductApiModel> GetProductById(int id)
    {
        var domainProduct = ProductApiModel.FromDomain(productService.GetProductById(id));
        return Ok(domainProduct);
    }
}