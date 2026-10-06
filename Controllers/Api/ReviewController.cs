using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechShop_API_backend_.Models;
using TechShop_API_backend_.Data;

using Amazon.S3;
using Amazon.S3.Model;
using TechShop_API_backend_.Service;
using TechShop_API_backend_.DTOs.Review;

namespace TechShop_API_backend_.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {

        private readonly ReviewRepository _reviewRepository;
        private readonly ProductRepository _productRepository;
        private readonly ImageService _imageService;
        public ReviewController(ReviewRepository reviewRepository, ImageService imageService, ProductRepository productRepository)
        {
            _reviewRepository = reviewRepository;
            _imageService = imageService;
            _productRepository = productRepository;
        }

        // GET: api/review
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var reviews = await _reviewRepository.GetAllAsync();
            return Ok(reviews);
        }

        // GET: api/review/product/{productId}
        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetByProduct(string productId)
        {
            var reviews = await _reviewRepository.GetReviewsByProductIdAsync(productId);
            return Ok(reviews);
        }

        // GET: api/review/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var reviews = await _reviewRepository.GetReviewsByUserIdAsync(userId);
            if (reviews == null || reviews.Count == 0)
                return NotFound();

            return Ok(reviews);
        }

        // POST: api/review/Create
        [Authorize]
        [HttpPost("Create")]
        public async Task<IActionResult> CreateReviewInDirectory([FromForm] CreateReviewRequestDto reviewDto)
        {
            // Validate incoming data
            if (reviewDto == null)
            {
                return BadRequest("Review data is null.");
            }

            if (reviewDto.Stars < 1 || reviewDto.Stars > 5)
            {
                return BadRequest("Stars must be between 1 and 5.");
            }

            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("User ID not found in token.");
            }

            // Initialize the Review object to save in the database
            var product = await _productRepository.GetByIdAsync(reviewDto.ProductId);
            if (product == null)
            {
                return BadRequest("Invalid Product ID.");
            }

            var review = new Review
            {
                UserID = userId,
                ProductId = reviewDto.ProductId,
                Stars = reviewDto.Stars,
                Comment = reviewDto.Comment,
                CreatedTime = DateTime.UtcNow,
                MediaURLs = new List<string>()
            };

            // Upload the media files and add their URLs to the review
            if (reviewDto.MediaFiles != null)
            {
                foreach (var file in reviewDto.MediaFiles)
                {
                    var imageUrl = await _imageService.UploadImageAsync(file);
                    review.MediaURLs.Add(imageUrl);
                }
            }

            // Save the review to database
            await _reviewRepository.CreateReviewAsync(review);

            return Ok("Review created successfully.");
        }

        // DELETE: api/review/{id}
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await _reviewRepository.DeleteReviewWithIDAsync(id);
            if (existing)
            {
                return NoContent();
            }

            return NotFound();
        }
    }



}
