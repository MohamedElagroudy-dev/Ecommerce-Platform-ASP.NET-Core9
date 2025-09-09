using Core.Entities.Product;
using Core.Interfaces;
using Core.Sharing;
using Ecom.Application.Products.DTOs;
using Ecom.Application.Products.Mappings;
using Ecom.Core.Entities.Product;

namespace Ecom.Application.Products.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageManagementService _imageService;

        public ProductService(IUnitOfWork unitOfWork, IImageManagementService imageService)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
        }

        public async Task<IEnumerable<ProductDTO>> GetAllAsync(ProductParams productParams)
        {
            var products = await _unitOfWork.Products.GetAllAsync(productParams);
            return products.Select(p => p.ToDto()).ToList();
        }

        public async Task<bool> AddAsync(AddProductDTO dto)
        {
            if (dto == null) return false;

            var product = dto.ToEntity();
            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.CompleteAsync();

            if (dto.Photos != null)
            {
                var imagePaths = await _imageService.AddImageAsync(dto.Photos, dto.Name);
                var photos = imagePaths.Select(path => new Photo
                {
                    ImageName = path,
                    ProductId = product.Id
                }).ToList();

                foreach (var photo in photos)
                    await _unitOfWork.Photos.AddAsync(photo);

                await _unitOfWork.CompleteAsync();
            }

            return true;
        }

        public async Task<bool> UpdateAsync(UpdateProductDTO dto)
        {
            var product = await _unitOfWork.Products.GetByidAsync(dto.Id, p => p.Photos);
            if (product == null) return false;

            product.UpdateEntity(dto);

            // Delete old images
            foreach (var photo in product.Photos)
                _imageService.DeleteImageAsync(photo.ImageName);

            product.Photos.Clear();

            // Add new images
            if (dto.Photos != null)
            {
                var imagePaths = await _imageService.AddImageAsync(dto.Photos, dto.Name);
                product.Photos = imagePaths.Select(path => new Photo
                {
                    ImageName = path,
                    ProductId = product.Id
                }).ToList();
            }

            await _unitOfWork.Products.UpdateAsync(product.Id, product);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByidAsync(id, p => p.Photos);
            if (product == null) return false;

            foreach (var photo in product.Photos)
                _imageService.DeleteImageAsync(photo.ImageName);

            await _unitOfWork.Products.DeleteAsync(product.Id);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
