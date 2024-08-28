using WebSellingPhone.Bussiness.Service.Base;
using WebSellingPhone.Bussiness.ViewModel;
using WebSellingPhone.Data.Models;

namespace WebSellingPhone.Bussiness.Service
{
    public interface IBrandService : IBaseService<Brand>
    {

        //Task<Brand> CreateBrand(BrandCreate brandCreate);
        //Task<Brand> UpdateProduct(BrandVm brand);
        Task<PaginatedResult<Brand>> GetByPagingAsync(
            string filter = "",
            string sortBy = "",
            int pageIndex = 1,
            int pageSize = 10);
    }
}
