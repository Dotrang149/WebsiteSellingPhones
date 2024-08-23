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
            var brandsViewModels = brands.Select(b => b.ToBrandVm());
            return Ok(brandsViewModels);
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var brand = await _brandService.GetByIdAsync(id);

            if (brand == null)
            {
                return NotFound();
            }
            return Ok(brand.ToBrandVm());
        }

        [HttpPost("create-brand")]
        public async Task<IActionResult> Create([FromBody] BrandVm brandVm)
        {
            if (brandVm == null)
            {
                return BadRequest("Brand data is null");
            }

            var brand = brandVm.ToBrand();

            var result = await _brandService.AddAsync(brand);

            if (result > 0)
            {
                return CreatedAtAction(nameof(GetById), new { id = brand.Id }, brand.ToBrandVm());
            }
            return BadRequest("Failed to create brand");
        }

        [HttpPut("update-brand/{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] BrandVm brandVm)
        {
            if (brandVm == null || id == Guid.Empty)
            {
                return BadRequest("Dữ liệu thương hiệu không hợp lệ.");
            }

            var existingBrand = await _brandService.GetByIdAsync(id);

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
