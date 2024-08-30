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
    public class OrderControllerTests
    {
        private readonly Mock<IOrderService> _mockOrderService;
        private readonly OrderController _controller;

        public OrderControllerTests()
        {
            _mockOrderService = new Mock<IOrderService>();
            _controller = new OrderController(_mockOrderService.Object);
        }

        [Fact]
        public async Task GetAllOrder_ReturnsOkResult_WithOrders()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order { Id = Guid.NewGuid(), TotalAmount = 100m, PaymentMethod = "Credit Card", UserOrderId = Guid.NewGuid() },
                new Order { Id = Guid.NewGuid(), TotalAmount = 200m, PaymentMethod = "PayPal", UserOrderId = Guid.NewGuid() }
            };

            _mockOrderService.Setup(service => service.GetAllAsync()).ReturnsAsync(orders);

            // Act
            var result = await _controller.GetAllOrder();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedOrders = okResult.Value.Should().BeAssignableTo<List<OrderVm>>().Subject;
            returnedOrders.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetOrderById_ReturnsOkResult_WithOrder()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var order = new Order { Id = orderId, TotalAmount = 100m, PaymentMethod = "Credit Card", UserOrderId = Guid.NewGuid() };

            _mockOrderService.Setup(service => service.GetByIdAsync(orderId)).ReturnsAsync(order);

            // Act
            var result = await _controller.GetOrderById(orderId);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var orderVm = okResult.Value.Should().BeAssignableTo<OrderVm>().Subject;
            orderVm.Id.Should().Be(orderId);
        }

        [Fact]
        public async Task CreateOrder_ValidModel_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var orderVm = new OrderVm { TotalAmount = 100m, PaymentMethod = "Credit Card", UserOrderId = Guid.NewGuid() };
            var order = new Order { Id = Guid.NewGuid(), TotalAmount = orderVm.TotalAmount, PaymentMethod = orderVm.PaymentMethod, UserOrderId = orderVm.UserOrderId };

            _mockOrderService.Setup(service => service.AddAsync(It.IsAny<Order>())).ReturnsAsync(1); // Returns an int

            // Act
            var result = await _controller.CreateOrder(orderVm);

            // Assert
            var createdAtActionResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdAtActionResult.ActionName.Should().Be(nameof(OrderController.GetOrderById));
        }

        [Fact]
        public async Task UpdateOrder_ValidModel_ReturnsNoContent()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var orderVm = new OrderVm { TotalAmount = 100m, PaymentMethod = "Credit Card", UserOrderId = Guid.NewGuid() };
            var existingOrder = new Order { Id = orderId, TotalAmount = 50m, PaymentMethod = "PayPal", UserOrderId = Guid.NewGuid() };

            _mockOrderService.Setup(service => service.GetByIdAsync(orderId)).ReturnsAsync(existingOrder);
            _mockOrderService.Setup(service => service.UpdateAsync(It.IsAny<Order>())).ReturnsAsync(1); // Returns an int

            // Act
            var result = await _controller.UpdateOrder(orderId, orderVm);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteOrder_OrderExists_ReturnsNoContent()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var existingOrder = new Order { Id = orderId, TotalAmount = 100m, PaymentMethod = "Credit Card", UserOrderId = Guid.NewGuid() };

            _mockOrderService.Setup(service => service.GetByIdAsync(orderId)).ReturnsAsync(existingOrder);
            _mockOrderService.Setup(service => service.DeleteAsync(orderId)).ReturnsAsync(true); // Returns a bool

            // Act
            var result = await _controller.DeleteOrder(orderId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteOrder_OrderDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var orderId = Guid.NewGuid();

            _mockOrderService.Setup(service => service.GetByIdAsync(orderId)).ReturnsAsync((Order)null);

            // Act
            var result = await _controller.DeleteOrder(orderId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
