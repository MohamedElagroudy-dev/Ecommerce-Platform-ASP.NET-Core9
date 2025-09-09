using Core.Sharing;
using Ecom.Application.Products.DTOs;

namespace Ecom.Application.Products.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetAllAsync(ProductParams productParams);
        Task<bool> AddAsync(AddProductDTO dto);
        Task<bool> UpdateAsync(UpdateProductDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
