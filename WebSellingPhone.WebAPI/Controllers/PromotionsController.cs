﻿using Microsoft.AspNetCore.Mvc;
using WebSellingPhone.Bussiness.Service;
using WebSellingPhone.Bussiness.ViewModel;
using WebSellingPhone.Bussiness.ViewModel.Mappers;
using WebSellingPhone.Data.Models;

namespace WebSellingPhone.WebAPI.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class PromotionController : ControllerBase
    {
        private readonly IPromotionService _promotionService;

        public PromotionController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }

        [HttpGet("get-all-promotions")]
        public async Task<IActionResult> GetAll()
        {
            var promotions = await _promotionService.GetAllAsync();
            var promotionViewModels = promotions.ToList().Select(p => p.ToPromotionVm());
            return Ok(promotionViewModels);
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var promotion = await _promotionService.GetByIdAsync(id);

            if (promotion == null)
            {
                return NotFound();
            }
            return Ok(promotion.ToPromotionVm());
        }

        [HttpPost("create-promotion")]
        public async Task<IActionResult> Create([FromBody] PromotionVm promotionVm)
        {
            if (promotionVm == null)
            {
                return BadRequest("Promotion data is null");
            }

            var promotion = promotionVm.ToPromotion();
            var result = await _promotionService.AddAsync(promotion);

            if (result > 0)
            {
                return CreatedAtAction(nameof(GetById), new { id = promotion.Id }, promotion.ToPromotionVm());
            }
            return BadRequest("Failed to create promotion");
        }


        [HttpPut("update-promotion/{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] PromotionVm promotionVm)
        {
            if (promotionVm == null || id == Guid.Empty)
            {
                return BadRequest("Dữ liệu khuyến mãi không hợp lệ.");
            }

            var existingPromotion = await _promotionService.GetByIdAsync(id);

            if (existingPromotion == null)
            {
                return NotFound("Không tìm thấy khuyến mãi.");
            }

            // Cập nhật các thuộc tính từ ViewModel sang thực thể hiện tại
            existingPromotion.Name = promotionVm.Name;
            existingPromotion.Description = promotionVm.Description;
            existingPromotion.DateStart = promotionVm.DateStart;
            existingPromotion.DateEnd = promotionVm.DateEnd;

            // Cập nhật vào cơ sở dữ liệu
            var result = await _promotionService.UpdateAsync(existingPromotion);

            if (result > 0)
            {
                return NoContent();
            }

            return BadRequest("Cập nhật khuyến mãi thất bại.");
        }


        [HttpDelete("delete-promotion/{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid promotion Id");
            }

            var result = await _promotionService.DeleteAsync(id);

            if (result)
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}