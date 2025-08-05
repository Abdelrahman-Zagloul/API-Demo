using Microsoft.EntityFrameworkCore;
using API_Demo.Data;
using API_Demo.Model;

namespace API_Demo.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(Category category)
        {
            try
            {
                _context.Categories.Add(category);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public void Update(Category category)
        {
            try
            {
                var oldCategory = _context.Categories.Find(category.Id);
                if (oldCategory == null)
                    return;
                oldCategory.Name = category.Name;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public void Delete(Category category)
        {
            try
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public Category? GetById(int id)
        {
            return _context.Categories.Find(id);
        }
        public List<Category> GetAll()
        {
            return _context.Categories.Include(x=>x.Products).ToList();
        }

    }
}
