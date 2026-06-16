using Domain.Models;

namespace Domain.Contracts;

public interface IProductService
{
    Product GetProductById(int i);
}