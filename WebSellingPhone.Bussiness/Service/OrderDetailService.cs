using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebSellingPhone.Bussiness.Service.Base;
using WebSellingPhone.Bussiness.ViewModel;
using WebSellingPhone.Data;
using WebSellingPhone.Data.Infrastructure;
using WebSellingPhone.Data.Models;
using WebSellingPhone.Data.Repository;

namespace WebSellingPhone.Bussiness.Service
{
    public class OrderDetailService : BaseService<OrderDetail>, IOrderDetailService
    {
        private readonly PhoneWebDbContext _context;
        public OrderDetailService(IUnitOfWork unitOfWork, ILogger<OrderDetailService> logger, PhoneWebDbContext context) : base(unitOfWork, logger) 
        {
            _context = context;
        }

        public Task<PaginatedResult<OrderDetail>> GetByPagingAsync(string filter = "", string sortBy = "", int pageIndex = 1, int pageSize = 10)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(Guid orderId)
        {
            try
            {
                return await _context.OrderDetails.Where(o => o.OrderId == orderId).ToListAsync(); // Sử dụng _context
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting order details by order ID.");
                return new List<OrderDetail>();
            }
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsByProductIdAsync(Guid productId)
        {
            try
            {
                return await _context.OrderDetails.Where(o => o.ProductId == productId).ToListAsync(); // Sử dụng _context
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting order details by product ID.");
                return new List<OrderDetail>();
            }
        }

        //public async Task<PaginatedResult<OrderDetail>> GetByPagingAsync(string filter = "", string sortBy = "", int pageIndex = 1, int pageSize = 10)
        //{
        //    Func<IQueryable<OrderDetail>, IOrderedQueryable<OrderDetail>> orderBy = null;

        //    switch (sortBy.ToLower())
        //    {
        //        case "price":
        //            orderBy = q => q.OrderBy(p => p.Price);
        //            break;
        //        case "product":
        //            orderBy = q => q.OrderBy(p => p.Products);
        //            break;

        //    }

        //    Expression<Func<OrderDetail, bool>> filterQuery = null;

        //    if (!string.IsNullOrWhiteSpace(filter))
        //    {
        //        filterQuery = p => p.Price.Contains(filter);
        //    }

        //    return await GetAsync(filterQuery, orderBy, "", pageIndex, pageSize);

        //}

    }
}
