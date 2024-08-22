using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using WebSellingPhone.Bussiness.Service.Base;
using WebSellingPhone.Bussiness.ViewModel;
using WebSellingPhone.Data.Infrastructure;
using WebSellingPhone.Data.Models;

namespace WebSellingPhone.Bussiness.Service
{
    public class UserService : BaseService<Users>,IUserService
    {
        public UserService(IUnitOfWork unitOfWork, ILogger<UserService> logger ): base(unitOfWork, logger) { }

        public async Task<PaginatedResult<Users>> GetByPagingAsync(string filter = "", string sortBy = "", int pageIndex = 1, int pageSize = 10)
        {
            Func<IQueryable<Users>, IOrderedQueryable<Users>> orderBy = null;
            switch(sortBy.ToLower())
            {
                case "id": 
                    orderBy = u => u.OrderBy(u => u.Id);
                    break;
                case "name":
                    orderBy = u => u.OrderBy(u => u.UserName);
                    break;
            }
            Expression<Func<Users, bool>> filterQuery = null;

            if (!string.IsNullOrWhiteSpace(filter))
            {
                filterQuery = p => p.UserName.Contains(filter);
            }

            return await GetAsync(filterQuery, orderBy, "", pageIndex, pageSize);
        }
    }
}
