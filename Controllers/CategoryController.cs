using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API_Demo.Dto;
using API_Demo.Model;
using API_Demo.Repository;

namespace API_Demo.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var categories = _categoryRepository.GetAll();
            if (!categories.Any())
                return NotFound();

            var categoriesDto = categories.Select(x => new CategoryDto() { Id = x.Id, Name = x.Name, NumberOfProduct = x.Products.Count() });
            return Ok(categoriesDto);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var category = _categoryRepository.GetById(id);
            if (category == null)
                return NotFound();
            return Ok(new { category.Id, category.Name });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var category = _categoryRepository.GetById(id);
                if (category == null)
                    return NotFound();

                _categoryRepository.Delete(category);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound("An Error Will remove This category");
            }
        }
        [HttpPost]
        public IActionResult Create(CategoryAddDto categoryDto)
        {
            try
            {
                var category = new Category() { Id = categoryDto.Id, Name = categoryDto.Name };
                _categoryRepository.Add(category);
                return Created();
            }
            catch (Exception ex)
            {
                return NotFound("An Error Will add This category");
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CategoryAddDto categoryDto)
        {
            try
            {
                var result = _categoryRepository.GetById(id);
                if (result == null)
                    return NotFound();

                var category = new Category() { Id = categoryDto.Id, Name = categoryDto.Name };
                _categoryRepository.Update(category);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound("An Error Will update This category");
            }
        }
    }
}
