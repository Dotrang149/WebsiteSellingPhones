using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebSellingPhone.Bussiness.Service;
using WebSellingPhone.Bussiness.ViewModel;
using WebSellingPhone.Bussiness.ViewModel.Mappers;
using WebSellingPhone.Data.Models;

namespace WebSellingPhone.WebAPI.Controllers
{
    
    [Route("api/[Controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandsController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet("get-all-brands")]
        public async Task<IActionResult> GetAll()
        {
            var brands = await _brandService.GetAllAsync();
            return Ok(brands);
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetById( Guid id)
        {
            var brand = await _brandService.GetByIdAsync(id);

            if (brand == null)
            {
                return NotFound();
            }
            return Ok(brand.ToBrandVm());
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpPost("create-brand")]
        public async Task<IActionResult> Create([FromBody] BrandCreate brandVm)
        {
            if (brandVm == null)
            {
                return BadRequest("Brand data is null");
            }

            var brand = new Brand
            {
                Name = brandVm.Name,
                Description = brandVm.Description
            };

            var result = await _brandService.AddAsync(brand);

            if (result > 0)
            {
                return CreatedAtAction(nameof(GetById), new { id = brand.Id }, brand.ToBrandVm());
            }
            return BadRequest("Failed to create brand");
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpPut("update-brand/{id}")]
        public async Task<IActionResult> Update( [FromBody] BrandVm brandVm)
        {
            if (brandVm == null )
            {
                return BadRequest("Dữ liệu thương hiệu không hợp lệ.");
            }

            var existingBrand = await _brandService.GetByIdAsync(brandVm.Id);

            if (existingBrand == null)
            {
                return NotFound("Không tìm thấy thương hiệu.");
            }

            // Cập nhật các thuộc tính cần thiết từ ViewModel sang thực thể đang được theo dõi
            existingBrand.Name = brandVm.Name;
            existingBrand.Description = brandVm.Description;
            // Thêm các thuộc tính khác nếu cần

            // Cập nhật thông tin trong cơ sở dữ liệu
            var result = await _brandService.UpdateAsync(existingBrand);

            if (result > 0)
            {
                return NoContent();
            }

            return BadRequest("Cập nhật thương hiệu thất bại.");
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("delete-brand/{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid brand Id");
            }

            var result = await _brandService.DeleteAsync(id);

            if (result)
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}