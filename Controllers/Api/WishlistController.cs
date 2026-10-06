using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using TechShop_API_backend_.Data;
using TechShop_API_backend_.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TechShop_API_backend_.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WishlistController : ControllerBase
    {
        private readonly UserDetailRepository _userDetailRepository;

        public WishlistController(UserDetailRepository userDetailRepository)
        {
            _userDetailRepository = userDetailRepository;
        }

        private bool TryGetUserId(out int userId)
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdStr, out userId);
        }

        /// <summary>
        /// Get all wishlist items for a user.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetWishlist()
        {
            if (!TryGetUserId(out int userId))
                return Unauthorized("User ID not found in token.");

            var user = await _userDetailRepository.GetUserByUserId(userId);
            if (user == null)
                return NotFound("User not found.");

            return Ok(user.Wishlist ?? new List<WishlistItem>());
        }

        [HttpGet("check")]
        public async Task<IActionResult> CheckIsAdded(string productId)
        {
            if (!TryGetUserId(out int userId))
                return Unauthorized("User ID not found in token.");

            var success = await _userDetailRepository.IsProductInWishlistAsync(userId, productId);
            return Ok(success);
        }

        /// <summary>
        /// Add a new item to the user's wishlist.
        /// </summary>
        [HttpPost("add")]
        public async Task<IActionResult> AddWishlistItem([FromBody] WishlistItem wishlistItem)
        {
            if (wishlistItem == null || string.IsNullOrEmpty(wishlistItem.ProductId))
                return BadRequest("Invalid wishlist item data.");

            if (!TryGetUserId(out int userId))
                return Unauthorized("User ID not found in token.");

            var success = await _userDetailRepository.AddWishlistItemAsync(userId, wishlistItem);
            if (!success)
                return Conflict("Item already exists in wishlist or user not found.");

            return Ok("Item added to wishlist successfully.");
        }

        /// <summary>
        /// Remove an item from the user's wishlist.
        /// </summary>
        [HttpDelete("remove/{productId}")]
        public async Task<IActionResult> RemoveWishlistItem(string productId)
        {
            if (!TryGetUserId(out int userId))
                return Unauthorized("User ID not found in token.");

            var success = await _userDetailRepository.RemoveWishlistItemAsync(userId, productId);
            if (!success)
                return NotFound("Wishlist item not found or user not found.");

            return Ok("Item removed from wishlist.");
        }

        /// <summary>
        /// Move an item from wishlist to cart.
        /// </summary>
        [HttpPost("move/{productId}")]
        public async Task<IActionResult> MoveWishlistItemToCart(string productId)
        {
            if (!TryGetUserId(out int userId))
                return Unauthorized("User ID not found in token.");

            var success = await _userDetailRepository.MoveWishlistItemToCartAsync(userId, productId);
            if (!success)
                return BadRequest("Failed to move item from wishlist to cart.");

            return Ok("Item moved to cart successfully.");
        }
    }
}

