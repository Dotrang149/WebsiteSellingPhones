using WebSellingPhone.Bussiness.Service.Base;
using WebSellingPhone.Bussiness.ViewModel;
using WebSellingPhone.Data.Models;

namespace WebSellingPhone.Bussiness.Service
{
    public interface  IProductService : IBaseService<Product>
    {
        Task<Product> CreateProduct(ProductCreate product);
        Task<Product> UpdateProduct(ProductVm product);
        Task<PaginatedResult<Product>> GetByPagingAsync(
            string filter = "",
            string sortBy = "",
            int pageIndex = 1,
            int pageSize = 20);
    }
}
