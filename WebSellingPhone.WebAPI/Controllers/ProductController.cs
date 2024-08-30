using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebSellingPhone.Bussiness.Service;
using WebSellingPhone.Bussiness.ViewModel;
using WebSellingPhone.Bussiness.ViewModel.Mappers;
using WebSellingPhone.Data;
using WebSellingPhone.Data.Models;

namespace WebSellingPhone.WebAPI.Controllers
{
    //[Authorize(Policy ="AdminOnly")]
    [Route("api/[Controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly PhoneWebDbContext _context;
        public ProductController(IProductService productService, PhoneWebDbContext context)
        {
            _productService = productService;
            _context = context; 
        }

        [HttpGet("get-all-products")]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();
            
            return Ok(products);
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetById( Guid id)
        {
            var product = await _productService.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }
            return Ok(product.ToProductVm());
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpPost("create-product")]
        public async Task<IActionResult> Create([FromBody] ProductCreate productCreate)
        {
            var product = await _productService.CreateProduct(productCreate);
            if (product == null)
            {
                return BadRequest("Fail");
            }
            return Ok();
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpPut("update-product/{id}")]
        public async Task<IActionResult> Update( [FromBody] ProductVm productVm)
        {
            var product = await _productService.UpdateProduct(productVm);
            return Ok(product);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("delete-product/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid product Id");
            }

            var result = await _productService.DeleteAsync(id);

            if (result)
            {
                return Ok();
            }
            return NotFound();
        }

        [HttpGet("get-products-by-paging")]
        public async Task<IActionResult> GetByPaging([FromQuery] string filter = "", [FromQuery] string sortBy = "", [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 20)
        {
            var paginatedProducts = await _productService.GetByPagingAsync(filter, sortBy, pageIndex, pageSize);
            return Ok(paginatedProducts);
        }

        [HttpGet("filter-products")]
        public async Task<IActionResult> FilterProducts([FromQuery] decimal? maxPrice, [FromQuery] string? brand)
        {
            var query = _context.Products.AsQueryable();

            // Lọc theo giá nếu maxPrice có giá trị
            if (maxPrice.HasValue && !string.IsNullOrEmpty(brand))
            {
                var normalizedBrand = brand.ToLower(); // Hoặc brand.ToUpper()
                query = query.Where(p => p.Price <= maxPrice.Value && p.Brand.Name.Equals(normalizedBrand, StringComparison.CurrentCultureIgnoreCase));
            }
            else if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }
            else if (!string.IsNullOrEmpty(brand))
            {
                var normalizedBrand = brand.ToLower(); // Hoặc brand.ToUpper()
                query = query.Where(p => p.Brand.Name.ToLower() == normalizedBrand);
            }

            // Lấy danh sách sản phẩm
            var products = await query.ToListAsync();

            return Ok(products);
        }
    }
}