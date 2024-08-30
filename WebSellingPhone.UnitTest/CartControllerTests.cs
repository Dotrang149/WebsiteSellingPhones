using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebSellingPhone.Bussiness.Service;
using WebSellingPhone.Data.Models;
using WebSellingPhone.WebAPI.Controllers;
using Xunit;

namespace WebSellingPhone.UnitTest
{
    public class CartControllerTests
    {
        private readonly Mock<ICartService> _mockCartService;
        private readonly CartController _controller;

        public CartControllerTests()
        {
            _mockCartService = new Mock<ICartService>();
            _controller = new CartController(_mockCartService.Object);
        }

        [Fact]
        public async Task AddToCart_ValidInput_ReturnsOkResult()
        {
            // Arrange
            var productId = Guid.NewGuid();
            int quantity = 2;

            _mockCartService.Setup(service => service.AddToCart(productId, quantity)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddToCart(productId, quantity);

            // Assert
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be("Product added to cart successfully!");
        }

        [Fact]
        public void GetCart_ReturnsOkResult_WithCartContents()
        {
            // Arrange
            var cart = new Cart
            {
                Items = new List<CartItem>
                {
                    new CartItem { ProductId = Guid.NewGuid(), ProductName = "Product A", Price = 100m, Quantity = 2 },
                    new CartItem { ProductId = Guid.NewGuid(), ProductName = "Product B", Price = 200m, Quantity = 1 }
                }
            };

            _mockCartService.Setup(service => service.GetCurrentCart()).Returns(cart);

            // Act
            var result = _controller.GetCart();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedCart = okResult.Value.Should().BeAssignableTo<Cart>().Subject;
            returnedCart.Items.Should().HaveCount(2);
        }
    }
}

