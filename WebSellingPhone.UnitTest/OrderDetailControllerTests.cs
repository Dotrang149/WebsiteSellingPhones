using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using WebSellingPhone.Bussiness.Service;
using WebSellingPhone.Bussiness.ViewModel;
using WebSellingPhone.Data.Models;
using WebSellingPhone.WebAPI.Controllers;

namespace WebSellingPhone.UnitTest
{
    public class OrderDetailControllerTests
    {
        private readonly Mock<IOrderDetailService> _mockOrderDetailService;
        private readonly Mock<ILogger<OrderDetailController>> _mockLogger;
        private readonly OrderDetailController _controller;

        public OrderDetailControllerTests()
        {
            _mockOrderDetailService = new Mock<IOrderDetailService>();
            _mockLogger = new Mock<ILogger<OrderDetailController>>();
            _controller = new OrderDetailController(_mockOrderDetailService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetOrderDetailsByOrderIdAsync_ReturnsOkResult_WithOrderDetailViewModels()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var orderDetails = new List<OrderDetail>
            {
                new OrderDetail { Price = 10m, Quantity = 2, ProductId = Guid.NewGuid(), OrderId = orderId },
                new OrderDetail { Price = 20m, Quantity = 1, ProductId = Guid.NewGuid(), OrderId = orderId }
            };

            var orderDetailViewModels = orderDetails.Select(od => new OrderDetailVm
            {
                Price = od.Price,
                Quantity = od.Quantity,
                ProductId = od.ProductId,
                OrderId = od.OrderId
            }).ToList();

            _mockOrderDetailService
                .Setup(service => service.GetOrderDetailsByOrderIdAsync(orderId))
                .ReturnsAsync(orderDetails);

            // Act
            var result = await _controller.GetOrderDetailsByOrderIdAsync(orderId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var returnValue = okResult.Value as List<OrderDetailVm>;
            Assert.NotNull(returnValue);
            Assert.Equal(orderDetailViewModels.Count, returnValue.Count);
        }

        [Fact]
        public async Task GetOrderDetailsByProductIdAsync_ReturnsOkResult_WithOrderDetailViewModels()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var orderDetails = new List<OrderDetail>
            {
                new OrderDetail { Price = 10m, Quantity = 2, ProductId = productId, OrderId = Guid.NewGuid() },
                new OrderDetail { Price = 20m, Quantity = 1, ProductId = productId, OrderId = Guid.NewGuid() }
            };

            var orderDetailViewModels = orderDetails.Select(od => new OrderDetailVm
            {
                Price = od.Price,
                Quantity = od.Quantity,
                ProductId = od.ProductId,
                OrderId = od.OrderId
            }).ToList();

            _mockOrderDetailService
                .Setup(service => service.GetOrderDetailsByProductIdAsync(productId))
                .ReturnsAsync(orderDetails);

            // Act
            var result = await _controller.GetOrderDetailsByProductIdAsync(productId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var returnValue = okResult.Value as List<OrderDetailVm>;
            Assert.NotNull(returnValue);
            Assert.Equal(orderDetailViewModels.Count, returnValue.Count);
        }

        [Fact]
        public async Task CreateOrderDetailAsync_ReturnsOkResult_OnSuccess()
        {
            // Arrange
            var orderDetailVm = new OrderDetailVm
            {
                Price = 10m,
                Quantity = 2,
                ProductId = Guid.NewGuid(),
                OrderId = Guid.NewGuid()
            };

            _mockOrderDetailService
                .Setup(service => service.AddAsync(It.IsAny<OrderDetail>()))
                .ReturnsAsync(1);

            // Act
            var result = await _controller.CreateOrderDetailAsync(orderDetailVm);

            // Assert
            var okResult = result as OkResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task CreateOrderDetailAsync_ReturnsBadRequest_WhenOrderDetailVmIsNull()
        {
            // Arrange
            OrderDetailVm orderDetailVm = null;

            // Act
            var result = await _controller.CreateOrderDetailAsync(orderDetailVm);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task CreateOrderDetailAsync_ReturnsBadRequest_OnFailure()
        {
            // Arrange
            var orderDetailVm = new OrderDetailVm
            {
                Price = 10m,
                Quantity = 2,
                ProductId = Guid.NewGuid(),
                OrderId = Guid.NewGuid()
            };

            _mockOrderDetailService
                .Setup(service => service.AddAsync(It.IsAny<OrderDetail>()))
                .ReturnsAsync(0);

            // Act
            var result = await _controller.CreateOrderDetailAsync(orderDetailVm);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }
    }
}
