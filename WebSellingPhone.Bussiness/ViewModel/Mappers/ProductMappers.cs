using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSellingPhone.Data.Models;

namespace WebSellingPhone.Bussiness.ViewModel.Mappers
{
    public static class ProductMappers
    {
        public static ProductVm ToProductVm(this Product product)
        {
            return new ProductVm
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Image = product.Image,
               // BrandProductId = product.BrandProductId
            };
        }

        public static Product ToProduct(this ProductVm productVm)
        {
            return new Product
            {
                Id = productVm.Id,
                Name = productVm.Name,
                Description = productVm.Description,
                Price = productVm.Price,
                Image = productVm.Image,
                //BrandProductId = productVm.BrandProductId

            };
        }
    }
}