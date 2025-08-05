using API_Demo.Model;

namespace API_Demo.Repository
{
    public interface ICategoryRepository
    {
        void Add(Category category);
        void Update(Category category);
        void Delete(Category category);
        Category? GetById(int id);
        List<Category> GetAll();

    }
}
