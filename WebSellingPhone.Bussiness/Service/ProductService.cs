using Microsoft.EntityFrameworkCore;
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


        public async Task<Product> CreateProduct(ProductCreate productDto)
        {
            // Kiểm tra xem Brand có tồn tại hay không
            var brand = await _unitOfWork.BrandRepository.GetQuery(b => b.Name == productDto.BrandName).FirstOrDefaultAsync();

            var promotion = await _unitOfWork.PromotionRepository.GetQuery(p => p.Name == productDto.PromotionName).FirstOrDefaultAsync();

            if (promotion == null)
            {
                promotion = new Promotion { Name = productDto.PromotionName, Description = "null"};
                _unitOfWork.PromotionRepository.Add(promotion);
                await _unitOfWork.SaveChangesAsync();
            }
            if (brand == null)
            {
                // Nếu chưa tồn tại, tạo một Brand mới
                brand = new Brand { Name = productDto.BrandName,Description = "null" };
                _unitOfWork.BrandRepository.Add(brand);
                await _unitOfWork.SaveChangesAsync();
            }

            // Tạo Product mới với BrandId tương ứng
            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Image = productDto.Image,
                PromotionProductId = promotion.Id,
                BrandProductId = brand.Id
            };
            await AddAsync(product);
            return product;
        }

        public async Task<Product> UpdateProduct(ProductVm productVm)
        {
            // Lấy Product cần sửa
            var product = await GetByIdAsync(productVm.Id);
            if (product == null)
                throw new Exception($"Product with ID {productVm.Id} not found.");

            // Kiểm tra xem Brand có tồn tại hay không
            var brand = await _unitOfWork.BrandRepository.GetQuery(b => b.Name == productVm.BrandName).FirstOrDefaultAsync();

            var promotion = await _unitOfWork.PromotionRepository.GetQuery(p => p.Name == productVm.PromotionName).FirstOrDefaultAsync();

            if (promotion == null)
            {
                promotion = new Promotion { Name = productVm.Name,Description = "null" };
                _unitOfWork.PromotionRepository.Add(promotion);
                await _unitOfWork.SaveChangesAsync();
            }

            if (brand == null)
            {
                // Nếu chưa tồn tại, tạo một Brand mới
                brand = new Brand { Name = productVm.BrandName,Description = "null" };
                 _unitOfWork.BrandRepository.Add(brand);
                await _unitOfWork.SaveChangesAsync();
            }

            // Cập nhật thông tin Product
            product.Name = productVm.Name;
            product.Description = productVm.Description;
            product.Price = productVm.Price;
            product.Image = productVm.Image;
            product.PromotionProductId = promotion.Id;
            product.BrandProductId = brand.Id;
            await UpdateAsync(product);
            return product;
        }


        public async Task<PaginatedResult<Product>> GetByPagingAsync(string filter = "", string sortBy = "", int pageIndex = 1, int pageSize = 20)
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
