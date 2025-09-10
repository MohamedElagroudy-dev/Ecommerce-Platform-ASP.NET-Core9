using Ecom.Core.Entities.Product;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Product
{
    public class Product : BaseEntity
    {
        public required string Name { get; set; }
        public required string Description { get; set; }

        public decimal Price { get; set; }

        public string? Type { get; set; }
        public string? Brand { get; set; }

        public int QuantityInStock { get; set; }

        public virtual List<Photo> Photos { get; set; } = new();

        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; } = null!;

        public double Rating { get; set; }
    }
}
