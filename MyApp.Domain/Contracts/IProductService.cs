using Domain.Models;

namespace Domain.Contracts;

public interface IProductService
{
    Product GetProduct(int id);
}