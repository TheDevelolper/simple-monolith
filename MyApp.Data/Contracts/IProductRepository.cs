using Data.Entities;

namespace Data.Contracts;

public interface IProductRepository
{
    public ProductEntity GetProduct(int id);
    public IEnumerable<ProductEntity> GetProducts();
}