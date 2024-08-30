using Microsoft.AspNetCore.Mvc;
using Moq;
using WebSellingPhone.Bussiness.Service;
using WebSellingPhone.Bussiness.ViewModel;
using WebSellingPhone.Data.Models;
using WebSellingPhone.WebAPI.Controllers;

namespace WebSellingPhone.UnitTest
{
    public class PromotionControllerTests
    {
        private readonly Mock<IPromotionService> _mockPromotionService;
        private readonly PromotionController _controller;

        public PromotionControllerTests()
        {
            _mockPromotionService = new Mock<IPromotionService>();
            _controller = new PromotionController(_mockPromotionService.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithListOfPromotions()
        {
            // Arrange
            var promotions = new List<Promotion>
            {
                new Promotion { Id = Guid.NewGuid(), Name = "Promo1", Description = "Desc1", DateStart = DateTime.Now, DateEnd = DateTime.Now.AddDays(1) },
                new Promotion { Id = Guid.NewGuid(), Name = "Promo2", Description = "Desc2", DateStart = DateTime.Now, DateEnd = DateTime.Now.AddDays(1) }
            };

            _mockPromotionService.Setup(service => service.GetAllAsync()).ReturnsAsync(promotions);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            var promotionViewModels = okResult.Value as IEnumerable<PromotionVm>;
            Assert.NotNull(promotionViewModels);
            Assert.Equal(2, promotionViewModels.Count());
        }

        [Fact]
        public async Task GetById_ReturnsOkResult_WithPromotionVm()
        {
            // Arrange
            var promotionId = Guid.NewGuid();
            var promotion = new Promotion { Id = promotionId, Name = "Promo1", Description = "Desc1", DateStart = DateTime.Now, DateEnd = DateTime.Now.AddDays(1) };

            _mockPromotionService.Setup(service => service.GetByIdAsync(promotionId)).ReturnsAsync(promotion);

            // Act
            var result = await _controller.GetById(promotionId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            var promotionViewModel = okResult.Value as PromotionVm;
            Assert.NotNull(promotionViewModel);
            Assert.Equal(promotionId, promotionViewModel.Id);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var promotionCreate = new PromotionCreate { Name = "Promo1", Description = "Desc1", DateStart = DateTime.Now, DateEnd = DateTime.Now.AddDays(1) };
            var promotion = new Promotion { Id = Guid.NewGuid(), Name = "Promo1", Description = "Desc1", DateStart = DateTime.Now, DateEnd = DateTime.Now.AddDays(1) };

            _mockPromotionService.Setup(service => service.AddAsync(It.IsAny<Promotion>())).ReturnsAsync(1);

            // Act
            var result = await _controller.Create(promotionCreate);

            // Assert
            var createdAtActionResult = result as CreatedAtActionResult;
            Assert.NotNull(createdAtActionResult);
            Assert.Equal(201, createdAtActionResult.StatusCode);
        }

        [Fact]
        public async Task Update_ReturnsNoContent()
        {
            // Arrange
            var promotionVm = new PromotionVm { Id = Guid.NewGuid(), Name = "Updated Promo", Description = "Updated Desc", DateStart = DateTime.Now, DateEnd = DateTime.Now.AddDays(1) };
            var existingPromotion = new Promotion { Id = promotionVm.Id, Name = "Old Promo", Description = "Old Desc", DateStart = DateTime.Now, DateEnd = DateTime.Now.AddDays(1) };

            _mockPromotionService.Setup(service => service.GetByIdAsync(promotionVm.Id)).ReturnsAsync(existingPromotion);
            _mockPromotionService.Setup(service => service.UpdateAsync(existingPromotion)).ReturnsAsync(1);

            // Act
            var result = await _controller.Update(promotionVm);

            // Assert
            var noContentResult = result as NoContentResult;
            Assert.NotNull(noContentResult);
            Assert.Equal(204, noContentResult.StatusCode);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent()
        {
            // Arrange
            var promotionId = Guid.NewGuid();

            _mockPromotionService.Setup(service => service.DeleteAsync(promotionId)).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(promotionId);

            // Assert
            var noContentResult = result as NoContentResult;
            Assert.NotNull(noContentResult);
            Assert.Equal(204, noContentResult.StatusCode);
        }
    }
}
