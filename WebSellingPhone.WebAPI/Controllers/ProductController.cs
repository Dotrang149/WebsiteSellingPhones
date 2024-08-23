using Microsoft.AspNetCore.Mvc;
using WebSellingPhone.Bussiness.Service;
using WebSellingPhone.Bussiness.ViewModel;
using WebSellingPhone.Bussiness.ViewModel.Mappers;
using WebSellingPhone.Data.Models;

namespace WebSellingPhone.WebAPI.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("get-all-products")]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();
            var productsViewModels = products.ToList().Select(p => p.ToProductVm());
            return Ok(productsViewModels);
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var product = await _productService.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }
            return Ok(product.ToProductVm());
        }

        [HttpPost("create-product")]
        public async Task<IActionResult> Create([FromBody] ProductVm productVm)
        { 
            if (productVm == null)
            {
                return BadRequest("Product data is null");
            }

            var product = productVm.ToProduct();

            var result = await _productService.AddAsync(product);

            if (result > 0)
            {
                return CreatedAtAction(nameof(GetById), new { id = product.Id}, product.ToProductVm());
            }
            return BadRequest("Failed to create product");
        }

        [HttpPut("update-product/{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] ProductVm productVm)
        {
            if (productVm == null || id == Guid.Empty)
            {
                return BadRequest("Dữ liệu sản phẩm không hợp lệ.");
            }

            var existingProduct = await _productService.GetByIdAsync(id);

            if (existingProduct == null)
            {
                return NotFound("Không tìm thấy sản phẩm.");
            }

            // Cập nhật các thuộc tính từ ViewModel sang thực thể hiện tại
            existingProduct.Name = productVm.Name;
            existingProduct.Description = productVm.Description;
            existingProduct.Price = productVm.Price;
            existingProduct.Image = productVm.Image;
            existingProduct.BrandProductId = productVm.BrandProductId;
            existingProduct.PromotionProductId = productVm.PromotionProductId;

            // Cập nhật vào cơ sở dữ liệu
            var result = await _productService.UpdateAsync(existingProduct);

            if (result > 0)
            {
                return NoContent();
            }

            return BadRequest("Cập nhật sản phẩm thất bại.");
        }


        [HttpDelete("delete-product/{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid product Id");
            }

            var result = await _productService.DeleteAsync(id);

            if (result)
            {
                return NoContent();
            }
            return NotFound();
        }

        [HttpGet("get-products-by-paging")]
        public async Task<IActionResult> GetByPaging([FromQuery] string filter = "", [FromQuery] string sortBy = "", [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var paginatedProducts = await _productService.GetByPagingAsync(filter, sortBy, pageIndex, pageSize);    
            return Ok(paginatedProducts);
        }
    }
}
