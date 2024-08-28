using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using WebSellingPhone.Bussiness.Service.Base;
using WebSellingPhone.Bussiness.ViewModel;
using WebSellingPhone.Data;
using WebSellingPhone.Data.Infrastructure;
using WebSellingPhone.Data.Models;

namespace WebSellingPhone.Bussiness.Service
{
    public class AuthService : BaseService<Users>,IAuthService
    {
        private readonly UserManager<Users> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly PhoneWebDbContext _context;


        public AuthService(UserManager<Users> userManager,
            RoleManager<Role> roleManager,
            IConfiguration configuration,PhoneWebDbContext context, IUnitOfWork unitOfWork, ILogger<AuthService> logger) : base(unitOfWork,logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return false;
            }
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<IEnumerable<UserViewModel>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var roles = await _roleManager.Roles.ToListAsync();

            var userVms = new List<UserViewModel>();

            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var userVm = new UserViewModel
                {
                    Id = user.Id,
                    Name = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Password = user.PasswordHash,
                    Role = string.Join(",", userRoles)
                };
                userVms.Add(userVm);
            }

            return userVms;
        }

        public async Task<UserViewModel> GetUserByIdAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new FileNotFoundException($"User with ID {userId} not found.");
            }
            var userRoles = await _userManager.GetRolesAsync(user);

            var userVm = new UserViewModel()
            {
                Id = user.Id,
                Name = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Password = user.PasswordHash,
                Role = string.Join(",", userRoles)
            };
            return userVm;
        }

        public async Task<LoginResponseViewModel> LoginAsync(LoginViewModel loginViewModel)
        {
            var user = await _userManager.FindByNameAsync(loginViewModel.UserName);

            if (user == null || !await _userManager.CheckPasswordAsync(user, loginViewModel.Password))
            {
                return null;
            }

            var token = await GenerateJwtTokenAsync(user);

            return new LoginResponseViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Token = token.Token,
                RefershToken = token.RefershToken,
                Expires = token.Expires
            };
        }

        private async Task<LoginResponseViewModel> GenerateJwtTokenAsync(Users user)
        {
            var authClaims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        
    };

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                authClaims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }

            if (await _userManager.IsInRoleAsync(user, "Customer"))
            {
                authClaims.Add(new Claim(ClaimTypes.Role, "Customer"));
            }

            if (!await _userManager.IsInRoleAsync(user, "Admin") && !await _userManager.IsInRoleAsync(user, "Customer"))
            {
                authClaims.Add(new Claim(ClaimTypes.Role, "NO_USER"));
            }


            var authSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.UtcNow.AddMinutes(5),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            var refreshToken = new RefreshToken
            {
                JwtId = token.Id,
                IsRevoked = false,
                UserId = user.Id,
                DateAdded = DateTime.UtcNow,
                DateExpire = DateTime.UtcNow.AddMonths(6),
                Token = Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString()
            };

            await _context.RefreshToken.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            return new LoginResponseViewModel
            {
                Token = jwtToken,
                RefershToken = refreshToken.Token,
                Expires = token.ValidTo
            };
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
                PasswordHash = registerViewModel.Password,
                Email = registerViewModel.Email,
                PhoneNumber = registerViewModel.PhoneNumber
               
            };

            var result = await _userManager.CreateAsync(user, registerViewModel.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Customer");
                return await LoginAsync(new LoginViewModel()
                {
                    UserName = registerViewModel.UserName,
                    Password = registerViewModel.Password
                });
            }
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new ArgumentException($"The user could not be created. Errors: {errors}");
            
           
        }

        public async Task<bool> AdminUpdateUserAsync(UserViewModel userView)
        {
            var existingUser = await _userManager.FindByIdAsync(userView.Id.ToString());

            if (existingUser != null)
            {
                existingUser.Email = userView.Email;
                existingUser.UserName = userView.Name;
                existingUser.PhoneNumber = userView.PhoneNumber;
                existingUser.PasswordHash = userView.Password;

                var result = await _userManager.UpdateAsync(existingUser);
                return result.Succeeded;
            }

            return false;
        }

        public async Task<bool> UpdateCustomerAsync(Guid userId, string newEmail, string newUserName)
        {
            var existingUser = await _userManager.FindByIdAsync(userId.ToString());

            if (existingUser != null)
            {
                existingUser.Email = newEmail;
                existingUser.UserName = newUserName;

                var result = await _userManager.UpdateAsync(existingUser);
                return result.Succeeded;
            }

            return false;
        }

        public async Task<PaginatedResult<Users>> GetByPagingAsync(string filter = "", string sortBy = "", int pageIndex = 1, int pageSize = 20)
        {
            Func<IQueryable<Users>, IOrderedQueryable<Users>> orderBy = null;
            switch (sortBy.ToLower())
            {
                case "id":
                    orderBy = u => u.OrderBy(u => u.Id);
                    break;
                case "name":
                    orderBy = u => u.OrderBy(u => u.UserName);
                    break;
            }
            Expression<Func<Users, bool>> filterQuery = null;

            if (!string.IsNullOrWhiteSpace(filter))
            {
                filterQuery = p => p.UserName.Contains(filter);
            }

            return await GetAsync(filterQuery, orderBy, "", pageIndex, pageSize);
        }

        public async Task<UserViewModel> GetUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                throw new FileNotFoundException($"User with email {email} not found.");
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var userVm = new UserViewModel
            {
                Id = user.Id,
                Name = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Password = user.PasswordHash,
                Role = string.Join(",", userRoles)
            };

            return userVm;
        }
    }

        

    

        

       
   }

