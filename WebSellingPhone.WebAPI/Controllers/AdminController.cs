using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebSellingPhone.Bussiness.ViewModel;
using WebSellingPhone.Data.Models;

namespace WebSellingPhone.WebAPI.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<Users> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public AdminController(UserManager<Users> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest createUserRequest)
        {

            var user = new Users { UserName = createUserRequest.UserName, Email = createUserRequest.Email};
            
            
            var result = await _userManager.CreateAsync(user, createUserRequest.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, createUserRequest.Role);
                return Ok();
            }
            return BadRequest();
        }

        //[HttpPost("AssignRole")]
        //public async Task<IActionResult> AssignRole(string userId, string roleName)
        //{
        //    var user = await _userManager.FindByIdAsync(userId);
        //    if (user == null)
        //    {
        //        return NotFound("User not found");
        //    }

        //    var result = await _userManager.AddToRoleAsync(user, roleName);
        //    if (result.Succeeded)
        //    {
        //        return Ok($"User assigned to role {roleName}");
        //    }
        //    return BadRequest(result.Errors);
        //}

        [HttpPost("RemoveRole")]
        public async Task<IActionResult> RemoveRole([FromBody] RemoveRoleRequest removeRole)
        {
            var user = await _userManager.FindByIdAsync(removeRole.UserId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var result = await _userManager.RemoveFromRoleAsync(user, removeRole.RoleName);
            if (result.Succeeded)
            {
                return Ok($"User removed from role {removeRole}");
            }
            return BadRequest(result.Errors);
        }

        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest createRole)
        {
            var roleExists = await _roleManager.RoleExistsAsync(createRole.RoleName);
            if (roleExists)
            {
                return BadRequest("Role already exists.");
            }

            var role = new Role { Name = createRole.RoleName, Description = createRole.RoleDescription};
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return Ok("Role created successfully.");
            }
            return BadRequest(result.Errors);
        }

        [HttpPost("DeleteRole")]
        public async Task<IActionResult> DeleteRole([FromBody] DeleteRoleRequest deleteRole)
        {
            var role = await _roleManager.FindByNameAsync(deleteRole.RoleName);    
            if (role == null)
            {
                return NotFound("Role not found");
            }

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return Ok("Role deleted successfully.");
            }
            return BadRequest(result.Errors);
        }


        public class RemoveRoleRequest
        {
            public string? UserId { get; set; }
            public string? RoleName { get; set; }
        }

        public class CreateRoleRequest
        {
            public string RoleName { get; set; }
            public string RoleDescription { get; set; }
        }

        public class DeleteRoleRequest
        {
            public string RoleName { get; set; }
        }
    }
}
