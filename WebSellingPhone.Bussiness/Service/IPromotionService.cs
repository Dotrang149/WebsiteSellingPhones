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
    public interface IPromotionService : IBaseService<Promotion>
    {
        //Task<Promotion> CreatePromotion(PromotionCreate promotionCreate);
        //Task<Promotion> UpdatePromotion(PromotionVm promotion);

        Task<PaginatedResult<Promotion>> GetByPagingAsync(
            string filter = "",
            string sortBy = "",
            int pageIndex = 1,
            int pageSize = 10);
    }
}
