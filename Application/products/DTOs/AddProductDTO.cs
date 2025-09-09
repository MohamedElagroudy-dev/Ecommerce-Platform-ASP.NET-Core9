using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Ecom.Application.Products.DTOs
{
    public record AddProductDTO
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public decimal Price { get; init; }
        public int CategoryId { get; init; }
        public IFormFileCollection Photos { get; init; }
    }
}
