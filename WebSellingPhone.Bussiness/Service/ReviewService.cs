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
    public class ReviewService : BaseService<Review>, IReviewService
    {
        public ReviewService(IUnitOfWork unitOfWork, ILogger<ReviewService> logger) : base(unitOfWork, logger) { }

        public async Task<PaginatedResult<Review>> GetByPagingAsync(string filter = "", string sortBy = "", int pageIndex = 1, int pageSize = 10)
        {
            Func<IQueryable<Review>, IOrderedQueryable<Review>> orderBy = null;
            switch (sortBy.ToLower())
            {
                case "UserId":
                    orderBy = u => u.OrderBy(u => u.UserId);
                    break;
                case "ProductId":
                    orderBy = u => u.OrderBy(u => u.ProductId);
                    break;
            }
            Expression<Func<Review, bool>> filterQuery = null;

            if (!string.IsNullOrWhiteSpace(filter))
            {
                filterQuery = p => p.Comment.Contains(filter);
            }

            return await GetAsync(filterQuery, orderBy, "", pageIndex, pageSize);
        }
    }
}
