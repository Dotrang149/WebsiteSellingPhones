using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebSellingPhone.Bussiness.Service.Base;
using WebSellingPhone.Bussiness.ViewModel;
using WebSellingPhone.Data.Infrastructure;
using WebSellingPhone.Data.Models;

namespace WebSellingPhone.Bussiness.Service
{
    public class OrderService: BaseService<Order>, IOrderService
    {
        public OrderService(IUnitOfWork unitOfWork, ILogger<OrderService> logger) : base(unitOfWork, logger) { }


        public async Task<PaginatedResult<Order>> GetByPagingAsync(string filter = "", string sortBy = "", int pageIndex = 1, int pageSize = 10)
        {
            Func<IQueryable<Order>, IOrderedQueryable<Order>> orderBy = null;

            switch (sortBy.ToLower())
            {
                case "payment":
                    orderBy = q => q.OrderBy(p => p.PaymentMethod);
                    break;
                case "id":
                    orderBy = q => q.OrderBy(p => p.Id);
                    break;

            }

            Expression<Func<Order, bool>> filterQuery = null;

            if (!string.IsNullOrWhiteSpace(filter))
            {
                filterQuery = p => p.PaymentMethod.Contains(filter);
            }

            return await GetAsync(filterQuery, orderBy, "", pageIndex, pageSize);

        }


    }
}
