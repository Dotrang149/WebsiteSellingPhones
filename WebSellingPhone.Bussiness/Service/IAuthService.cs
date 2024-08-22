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
        Task<IEnumerable<UserViewModel>> GetAllUsersAsync();
        Task<bool> UpdateUserAsync(Users user);
        Task<bool> DeleteUserAsync(Guid userId);
        Task<PaginatedResult<Users>> GetByPagingAsync(
            string filter = "",
            string sortBy = "",
            int pageIndex = 1,
            int pageSize = 10);
    }
}
