using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebSellingPhone.Bussiness.ViewModel;
using WebSellingPhone.Data;
using WebSellingPhone.Data.Models;
using WebSellingPhone.WebAPI.Controllers;
using Xunit;

namespace WebSellingPhone.UnitTest
{
    public class ReviewControllerTests
    {
        private readonly PhoneWebDbContext _context;
        private readonly ReviewController _controller;

        public ReviewControllerTests()
        {
            var options = new DbContextOptionsBuilder<PhoneWebDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new PhoneWebDbContext(options);
            _controller = new ReviewController(_context);

            // Seed data
            SeedDatabase();
        }

        private void SeedDatabase()
        {

            var reviews = new List<Review>
            {
                new Review { UserId = Guid.NewGuid(), ProductId = Guid.NewGuid(), Comment = "Great product!" },
                new Review { UserId = Guid.NewGuid(), ProductId = Guid.NewGuid(), Comment = "Not so good." }
            };

            _context.Reviews.AddRange(reviews);
            _context.SaveChanges();
        }


        [Fact]
        public async Task CreateReview_ReturnsCreatedAtActionResult_WithReview()
        {
            // Arrange
            var reviewVm = new ReviewVm
            {
                Comment = "Excellent!",
                UserId = Guid.NewGuid(),
                ProductId = Guid.NewGuid()
            };

            // Act
            var result = await _controller.CreateReview(reviewVm);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Review>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var returnValue = Assert.IsType<Review>(createdAtActionResult.Value);
            Assert.Equal(reviewVm.Comment, returnValue.Comment);
        }

        [Fact]
        public async Task UpdateReviewComment_ReturnsNoContent_WhenReviewExists()
        {
            // Arrange
            var reviewVm = new ReviewVm
            {
                Comment = "Updated comment",
                UserId = _context.Reviews.First().UserId,
                ProductId = _context.Reviews.First().ProductId
            };

            // Act
            var result = await _controller.UpdateReviewComment(reviewVm);

            // Assert
            Assert.IsType<NoContentResult>(result);
            var updatedReview = await _context.Reviews
                .FirstOrDefaultAsync(r => r.UserId == reviewVm.UserId && r.ProductId == reviewVm.ProductId);
            Assert.Equal(reviewVm.Comment, updatedReview.Comment);
        }

        [Fact]
        public async Task DeleteReview_ReturnsOkResult_WhenReviewExists()
        {
            // Arrange
            var review = _context.Reviews.First();
            var userId = review.UserId;
            var productId = review.ProductId;

            // Act
            var result = await _controller.DeleteReview(userId, productId);

            // Assert
            Assert.IsType<OkResult>(result);
            var deletedReview = await _context.Reviews
                .FirstOrDefaultAsync(r => r.UserId == userId && r.ProductId == productId);
            Assert.Null(deletedReview);
        }
    }
}
