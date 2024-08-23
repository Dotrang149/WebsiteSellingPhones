using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebSellingPhone.Bussiness.Service;
using WebSellingPhone.Bussiness.ViewModel;
using WebSellingPhone.Data.Models;

namespace WebSellingPhone.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<Users> _userManager;

        public UserController(IAuthService authService, UserManager<Users> userManager)
        {
            _authService = authService;
            _userManager = userManager;
        }

        [HttpGet("check-role")]
        public async Task<IActionResult> CheckUserRole()
        {
            // Lấy người dùng hiện tại
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized("User not found");
            }

            // Kiểm tra vai trò của người dùng
            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return Ok("User is in Admin role");
            }
            else if(await _userManager.IsInRoleAsync(user,"Customer"))
            {
                return Ok("User is in Customer role");
            }
            else
            {
                return Ok("User is not in role");
            }
        }


        [Authorize(Policy = "AdminOnly")]
        [HttpGet("Get-All-Users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _authService.GetAllUsersAsync();
            return Ok(users);
        }


        [Authorize(Policy = "AdminOnly")]
        [HttpGet("Get-by-id/{id}")]
        public async Task<IActionResult> GetById(Guid Id)
        {
            var userVm = await _authService.GetUserByIdAsync(Id);
            return Ok(userVm);
        }



        [Authorize(Policy = "AdminOny")]
        [HttpGet("get-user-by-page")]
        public async Task<IActionResult> GetUsersByPage([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string filter = "", [FromQuery] string sortBy = "")
        {

            return Ok(_authService.GetByPagingAsync(filter, sortBy, page, pageSize));
        }



        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel register)
        {
            try
            {
                var loginResponse = await _authService.RegisterAsync(register);
                return Created(nameof(Register), $"User {register.Email} created!");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while registering the user.");
            }
        }




        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var authResult = await _authService.LoginAsync(loginViewModel);

            if (authResult == null)
            {
                return Unauthorized();
            }

            return Ok(authResult);
        }



        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUserAsync(Guid id)
        {
            try
            {
                var deleted = await _authService.DeleteUserAsync(id);
                if (deleted)
                {
                    return NoContent();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the user.");
            }
        }


        [Authorize(Policy = "AdminOnly")]
        [HttpPut("users-by-admin")]
        public async Task<IActionResult> UpdateByAdmin(Guid id, [FromBody] UserViewModel userViewModel)
        {
            try
            {
                var existingUser = await _authService.GetUserByIdAsync(id);
                if (existingUser == null)
                {
                    return NotFound($"User with ID {id} not found.");
                }

                var updated = await _authService.AdminUpdateUserAsync(id,userViewModel);
                if (updated)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("Failed to update the user.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the user.");
            }
        }




        [Authorize(Policy = "CustomerOnly")]
        [HttpPut("users-by-customer")]
        public async Task<IActionResult> UpdateCustomerAsync(string name , string email)
        {
            try
            {
                var users = await _userManager.GetUserAsync(User);
                var updated = await _authService.UpdateCustomerAsync(users.Id,email,name);
                if (updated)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("Failed to update the user.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the user.");
            }
        }



        [Authorize(Roles = "Admin")]
        [HttpGet("users/paging")]
        public async Task<IActionResult> GetUsersByPagingAsync(string filter = "", string sortBy = "", int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var result = await _authService.GetByPagingAsync(filter, sortBy, pageIndex, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching users.");
            }
        }

    }
}
