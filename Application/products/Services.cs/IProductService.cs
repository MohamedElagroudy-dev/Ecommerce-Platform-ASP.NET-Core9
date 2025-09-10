using Core.Sharing;
using Ecom.Application.Products.DTOs;

namespace Ecom.Application.Products.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetAllAsync(ProductParams productParams);
        Task<ProductDTO?> AddAsync(AddProductDTO dto);
        Task<ProductDTO?> UpdateAsync(UpdateProductDTO dto);
        Task<ProductDTO?> DeleteAsync(int id);
        Task<ProductDTO?> GetProductAsync(int id);
    }
}
