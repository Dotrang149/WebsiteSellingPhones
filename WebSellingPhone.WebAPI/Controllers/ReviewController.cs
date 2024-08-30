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

    
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly PhoneWebDbContext _context;
        
        public ReviewController(PhoneWebDbContext context)
        {
            _context = context;
        }

        [HttpGet("get-by-productid/{productId}")]
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

        
        [HttpGet("get-by-userid/{userId}")]
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

        [Authorize(Policy = "CustomerOnly")]
        [HttpPost("add-comment")]
        public async Task<ActionResult<Review>> CreateReview([FromBody] ReviewVm reviewVm)
        {
            var review = new Review
            {
                Comment = reviewVm.Comment,
                UserId = reviewVm.UserId,
                ProductId = reviewVm.ProductId
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReviewsByProduct", new { productId = review.ProductId }, review);
        }


        [Authorize(Policy = "CustomerOnly")]
        [HttpPut("update-comment")]
        public async Task<IActionResult> UpdateReviewComment([FromBody] ReviewVm reviewVm)
        {
            var review = await _context.Reviews
                .FirstOrDefaultAsync(r => r.UserId == reviewVm.UserId && r.ProductId == reviewVm.ProductId);

            if (review == null)
            {
                return NotFound();
            }

            review.Comment = reviewVm.Comment;
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [Authorize(Policy = "CustomerOnly")]
        [HttpDelete("delete-comment")]
        public async Task<IActionResult> DeleteReview([FromQuery] Guid userId, [FromQuery] Guid productId)
        {
            var review = await _context.Reviews
                .FirstOrDefaultAsync(r => r.UserId == userId && r.ProductId == productId);

            if (review == null)
            {
                return NotFound();
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
