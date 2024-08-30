using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebSellingPhone.Bussiness.Service;
using WebSellingPhone.Bussiness.ViewModel;
using WebSellingPhone.Data.Models;
using WebSellingPhone.WebAPI.Controllers;

namespace WebSellingPhone.UnitTest
{
    public class UserControllerTests
    {
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly Mock<UserManager<Users>> _mockUserManager;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _mockAuthService = new Mock<IAuthService>();

            var userStore = new Mock<IUserStore<Users>>();
            _mockUserManager = new Mock<UserManager<Users>>(userStore.Object, null, null, null, null, null, null, null, null);

            _controller = new UserController(_mockAuthService.Object, _mockUserManager.Object);
        }

        [Fact]
        public async Task Register_ReturnsCreatedResult_WhenSuccessful()
        {
            // Arrange
            var registerViewModel = new RegisterViewModel
            {
                UserName = "testuser",
                Password = "password",
                ConfirmPassword = "password",
                Email = "test@example.com",
                PhoneNumber = "1234567890"
            };

            var loginResponseViewModel = new LoginResponseViewModel
            {
                Id = Guid.NewGuid(),
                UserName = "testuser",
                Token = "sampletoken",
                RefershToken = "samplerefreshtoken",
                Expires = DateTime.UtcNow.AddHours(1)
            };

            _mockAuthService.Setup(s => s.RegisterAsync(registerViewModel))
                .ReturnsAsync(loginResponseViewModel);

            // Act
            var result = await _controller.Register(registerViewModel);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);
            var returnValue = Assert.IsType<LoginResponseViewModel>(createdResult.Value);

            Assert.Equal(loginResponseViewModel.Token, returnValue.Token);
            Assert.Equal(loginResponseViewModel.RefershToken, returnValue.RefershToken);
            Assert.Equal(loginResponseViewModel.UserName, returnValue.UserName);
            Assert.NotEqual(Guid.Empty, returnValue.Id);
            Assert.True(returnValue.Expires > DateTime.UtcNow);
        }





        [Fact]
        public async Task Login_ReturnsOkResult_WithToken()
        {
            // Arrange
            var loginViewModel = new LoginViewModel
            {
                UserName = "testuser",
                Password = "password"
            };

            var loginResponse = new LoginResponseViewModel
            {
                Id = Guid.NewGuid(),
                UserName = "testuser",
                Token = "sampletoken",
                RefershToken = "samplerefreshtoken", // Corrected spelling
                Expires = DateTime.UtcNow.AddHours(1)
            };

            // Mocking the service method
            _mockAuthService.Setup(s => s.LoginAsync(loginViewModel))
                .ReturnsAsync(loginResponse);

            // Act
            var result = await _controller.Login(loginViewModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<LoginResponseViewModel>(okResult.Value);
            Assert.Equal("sampletoken", returnValue.Token);
            Assert.Equal("samplerefreshtoken", returnValue.RefershToken);
            Assert.Equal("testuser", returnValue.UserName);
            Assert.NotEqual(Guid.Empty, returnValue.Id);
            Assert.True(returnValue.Expires > DateTime.UtcNow);
        }

        [Fact]
        public async Task CheckUserRole_ReturnsOkResult_WithRole()
        {
            // Arrange
            var user = new Users { UserName = "testuser" };
            _mockUserManager.Setup(m => m.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
                .ReturnsAsync(user);
            _mockUserManager.Setup(m => m.IsInRoleAsync(user, "Admin"))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.CheckUserRole();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("User is in Admin role", okResult.Value);
        }

        [Fact]
        public async Task GetUsers_ReturnsOkResult_WithUsers()
        {
            // Arrange
            var users = new List<UserViewModel>
            {
                new UserViewModel { Id = Guid.NewGuid(), Name = "testuser", Email = "test@example.com" }
            };

            _mockAuthService.Setup(s => s.GetAllUsersAsync())
                .ReturnsAsync(users);

            // Act
            var result = await _controller.GetUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<UserViewModel>>(okResult.Value);
            Assert.NotEmpty(returnValue);
        }

        [Fact]
        public async Task GetById_ReturnsOkResult_WithUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userViewModel = new UserViewModel { Id = userId, Name = "testuser", Email = "test@example.com" };

            _mockAuthService.Setup(s => s.GetUserByIdAsync(userId))
                .ReturnsAsync(userViewModel);

            // Act
            var result = await _controller.GetById(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<UserViewModel>(okResult.Value);
            Assert.Equal(userId, returnValue.Id);
        }

        [Fact]
        public async Task UpdateByAdmin_ReturnsOkResult_WhenSuccessful()
        {
            // Arrange
            var userViewModel = new UserViewModel
            {
                Id = Guid.NewGuid(),
                Name = "updateduser",
                Email = "updated@example.com"
            };

            _mockAuthService.Setup(s => s.AdminUpdateUserAsync(userViewModel))
                .ReturnsAsync(true);

            _mockAuthService.Setup(s => s.GetUserByIdAsync(userViewModel.Id))
                .ReturnsAsync(userViewModel);

            // Act
            var result = await _controller.UpdateByAdmin(userViewModel);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task UpdateCustomerAsync_ReturnsOkResult_WhenSuccessful()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new Users { Id = userId, UserName = "testuser" };
            _mockUserManager.Setup(m => m.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
                .ReturnsAsync(user);

            _mockAuthService.Setup(s => s.UpdateCustomerAsync(userId, "newemail@example.com", "NewName"))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateCustomerAsync("NewName", "newemail@example.com");

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteUserAsync_ReturnsNoContent_WhenSuccessful()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _mockAuthService.Setup(s => s.DeleteUserAsync(userId))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteUserAsync(userId);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
        }
    }
}
