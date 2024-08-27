using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSellingPhone.Bussiness.ViewModel;
using WebSellingPhone.Data.Models;

namespace WebSellingPhone.Bussiness.Service
{
    public interface IAuthService
    {
        Task<LoginResponseViewModel> LoginAsync(LoginViewModel loginViewModel);
        Task<LoginResponseViewModel> RegisterAsync(RegisterViewModel registerViewModel);

        Task<UserViewModel> GetUserByIdAsync(Guid userId);
        Task<UserViewModel> GetUserByEmail (string email);
        Task<IEnumerable<UserViewModel>> GetAllUsersAsync();
        Task<bool> UpdateCustomerAsync(Guid userId, string newEmail, string newUserName);
        Task<bool> AdminUpdateUserAsync( UserViewModel userView);
        Task<bool> DeleteUserAsync(Guid userId);
        Task<PaginatedResult<Users>> GetByPagingAsync(
            string filter = "",
            string sortBy = "",
            int pageIndex = 1,
            int pageSize = 20);
    }
}
