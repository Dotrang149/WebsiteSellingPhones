using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSellingPhone.Data.Models;

namespace WebSellingPhone.Bussiness.ViewModel.Mappers
{
    public static class PromotionMapper
    {
        public static PromotionVm ToPromotionVm(this Promotion promotion)
        {
            return new PromotionVm
            {
                Id = promotion.Id,
                Name = promotion.Name,
                Description = promotion.Description,
                DateStart = promotion.DateStart,
                DateEnd = promotion.DateEnd,
                // Thêm các thuộc tính khác nếu có
            };
        }

        public static Promotion ToPromotion(this PromotionVm promotionVm)
        {
            return new Promotion
            {
                Id = (Guid)promotionVm.Id,
                Name = promotionVm.Name,
                Description = promotionVm.Description,
                DateStart = promotionVm.DateStart,
                DateEnd = promotionVm.DateEnd,
                // Thêm các thuộc tính khác nếu có
            };
        }
    }
}