using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebSellingPhone.Bussiness.Service;
using WebSellingPhone.Bussiness.ViewModel;
using WebSellingPhone.Data.Models;
using WebSellingPhone.WebAPI.Controllers;
using Xunit;

namespace WebSellingPhone.UnitTest
{
    public class BrandsControllerTests
    {
        private readonly Mock<IBrandService> _mockBrandService;
        private readonly BrandsController _controller;

        public BrandsControllerTests()
        {
            _mockBrandService = new Mock<IBrandService>();
            _controller = new BrandsController(_mockBrandService.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithListOfBrands()
        {
            // Arrange
            var brands = new List<Brand>
        {
            new() { Id = Guid.NewGuid(), Name = "Brand A", Description = "Description A" },
            new() { Id = Guid.NewGuid(), Name = "Brand B", Description = "Description B" }
        };
            _mockBrandService.Setup(service => service.GetAllAsync())
                             .ReturnsAsync(brands);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedBrands = okResult.Value.Should().BeAssignableTo<IEnumerable<Brand>>().Subject;
            returnedBrands.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetById_ExistingBrand_ReturnsOkResult()
        {
            // Arrange
            var brandId = Guid.NewGuid();
            var brand = new Brand { Id = brandId, Name = "Brand A", Description = "Description A" };
            _mockBrandService.Setup(service => service.GetByIdAsync(brandId))
                             .ReturnsAsync(brand);

            // Act
            var result = await _controller.GetById(brandId);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedBrand = okResult.Value.Should().BeOfType<BrandVm>().Subject;
            returnedBrand.Name.Should().Be("Brand A");
            returnedBrand.Description.Should().Be("Description A");
        }

        [Fact]
        public async Task GetById_NonExistingBrand_ReturnsNotFound()
        {
            // Arrange
            var brandId = Guid.NewGuid();
            _mockBrandService.Setup(service => service.GetByIdAsync(brandId))
                             .ReturnsAsync((Brand)null);

            // Act
            var result = await _controller.GetById(brandId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Create_ValidBrand_ReturnsCreatedAtAction()
        {
            // Arrange
            var brandVm = new BrandCreate { Name = "Brand A", Description = "Description A" };
            var brand = new Brand { Id = Guid.NewGuid(), Name = brandVm.Name, Description = brandVm.Description };

            _mockBrandService.Setup(service => service.AddAsync(It.IsAny<Brand>()))
                             .ReturnsAsync(1);

            // Act
            var result = await _controller.Create(brandVm);

            // Assert
            result.Should().BeOfType<CreatedAtActionResult>();
        }

        [Fact]
        public async Task Update_ValidBrand_ReturnsNoContent()
        {
            // Arrange
            var brandVm = new BrandVm { Id = Guid.NewGuid(), Name = "Brand A Updated", Description = "Description A Updated" };
            var existingBrand = new Brand { Id = brandVm.Id, Name = "Brand A", Description = "Description A" };

            _mockBrandService.Setup(service => service.GetByIdAsync(brandVm.Id))
                             .ReturnsAsync(existingBrand);
            _mockBrandService.Setup(service => service.UpdateAsync(existingBrand))
                             .ReturnsAsync(1);

            // Act
            var result = await _controller.Update(brandVm);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Delete_ExistingBrand_ReturnsNoContent()
        {
            // Arrange
            var brandId = Guid.NewGuid();

            _mockBrandService.Setup(service => service.DeleteAsync(brandId))
                             .ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(brandId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Delete_NonExistingBrand_ReturnsNotFound()
        {
            // Arrange
            var brandId = Guid.NewGuid();

            _mockBrandService.Setup(service => service.DeleteAsync(brandId))
                             .ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(brandId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
