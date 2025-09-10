namespace Ecom.Application.Products.DTOs
{
    public record ProductDTO
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public decimal Price { get; init; }
        public string CategoryName { get; init; } = string.Empty;
        public double Rating { get; init; }
        public List<PhotoDTO> Photos { get; init; } = new();
    }
}
