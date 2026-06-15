using phase_1.DTOs;
using phase_1.Models;
using phase_1.Repositories;
using phase_1.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
 

namespace phase_1.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IOrderRepository _orderRepository;

        public ReviewService(IReviewRepository reviewRepository, IOrderRepository orderRepository)
        {
            _reviewRepository = reviewRepository;
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<ReviewDto>> GetProductReviewsAsync(int productId)
        {
            var reviews = await _reviewRepository.GetReviewsByProductIdAsync(productId);
            return reviews.Select(r => new ReviewDto
            {
                Id = r.Id,
                UserId = r.UserId,
                UserName = r.User?.Name ?? "Anonymous",
                ProductId = r.ProductId,
                Rating = r.Rating,
                Comment = r.Comment,
                DateCreated = r.DateCreated,
                MediaUrls = r.ReviewMedias.Select(m => m.MediaUrl).ToList(),
                AdminReply = r.AdminReply,
                ReplyDate = r.ReplyDate
            });
        }

        public async Task<ReviewDto?> AddReviewAsync(int userId, AddReviewRequest request)
        {
            var existingReview = await _reviewRepository.GetReviewByUserAndProductAsync(userId, request.ProductId);
            if (existingReview != null)
            {
                return null; 
            }
            var userOrders = await _orderRepository.GetOrdersByUserIdAsync(userId);
            var hasBought = userOrders.Any(o => o.Status != "Cancelled" && o.OrderDetails.Any(od => od.ProductId == request.ProductId));
            
            if (!hasBought)
            {
                throw new InvalidOperationException("You can only review products you have purchased.");
            }

            var review = new Review
            {
                UserId = userId,
                ProductId = request.ProductId,
                Rating = request.Rating,
                Comment = request.Comment,
                DateCreated = DateTime.UtcNow,
                ReviewMedias = request.MediaUrls != null ? request.MediaUrls.Select(url => new ReviewMedia { MediaUrl = url }).ToList() : new List<ReviewMedia>()
            };

            await _reviewRepository.AddReviewAsync(review);
            var newReviewList = await _reviewRepository.GetReviewsByProductIdAsync(request.ProductId);
            var savedReview = newReviewList.FirstOrDefault(r => r.UserId == userId);

            return new ReviewDto
            {
                Id = savedReview?.Id ?? 0,
                UserId = userId,
                UserName = savedReview?.User?.Name ?? "Anonymous",
                ProductId = request.ProductId,
                Rating = request.Rating,
                Comment = request.Comment,
                DateCreated = review.DateCreated,
                MediaUrls = review.ReviewMedias.Select(m => m.MediaUrl).ToList(),
                AdminReply = savedReview?.AdminReply,
                ReplyDate = savedReview?.ReplyDate
            };
        }

        public async Task<ReviewDto?> ReplyReviewAsync(int reviewId, ReplyReviewRequest request)
        {
            var review = await _reviewRepository.GetReviewByIdAsync(reviewId);
            if (review == null)
            {
                return null;
            }

            review.AdminReply = request.Reply;
            review.ReplyDate = DateTime.UtcNow;

            await _reviewRepository.UpdateReviewAsync(review);

            return new ReviewDto
            {
                Id = review.Id,
                UserId = review.UserId,
                UserName = review.User?.Name ?? "Anonymous",
                ProductId = review.ProductId,
                Rating = review.Rating,
                Comment = review.Comment,
                DateCreated = review.DateCreated,
                MediaUrls = review.ReviewMedias.Select(m => m.MediaUrl).ToList(),
                AdminReply = review.AdminReply,
                ReplyDate = review.ReplyDate
            };
        }
    }
}
