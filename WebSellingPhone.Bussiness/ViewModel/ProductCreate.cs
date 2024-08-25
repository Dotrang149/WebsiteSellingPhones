using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSellingPhone.Bussiness.ViewModel
{
    public class ProductCreate
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Image {  get; set; }
        public string BrandName { get; set; }
        public string PromotionName { get; set; }
    }
}
