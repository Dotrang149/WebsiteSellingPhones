using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebSellingPhone.Bussiness.Service;
using WebSellingPhone.Bussiness.ViewModel;
using WebSellingPhone.Data;
using WebSellingPhone.Data.Models;

namespace WebSellingPhone.WebAPI.Controllers
{

    [Authorize(Policy = "CustomerOnly")]
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly PhoneWebDbContext _context;
        
        public ReviewController(PhoneWebDbContext context)
        {
            _context = context;
        }

        [HttpGet("get-by-productid/id={productId}")]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviewsByProduct(Guid productId, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var reviews = await _context.Reviews
                .Where(r => r.ProductId == productId)
                .Include(r => r.User)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(reviews);
        }

        
        [HttpGet("get-by-userid/id={userId}")]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviewsByUser(Guid userId, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var reviews = await _context.Reviews
                .Where(r => r.UserId == userId)
                .Include(r => r.Products)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(reviews);
        }

        
        [HttpPost("add-comment")]
        public async Task<ActionResult<Review>> CreateReview(Review review)
        {
            _context.Reviews.Add(new Review
            {
                Comment = review.Comment
            });
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReviewsByProduct", new { productId = review.ProductId }, review);
        }


        [HttpPut("update-commnet-user/{userId}/product/{productId}")]
        public async Task<IActionResult> UpdateReviewComment(Guid userId, Guid productId, [FromBody] string newComment)
        {
            var review = await _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Products)
                .FirstOrDefaultAsync(r => r.UserId == userId && r.ProductId == productId);

            if (review == null)
            {
                return NotFound();
            }

            review.Comment = newComment;
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("delete-comment/{userId}/{productId}")]
        public async Task<IActionResult> DeleteReview(Guid userId, Guid productId)
        {
            var review = await _context.Reviews.FindAsync(userId, productId);
            if (review == null)
                return NotFound();

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
