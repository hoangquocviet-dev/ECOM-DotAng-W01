using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using phase_1.DTOs;
using phase_1.Services.Interfaces;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace phase_1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        private int GetUserId()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdString, out int userId))
            {
                return userId;
            }
            return 0;
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetProductReviewsAsync(int productId)
        {
            var reviews = await _reviewService.GetProductReviewsAsync(productId);
            return Ok(reviews);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddReviewAsync([FromBody] AddReviewRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int userId = GetUserId();

            try
            {
                var review = await _reviewService.AddReviewAsync(userId, request);
                if (review == null)
                {
                    return BadRequest("You have already reviewed this product.");
                }

                return Ok(review);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
