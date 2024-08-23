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
    public interface IOrderDetailService : IBaseService<OrderDetail>
    {
        Task<PaginatedResult<OrderDetail>> GetByPagingAsync(
            string filter = "",
            string sortBy = "",
            int pageIndex = 1,
            int pageSize = 10);

        Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(Guid orderId);
        Task<IEnumerable<OrderDetail>> GetOrderDetailsByProductIdAsync(Guid productId);
    }

   
}
