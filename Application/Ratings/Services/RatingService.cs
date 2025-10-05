using Application.Account;
using Application.Ratings.DTOs;
using Application.Ratings.Mappings;
using Core.Entities;
using Core.Entities.Product;
using Core.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ratings.Services
{
    public class RatingService : IRatingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContext _userContext;
        private readonly ILogger<RatingService> _logger;

        public RatingService(IUnitOfWork unitOfWork, IUserContext userContext, ILogger<RatingService> logger)
        {
            _unitOfWork = unitOfWork;
            _userContext = userContext;
            _logger = logger;
        }

        public async Task<bool> AddRatingAsync(RatingDto ratingDto)
        {
            var currentUser = _userContext.GetCurrentUser();
            if (currentUser == null)
                throw new UnauthorizedAccessException("User not authenticated.");

            _logger.LogInformation("Adding rating for product {ProductId} by user {Email}", ratingDto.ProductId, currentUser.Email);

            var exists = await _unitOfWork.Ratings
                .GetByAsync(r => r.AppUserId == currentUser.Id && r.ProductId == ratingDto.ProductId);

            if (exists != null)
            {
                _logger.LogWarning("User {Email} already rated product {ProductId}", currentUser.Email, ratingDto.ProductId);
                return false;
            }

            // Create rating entity
            var rating = ratingDto.ToEntity(currentUser.Id);
            await _unitOfWork.Ratings.AddAsync(rating);

            // Update product average rating
            var product = await _unitOfWork.Products.GetAsync(ratingDto.ProductId);
            if (product != null)
            {
                var ratings = await _unitOfWork.Ratings.GetAllAsync(r => r.ProductId == ratingDto.ProductId);
                double avg = ratings.Any() ? ratings.Average(r => r.Stars) : ratingDto.Stars;
                product.rating = Math.Round(avg * 2, MidpointRounding.AwayFromZero) / 2;

                await _unitOfWork.Products.UpdateAsync(ratingDto.ProductId,product);
            }

            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<IReadOnlyList<ReturnRatingDto>> GetRatingsForProductAsync(int productId)
        {
            _logger.LogInformation("Fetching ratings for product {ProductId}", productId);

            var ratings = await _unitOfWork.Ratings
                .GetAllAsync(r => r.ProductId == productId, r => r.AppUser);

            return ratings.Select(r => r.ToDto()).ToList();
        }
    }
}
