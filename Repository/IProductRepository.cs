using API_Demo.Model;

namespace API_Demo.Repository
{
    public interface IProductRepository
    {
        void Add(Product product);
        void Update(Product product);
        void Delete(Product product);
        List<Product> GetAll();
        Product? GetById(int id);

    }
}
    