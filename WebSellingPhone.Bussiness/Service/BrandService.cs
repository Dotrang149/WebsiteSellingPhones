using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using WebSellingPhone.Bussiness.Service.Base;
using WebSellingPhone.Bussiness.ViewModel;
using WebSellingPhone.Data.Infrastructure;
using WebSellingPhone.Data.Models;

namespace WebSellingPhone.Bussiness.Service
{
    public class BrandService : BaseService<Brand>, IBrandService
    {
        public BrandService(IUnitOfWork unitOfWork, ILogger<BrandService> logger) : base(unitOfWork, logger) { }

        //public Task<Brand> CreateBrand(BrandCreate brandCreate)
        //{
        //    if (brandCreate == null)
        //    {
        //        throw new ArgumentNullException(nameof(brandCreate));
        //    }

        //    var ProductList = _unitOfWork.BrandRepository.GetQuery(b => b.Name == brandCreate.Name).ToList();

        //    var brand = new Brand
        //    {
        //        Name = brandCreate.Name,
        //        Description = brandCreate.Description,
   
        //    }
        //}

        //public Task<Brand> UpdateProduct(BrandVm brand)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<PaginatedResult<Brand>> GetByPagingAsync(string filter = "", string sortBy = "", int pageIndex = 1, int pageSize = 10)
        {
            Func<IQueryable<Brand>, IOrderedQueryable<Brand>> orderBy = null;
            switch (sortBy.ToLower())
            {
                case "id":
                    orderBy = b => b.OrderBy(b => b.Id);
                    break;
                case "name":
                    orderBy = b => b.OrderBy(b => b.Name);
                    break;
            }

            Expression<Func<Brand, bool>> filterQuery = null;
            if (!string.IsNullOrWhiteSpace(filter))
            {
                filterQuery = b => b.Name.Contains(filter);
            }

            return await GetAsync(filterQuery, orderBy, "", pageIndex, pageSize);
        }

        
    }
}