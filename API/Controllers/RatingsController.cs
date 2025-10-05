using Application.Ratings.DTOs;
using Application.Ratings.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public RatingsController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpPost]
        public async Task<IActionResult> AddRating(RatingDto dto)
        {
            var result = await _ratingService.AddRatingAsync(dto);
            return result ? Ok(new { message = "Rating added successfully" }) : BadRequest("You already rated this product");
        }

        [HttpGet("{productId:int}")]
        public async Task<IActionResult> GetRatings(int productId)
        {
            var result = await _ratingService.GetRatingsForProductAsync(productId);
            return Ok(result);
        }
    }
}
