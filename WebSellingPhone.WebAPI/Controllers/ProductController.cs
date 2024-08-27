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

        [HttpPut("update-product/{id}")]
        public async Task<IActionResult> Update( [FromBody] ProductVm productVm)
        {
            var product = await _productService.UpdateProduct(productVm);
            return Ok(product);
        }


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
    }
}