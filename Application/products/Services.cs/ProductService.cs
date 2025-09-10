using Application.Common;
using Core.Entities.Product;
using Core.Exceptions;
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

        public async Task<PagedResult<ProductDTO>> GetAllAsync(ProductParams productParams)
        {
            var (products,totalCount) = await _unitOfWork.Products.GetAllAsync(productParams);

            var productsDto = products.Select(p => p.ToDto()).ToList();

            var result = new PagedResult<ProductDTO>(productsDto, totalCount, productParams.pageSize, productParams.PageNumber);
            return result;
        }

        public async Task<ProductDTO?> AddAsync(AddProductDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

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

            return product.ToDto();
        }

        public async Task<bool> UpdateAsync(UpdateProductDTO updateProductDTO)
        {
            if (updateProductDTO is null)
                return false;

            var findProduct = await _unitOfWork.Products.GetByidAsync(
                updateProductDTO.Id,
                p => p.Photos,
                p => p.Category);

            if (findProduct is null)
                return false;

            // Update product properties using your existing mapping extension
            findProduct.UpdateEntity(updateProductDTO);

            // Get existing photos for this product
            var existingPhotos = findProduct.Photos?.ToList() ?? new List<Photo>();

            // Delete existing images from storage and database
            foreach (var photo in existingPhotos)
            {
                _imageService.DeleteImageAsync(photo.ImageName);
                await _unitOfWork.Photos.DeleteAsync(photo.Id);
            }

            // Add new photos if provided
            if (updateProductDTO.Photos != null && updateProductDTO.Photos.Any())
            {
                var imagePaths = await _imageService.AddImageAsync(updateProductDTO.Photos, updateProductDTO.Name);
                var newPhotos = imagePaths.Select(path => new Photo
                {
                    ImageName = path,
                    ProductId = updateProductDTO.Id
                }).ToList();

                foreach (var photo in newPhotos)
                    await _unitOfWork.Photos.AddAsync(photo);
            }

            // Update the product using your repository pattern
            await _unitOfWork.Products.UpdateAsync(updateProductDTO.Id, findProduct);
            await _unitOfWork.CompleteAsync();

            return true;
        }


        public async Task<ProductDTO?> DeleteAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByidAsync(id, p => p.Photos, p => p.Category);
            if (product == null)
                throw new NotFoundException(nameof(Product), id.ToString());

            foreach (var photo in product.Photos ?? Enumerable.Empty<Photo>())
            {
                _imageService.DeleteImageAsync(photo.ImageName);
            }


            await _unitOfWork.Products.DeleteAsync(product.Id);
            await _unitOfWork.CompleteAsync();

            return product.ToDto();
        }

        public async Task<ProductDTO?> GetProductAsync(int id)
        {
            var product = await _unitOfWork.Products
                .GetByidAsync(id, p => p.Photos, p => p.Category);

            if (product == null)
                throw new NotFoundException(nameof(Product), id.ToString());

            return product.ToDto();
        }
    }
}
