using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API_Demo.Attributes;
using API_Demo.Dto;
using API_Demo.Filter;
using API_Demo.Model;
using API_Demo.Repository;

namespace API_Demo.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    [Authorize]
    //[ServiceFilter(typeof(PermissionBeasdOnAuthorization))]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _Mapper;

        public ProductController(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _Mapper = mapper;
        }

        [HttpGet]
        [CheckPermission(Permission.Read)]
        public IActionResult GetAll()
        {
            var products = _productRepository.GetAll();
            if (!products.Any())
                return NotFound();
            //var productCategoryDto = products.Select(x => new ProductCategoryDto()
            //{
            //    Id = x.Id,
            //    Name = x.Name,
            //    CategoryName = x.Category.Name,
            //    Price = x.Price
            //});
            var ProductDto=_Mapper.Map<IEnumerable<ProductDto>>(products);   

            return Ok(ProductDto);
        }



        [HttpGet("{id}")]
        [CheckPermission(Permission.Read)]
        public IActionResult Get([FromRoute] int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
                return NotFound();
            var productCategoryDto = new ProductCategoryDto()
            {
                Id = product.Id,
                Price = product.Price,
                Name = product.Name,
                CategoryName = product.Category.Name
            };
            return Ok(productCategoryDto);
        }


        [HttpPut("{id}")]
        [CheckPermission(Permission.Update)]
        public IActionResult Update([FromRoute] int id, ProductDto productDto)
        {
            try
            {
                if (id != productDto.Id)
                    return BadRequest();
                var existing = _productRepository.GetById(id);
                if (existing == null)
                    return NotFound();
                var product = new Product() { Id = productDto.Id, Name = productDto.Name, Price = productDto.Price, CategoryId = productDto.CategoryId };
                _productRepository.Update(product);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound("An Error Will Update This Product");
            }
        }


        [HttpPost]
        [CheckPermission(Permission.Create)]
        public IActionResult Add(ProductDto productDto)
        {
            try
            {
                var product = new Product() { Name = productDto.Name, Price = productDto.Price, CategoryId = productDto.CategoryId };
                _productRepository.Add(product);
                return Created();
            }
            catch (Exception ex)
            {
                return NotFound("An Error Will add This Product");
            }
        }


        [HttpDelete("{id}")]
        [CheckPermission(Permission.Delete)]
        public IActionResult Delete(int id)
        {
            try
            {
                var product = _productRepository.GetById(id);
                if (product == null)
                    return NotFound();

                _productRepository.Delete(product);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound("An Error Will Remove This Product");
            }


        }
    }
}
