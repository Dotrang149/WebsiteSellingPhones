using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using WebSellingPhone.Data.Models;

namespace WebSellingPhone.Data.Data
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<Users>>();
            string[] roleNames = { "Admin", "Customer", "NO_USER" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    // Tạo các vai trò nếu chưa tồn tại
                    roleResult = await roleManager.CreateAsync(new Role 
                    {
                        Name = roleName, Description = $"{roleName} role"
                    });
                }
            }

            var adminEmail = "admin@admin.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var user = new Users
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }

        }
    }

}
