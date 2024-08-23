using Microsoft.AspNetCore.Mvc;
using WebSellingPhone.Bussiness.Service;
using WebSellingPhone.Bussiness.ViewModel;
using WebSellingPhone.Bussiness.ViewModel.Mappers;
using WebSellingPhone.Data.Models;

namespace WebSellingPhone.WebAPI.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly IOrderDetailService _orderDetailService;

        public OrderDetailsController(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }

        [HttpGet("get-all-orderdetails")]
        public async Task<IActionResult> GetAll()
        {
            var orderDetails = await _orderDetailService.GetAllAsync();
            var orderDetailsViewModels = orderDetails.ToList().Select(od => od.ToOrderDetailVm());
            return Ok(orderDetailsViewModels);
        }

        [HttpGet("get-by-id/{orderId}/{productId}")]
        public async Task<IActionResult> GetById([FromRoute] Guid orderId, [FromRoute] Guid productId)
        {
            var orderDetail = await _orderDetailService.GetByIdAsync(orderId, productId);

            if (orderDetail == null)
            {
                return NotFound();
            }
            return Ok(orderDetail.ToOrderDetailVm());
        }

        [HttpPost("create-orderdetail")]
        public async Task<IActionResult> Create([FromBody] OrderDetailVm orderDetailVm)
        {
            if (orderDetailVm == null)
            {
                return BadRequest("Order detail data is null");
            }

            var orderDetail = orderDetailVm.ToOrderDetail();
            var result = await _orderDetailService.AddAsync(orderDetail);

            if (result > 0)
            {
                return CreatedAtAction(nameof(GetById), new { orderId = orderDetail.OrderId, productId = orderDetail.ProductId }, orderDetail.ToOrderDetailVm());
            }
            return BadRequest("Failed to create order detail");
        }

        [HttpPut("update-orderdetail/{orderId}/{productId}")]
        public async Task<IActionResult> Update([FromRoute] Guid orderId, [FromRoute] Guid productId, [FromBody] OrderDetailVm orderDetailVm)
        {
            if (orderDetailVm == null || orderId == Guid.Empty || productId == Guid.Empty)
            {
                return BadRequest("Invalid order detail data");
            }

            var existingOrderDetail = await _orderDetailService.GetByIdAsync(orderId, productId);

            if (existingOrderDetail == null)
            {
                return NotFound();
            }

            var updatedOrderDetail = orderDetailVm.ToOrderDetail();
            updatedOrderDetail.OrderId = orderId;
            updatedOrderDetail.ProductId = productId;

            var result = await _orderDetailService.UpdateAsync(updatedOrderDetail);

            if (result > 0)
            {
                return NoContent();
            }
            return BadRequest("Failed to update order detail");
        }

        [HttpDelete("delete-orderdetail/{orderId}/{productId}")]
        public async Task<IActionResult> Delete([FromRoute] Guid orderId, [FromRoute] Guid productId)
        {
            if (orderId == Guid.Empty || productId == Guid.Empty)
            {
                return BadRequest("Invalid order or product Id");
            }

            var result = await _orderDetailService.DeleteAsync(orderId, productId);

            if (result)
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}
