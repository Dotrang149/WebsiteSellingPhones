using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using WebSellingPhone.Bussiness.Service;
using WebSellingPhone.Bussiness.ViewModel;
using WebSellingPhone.Data;
using WebSellingPhone.Data.Models;
using WebSellingPhone.WebAPI.Controllers;

namespace WebSellingPhone.UnitTest
{
    public class ProductControllerTests
    {
        private readonly ProductController _controller;
        private readonly Mock<IProductService> _mockProductService;
        private readonly PhoneWebDbContext _context;

        public ProductControllerTests()
        {
            var options = new DbContextOptionsBuilder<PhoneWebDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new PhoneWebDbContext(options);

            SeedDatabase();

            _mockProductService = new Mock<IProductService>();

            _controller = new ProductController(_mockProductService.Object, _context);
        }

        private void SeedDatabase()
        {
            var products = new List<Product>
        {
            new Product { Id = Guid.NewGuid(), Name = "Product1", Price = 100, Description = "Description1", Image = "Image1" },
            new Product { Id = Guid.NewGuid(), Name = "Product2", Price = 200, Description = "Description2", Image = "Image2" }
        };

            _context.Products.AddRange(products);
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithListOfProducts()
        {
            // Arrange
            var products = new List<Product>
    {
        new Product { Id = Guid.NewGuid(), Name = "Product1", Price = 100, Description = "Description1", Image = "Image1" },
        new Product { Id = Guid.NewGuid(), Name = "Product2", Price = 200, Description = "Description2", Image = "Image2" }
    };

            _mockProductService
                .Setup(service => service.GetAllAsync())
                .ReturnsAsync(products);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);

            var returnValue = okResult.Value as List<Product>;
            Assert.NotNull(returnValue);
            Assert.Equal(2, returnValue.Count);
        }


        [Fact]
        public async Task GetById_ReturnsOkResult_WithProductVm()
        {
            // Arrange
            var product = _context.Products.First();
            _mockProductService
                .Setup(service => service.GetByIdAsync(product.Id))
                .ReturnsAsync(product);

            // Act
            var result = await _controller.GetById(product.Id);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var returnValue = okResult.Value as ProductVm;
            Assert.NotNull(returnValue);
            Assert.Equal(product.Id, returnValue.Id);
        }

        [Fact]
        public async Task Create_ReturnsOkResult()
        {
            // Arrange
            var newProductCreate = new ProductCreate
            {
                Name = "New Product",
                Price = 150,
                Description = "New Description",
                Image = "NewImage"
            };

            var newProduct = new Product
            {
                Id = Guid.NewGuid(),
                Name = newProductCreate.Name,
                Price = newProductCreate.Price,
                Description = newProductCreate.Description,
                Image = newProductCreate.Image
            };

            _mockProductService
                .Setup(service => service.CreateProduct(newProductCreate))
                .ReturnsAsync(newProduct);

            // Act
            var result = await _controller.Create(newProductCreate);

            // Assert
            var okResult = result as OkResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }



        [Fact]
        public async Task Update_ReturnsOkResult_WithUpdatedProduct()
        {
            // Arrange
            var product = _context.Products.First();
            var updatedProductVm = new ProductVm
            {
                Id = product.Id,
                Name = "Updated Product",
                Price = 250,
                Description = "Updated Description",
                Image = "UpdatedImage"
            };

            _mockProductService
                .Setup(service => service.UpdateProduct(It.IsAny<ProductVm>()))
                .ReturnsAsync(new Product
                {
                    Id = updatedProductVm.Id,
                    Name = updatedProductVm.Name,
                    Price = updatedProductVm.Price,
                    Description = updatedProductVm.Description,
                    Image = updatedProductVm.Image
                });

            // Act
            var result = await _controller.Update(updatedProductVm);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var returnValue = okResult.Value as Product;
            Assert.NotNull(returnValue);
            Assert.Equal("Updated Product", returnValue.Name);
            Assert.Equal(250, returnValue.Price);
            Assert.Equal("Updated Description", returnValue.Description);
            Assert.Equal("UpdatedImage", returnValue.Image);
        }

        [Fact]
        public async Task Delete_ReturnsOkResult()
        {
            // Arrange
            var productId = _context.Products.First().Id;

            _mockProductService
                .Setup(service => service.DeleteAsync(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(productId);

            // Assert
            var okResult = result as OkResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }
    }
}
