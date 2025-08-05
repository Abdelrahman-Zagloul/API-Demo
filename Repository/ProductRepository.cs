using Microsoft.EntityFrameworkCore;
using API_Demo.Data;
using API_Demo.Model;

namespace API_Demo.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(Product product)
        {
            try
            {
                _context.Products.Add(product);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void Delete(Product product)
        {
            try
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<Product> GetAll()
        {
            return _context.Products.Include(x => x.Category).ToList();
        }

        public Product? GetById(int id)
        {
            return _context.Products.Include(x => x.Category).FirstOrDefault(x => x.Id == id);
        }

        public void Update(Product product)
        {
            try
            {
                var oldProduct = _context.Products.Find(product.Id);
                if (oldProduct == null)
                    return;

                oldProduct.Name = product.Name;
                oldProduct.Price = product.Price;
                oldProduct.CategoryId = product.CategoryId;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
