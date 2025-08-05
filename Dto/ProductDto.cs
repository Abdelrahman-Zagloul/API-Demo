using API_Demo.Model;
namespace API_Demo.Dto;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public int CategoryId { get; set; }
}
