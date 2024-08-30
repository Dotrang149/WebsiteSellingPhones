using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSellingPhone.Bussiness.Service.Base;
using WebSellingPhone.Bussiness.ViewModel;
using WebSellingPhone.Data.Models;

namespace WebSellingPhone.Bussiness.Service
{
    public interface IOrderService : IBaseService<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(Guid userId);
        Task<PaginatedResult<Order>> GetByPagingAsync(
            string filter = "",
            string sortBy = "",
            int pageIndex = 1,
            int pageSize = 10);

        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(Guid userId);

    }
}
