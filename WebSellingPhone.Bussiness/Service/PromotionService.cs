using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using WebSellingPhone.Bussiness.Service.Base;
using WebSellingPhone.Bussiness.ViewModel;
using WebSellingPhone.Data.Infrastructure;
using WebSellingPhone.Data.Models;

namespace WebSellingPhone.Bussiness.Service
{
    public class PromotionService : BaseService<Promotion>, IPromotionService
    {
        public PromotionService(IUnitOfWork unitOfWork, ILogger<PromotionService> logger) : base(unitOfWork, logger) { }

        public async Task<PaginatedResult<Promotion>> GetByPagingAsync(string filter = "", string sortBy = "", int pageIndex = 1, int pageSize = 10)
        {
            Func<IQueryable<Promotion>, IOrderedQueryable<Promotion>> orderBy = null;
            switch (sortBy.ToLower())
            {
                case "id":
                    orderBy = p => p.OrderBy(p => p.Id);
                    break;
                case "description":
                    orderBy = p => p.OrderBy(p => p.Description);
                    break;
                case "date_start":
                    orderBy = p => p.OrderBy(p => p.DateStart);
                    break;
                case "date_end":
                    orderBy = p => p.OrderBy(p => p.DateEnd);
                    break;
            }

            Expression<Func<Promotion, bool>> filterQuery = null;
            if (!string.IsNullOrWhiteSpace(filter))
            {
                filterQuery = p => p.Description.Contains(filter);
            }

            return await GetAsync(filterQuery, orderBy, "", pageIndex, pageSize);
        }
    }
}