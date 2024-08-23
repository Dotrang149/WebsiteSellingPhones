using WebSellingPhone.Bussiness.ViewModel;
using WebSellingPhone.Data.Models;

namespace WebSellingPhone.Bussiness.ViewModel.Mappers
{
    public static class UserMapper
    {
        public static UserViewModel ToUserVm(this Users user)
        {
            return new UserViewModel
            {
                Id = user.Id,
                Name = user.UserName,
                Email = user.Email,
                // Thêm các thuộc tính khác nếu có
            };
        }

        public static Users ToUser(this UserViewModel userVm)
        {
            return new Users
            {
                Id = userVm.Id,
                UserName = userVm.Name,
                Email = userVm.Email,
                // Thêm các thuộc tính khác nếu có
            };
        }
    }
}
