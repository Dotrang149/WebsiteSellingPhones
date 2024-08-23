using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using WebSellingPhone.Bussiness.Service.Base;
using WebSellingPhone.Bussiness.ViewModel;
using WebSellingPhone.Data.Infrastructure;
using WebSellingPhone.Data.Models;

namespace WebSellingPhone.Bussiness.Service
{
    public class ProductService : BaseService<Product>, IProductService
    {
        public ProductService(IUnitOfWork unitOfWork, ILogger<ProductService> logger) : base(unitOfWork, logger) { }

        public async Task<PaginatedResult<Product>> GetByPagingAsync(string filter = "", string sortBy = "", int pageIndex = 1, int pageSize = 10)
        {
            Func<IQueryable<Product>, IOrderedQueryable<Product>> orderBy = null;
            switch (sortBy.ToLower())
            {
                case "id":
                    orderBy = p => p.OrderBy(p => p.Id);
                    break;
                case "name":
                    orderBy = p => p.OrderBy(p => p.Name);
                    break;
                case "price":
                    orderBy = p => p.OrderBy(p => p.Price);
                    break;
            }
            Expression<Func<Product, bool>> filterQuery = null;

            if (!string.IsNullOrWhiteSpace(filter))
            {
                filterQuery = p => p.Name.Contains(filter);
            }

            return await GetAsync(filterQuery, orderBy, "", pageIndex, pageSize);
        }
    }
}
