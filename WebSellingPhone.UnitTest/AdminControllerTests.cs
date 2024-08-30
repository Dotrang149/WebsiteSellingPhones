using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebSellingPhone.Data.Models;
using WebSellingPhone.WebAPI.Controllers;
using Xunit;
using FluentAssertions;
using System.Threading.Tasks;
using WebSellingPhone.Bussiness.ViewModel;


namespace WebSellingPhone.UnitTest
{
    public class AdminControllerTests
    {
        private readonly Mock<UserManager<Users>> _mockUserManager;
        private readonly Mock<RoleManager<Role>> _mockRoleManager;
        private readonly AdminController _controller;

        public AdminControllerTests()
        {
            _mockUserManager = MockUserManager;
            _mockRoleManager = MockRoleManager();
            _controller = new AdminController(_mockUserManager.Object, _mockRoleManager.Object);
        }

        private static Mock<UserManager<Users>> MockUserManager
        {
            get
            {
                var store = new Mock<IUserStore<Users>>();
                return new Mock<UserManager<Users>>(store.Object, null, null, null, null, null, null, null, null);
            }
        }

        private static Mock<RoleManager<Role>> MockRoleManager()
        {
            var store = new Mock<IRoleStore<Role>>();
            return new Mock<RoleManager<Role>>(store.Object, null, null, null, null);
        }

        [Fact]
        public async Task CreateUser_ValidRequest_ReturnsOk()
        {
            // Arrange
            var request = new CreateUserRequest { UserName = "testuser", Email = "testuser@example.com", Password = "Password123!", Role = "Admin" };
            var user = new Users { UserName = request.UserName, Email = request.Email };

            _mockUserManager.Setup(um => um.CreateAsync(It.IsAny<Users>(), It.IsAny<string>()))
                            .ReturnsAsync(IdentityResult.Success);

            _mockUserManager.Setup(um => um.AddToRoleAsync(It.IsAny<Users>(), It.IsAny<string>()))
                            .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _controller.CreateUser(request);

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task RemoveRole_UserExistsAndRoleExists_ReturnsOk()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new AdminController.RemoveRoleRequest { UserId = "1", RoleName = "Admin" };
            var user = new Users { Id = userId, UserName = "testuser" };

            _mockUserManager.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
                            .ReturnsAsync(user);

            _mockUserManager.Setup(um => um.RemoveFromRoleAsync(It.IsAny<Users>(), It.IsAny<string>()))
                            .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _controller.RemoveRole(request);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                  .Which.Value.Should().Be($"User removed from role {request}");
        }

        [Fact]
        public async Task RemoveRole_UserDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var request = new AdminController.RemoveRoleRequest { UserId = "1", RoleName = "Admin" };

            _mockUserManager.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
                            .ReturnsAsync((Users)null);

            // Act
            var result = await _controller.RemoveRole(request);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>()
                  .Which.Value.Should().Be("User not found");
        }

        [Fact]
        public async Task CreateRole_ValidRequest_ReturnsOk()
        {
            // Arrange
            var request = new AdminController.CreateRoleRequest { RoleName = "NewRole", RoleDescription = "New Role Description" };

            _mockRoleManager.Setup(rm => rm.RoleExistsAsync(It.IsAny<string>()))
                            .ReturnsAsync(false);

            _mockRoleManager.Setup(rm => rm.CreateAsync(It.IsAny<Role>()))
                            .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _controller.CreateRole(request);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                  .Which.Value.Should().Be("Role created successfully.");
        }

        [Fact]
        public async Task CreateRole_RoleAlreadyExists_ReturnsBadRequest()
        {
            // Arrange
            var request = new AdminController.CreateRoleRequest { RoleName = "ExistingRole", RoleDescription = "Existing Role Description" };

            _mockRoleManager.Setup(rm => rm.RoleExistsAsync(It.IsAny<string>()))
                            .ReturnsAsync(true);

            // Act
            var result = await _controller.CreateRole(request);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                  .Which.Value.Should().Be("Role already exists.");
        }
    }
}