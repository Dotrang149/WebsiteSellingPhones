using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSellingPhone.Bussiness.ViewModel;
using WebSellingPhone.Data.Models;

namespace WebSellingPhone.Bussiness.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<Users> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IConfiguration _configuration;


        public AuthService(UserManager<Users> userManager,
            RoleManager<Role> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }
        public Task<LoginResponseViewModel> LoginAsync(LoginViewModel loginViewModel)
        {
            throw new NotImplementedException();
        }

        public async Task<LoginResponseViewModel> RegisterAsync(RegisterViewModel registerViewModel)
        {
            var existingUser = await _userManager.FindByNameAsync(registerViewModel.UserName);

            if (existingUser != null)
            {
                throw new ArgumentException("User already exists!");
            }

            var user = new Users()
            {
                UserName = registerViewModel.UserName,
                PasswordHash = registerViewModel.Password
            };

            var result = await _userManager.CreateAsync(user, registerViewModel.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                throw new ArgumentException($"The user could not be created. Errors: {errors}");
            }

            return await LoginAsync(new LoginViewModel()
            {
                UserName = registerViewModel.UserName,
                Password = registerViewModel.Password
            });
        }
    }
}
