
namespace Ecom.Application.Products.DTOs
{
    public record ProductDTO
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public decimal NewPrice { get; init; }
        public decimal Price { get; init; }
        public string CategoryName { get; init; }
        public double Rating { get; init; }
        public List<PhotoDTO> Photos { get; init; } = new();
    }
}