using WebSellingPhone.Bussiness.ViewModel;
using WebSellingPhone.Data.Models;

namespace WebSellingPhone.Bussiness.ViewModel.Mappers
{
    public static class BrandMapper
    {
        public static BrandVm ToBrandVm(this Brand brand)
        {
            return new BrandVm
            {
                Id = brand.Id,
                Name = brand.Name,
                Description = brand.Description,
                // Thêm các thuộc tính khác nếu có
            };
        }

        public static Brand ToBrand(this BrandVm brandVm)
        {
            return new Brand
            {
                Id = brandVm.Id,
                Name = brandVm.Name,
                Description = brandVm.Description,

                // Thêm các thuộc tính khác nếu có
            };
        }
    }
}