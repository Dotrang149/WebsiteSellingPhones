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

        [Fact]
        public async Task GetOrdersByUserId_ReturnsOkResult_WhenOrdersExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var orders = new List<Order>
    {
        new Order { Id = Guid.NewGuid(), TotalAmount = 100, PaymentMethod = "Credit Card", UserOrderId = userId },
        new Order { Id = Guid.NewGuid(), TotalAmount = 50, PaymentMethod = "PayPal", UserOrderId = userId }
    };

            var mockOrderService = new Mock<IOrderService>();
            mockOrderService.Setup(service => service.GetOrdersByUserIdAsync(userId))
                            .ReturnsAsync(orders);

            var controller = new OrderController(mockOrderService.Object);

            // Act
            var result = await controller.GetOrdersByUserId(userId) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var ordersViewModels = result.Value as List<OrderVm>;
            Assert.NotNull(ordersViewModels);
            Assert.Equal(2, ordersViewModels.Count);
            Assert.Equal(orders[0].Id, ordersViewModels[0].Id);
            Assert.Equal(orders[1].Id, ordersViewModels[1].Id);
        }

        [Fact]
        public async Task GetOrdersByUserId_ReturnsNotFound_WhenNoOrdersExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var mockOrderService = new Mock<IOrderService>();
            mockOrderService.Setup(service => service.GetOrdersByUserIdAsync(userId))
                            .ReturnsAsync((List<Order>)null); // No orders found

            var controller = new OrderController(mockOrderService.Object);

            // Act
            var result = await controller.GetOrdersByUserId(userId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

    }
}
