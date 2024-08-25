using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSellingPhone.Bussiness.ViewModel
{
    public class ProductCreate
    {
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal Price { get; set; }
        public string Image {  get; set; }
    }
}
